
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.Portals;
using OpenMMO.DebugManager;

namespace OpenMMO.Portals
{

	// ===================================================================================
	// PortalManager
	// ===================================================================================
	[RequireComponent(typeof(OpenMMO.NetworkManager))]
	[RequireComponent(typeof(Mirror.TelepathyTransport))]
	[DisallowMultipleComponent]
	public partial class PortalManager : MonoBehaviour
	{
	
		[Header("Options")]
		public bool active;
		
		[Header("Network Zones")]
		public NetworkZoneTemplate[] networkZones;
		
		[Header("Settings")]
		[Tooltip("MainZone data save interval (in seconds)")]
		public float zoneIntervalMain = 10f;
		
		[Header("Debug Helper")]
		public DebugHelper debug;
		
		// -------------------------------------------------------------------------------
		
		public static PortalManager singleton;
		
		protected OpenMMO.Network.NetworkManager 	networkManager;
		protected Mirror.TelepathyTransport 		networkTransport;
	
		[HideInInspector] public bool isSubZone;
		
		protected ushort originalPort;
		protected string zoneName = "";
		protected int zoneIndex = -1;
		protected int playersOnline;
		protected float zoneTimeoutMultiplier;
		
		public static List<PortalAnchorEntry> portalAnchors = new List<PortalAnchorEntry>();
		
		public static string autoSelectPlayer = "";
    	public static bool autoConnectClient;
		
		protected string mainZoneName	= "_mainZone";
		protected string argZoneIndex 	= "zone";
		
		// -------------------------------------------------------------------------------
    	// Awake
    	// -------------------------------------------------------------------------------
		void Awake()
    	{
    	
    		singleton = this;
    		
    		debug = new DebugHelper();
			debug.Init();
    		
    		networkManager 		= GetComponent<OpenMMO.Network.NetworkManager>();
    		networkTransport 	= GetComponent<Mirror.TelepathyTransport>();
    		
    		if (!active || GetIsMainZone || networkManager == null || networkTransport == null)
    			return;
    			
    		int tmpIndex = 0;
    			
    		foreach (NetworkZoneTemplate template in networkZones)
    		{
    		
    			tmpIndex++;
    			
    			if (zoneIndex == tmpIndex)
    				InitAsSubZone(template, tmpIndex);
    		}
    				
    	}
    	
		// -------------------------------------------------------------------------------
    	// SubZoneTimeoutInterval
    	// -------------------------------------------------------------------------------
		public float SubZoneTimeoutInterval
		{
			get {
				return zoneIntervalMain * zoneTimeoutMultiplier;
			}
		}
		
    	// -------------------------------------------------------------------------------
    	// InitAsSubZone
    	// -------------------------------------------------------------------------------
    	protected void InitAsSubZone(NetworkZoneTemplate _template, int zoneIndex)
    	{
    		isSubZone 						= true;
    		zoneName						= _template.title;
    		zoneTimeoutMultiplier			= _template.zoneTimeoutMultiplier;
    		networkTransport.port 			= (ushort)(originalPort + zoneIndex);
    		networkManager.onlineScene 		= _template.scene.SceneName;
    		networkManager.StartServer();
    	}
    	
		// -------------------------------------------------------------------------------
    	// SpawnSubZones
    	// -------------------------------------------------------------------------------
		public void SpawnSubZones()
		{
		
			if (!GetIsMainZone) return;
debug.Log("SpawnSubZones");
			InvokeRepeating(nameof(SaveZone), 0, zoneIntervalMain);
			
			int tmpIndex = 0;
			
			foreach (NetworkZoneTemplate template in networkZones)
    		{
    			
    			if (zoneIndex != tmpIndex)
    				SpawnSubZone(template, tmpIndex);
    			
    			tmpIndex++;
    				
    		}
		
		}

		// -------------------------------------------------------------------------------
    	// SpawnSubZone
    	// -------------------------------------------------------------------------------
		protected void SpawnSubZone(NetworkZoneTemplate _template, int zoneIndex)
		{
			Process process = new Process();
			process.StartInfo.FileName = Tools.GetProcessPath;
			process.StartInfo.Arguments = Tools.GetArgumentsString + " -" + argZoneIndex + zoneIndex.ToString();
			process.Start();
		}
		
		// -------------------------------------------------------------------------------
    	// OnClientMessageRequestPlayerSwitchServer
    	// -------------------------------------------------------------------------------
		public void OnClientMessageRequestPlayerSwitchServer(NetworkConnection conn, ClientMessageRequestPlayerSwitchServer msg)
		{
			
			if (NetworkServer.active)
				return;
			
			networkManager.StopClient();
			NetworkClient.Shutdown();
			OpenMMO.Network.NetworkManager.Shutdown();
			OpenMMO.Network.NetworkManager.singleton = networkManager;
			autoSelectPlayer = msg.playername;
			
			string sceneName = "";
			
			foreach (NetworkZoneTemplate template in networkZones)
    		{
				if (msg.zonename == template.name)
					sceneName = template.scene.SceneName;
			}
			
			SceneManager.LoadScene(sceneName);
			autoConnectClient = true;
			
		}
		
		// -------------------------------------------------------------------------------
    	// OnSceneLoaded
    	// -------------------------------------------------------------------------------
		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
		
			if (NetworkServer.active)
			{
				if (zoneName == scene.name && SubZoneTimeoutInterval > 0)
					InvokeRepeating(nameof(CheckSubZone), SubZoneTimeoutInterval, SubZoneTimeoutInterval);
			}
		
			if (autoConnectClient)
			{
				networkTransport.port = (ushort)(originalPort + zoneIndex);
				networkManager.StartClient();
				autoConnectClient = false;
			}
		
		}
		
    	// -------------------------------------------------------------------------------
    	// GetIsMainZone
    	// -------------------------------------------------------------------------------
    	protected bool GetIsMainZone
    	{
    		get
    		{
    			if (zoneIndex == -1)
    				zoneIndex = Tools.GetArgumentInt(argZoneIndex);
    			return (zoneIndex == 0);
    		}
    	}
    	
    	// -------------------------------------------------------------------------------
    	// SaveZone
    	// -------------------------------------------------------------------------------
    	void SaveZone()
    	{
    		DatabaseManager.singleton.SaveZoneTime(mainZoneName, playersOnline);
    	}
    	
    	// -------------------------------------------------------------------------------
    	// CheckSubZone
    	// -------------------------------------------------------------------------------
    	void CheckSubZone()
    	{
    		if (DatabaseManager.singleton.LoadZoneTime(mainZoneName) > SubZoneTimeoutInterval)
    			Application.Quit();
    	}
    	
        // -------------------------------------------------------------------------------
    	// OnDestroy
    	// -------------------------------------------------------------------------------
        void OnDestroy()
        {
        	CancelInvoke();
        }
        
        // ============================ PORTAL ANCHORS ===================================
        
        // -------------------------------------------------------------------------------
    	// CheckPortalAnchor
    	// -------------------------------------------------------------------------------
        public static bool CheckPortalAnchor(string _name)
        {
        	if (String.IsNullOrWhiteSpace(_name))
        		return false;
        	UnityEngine.Debug.Log(_name);
        	foreach (PortalAnchorEntry anchor in portalAnchors)
        	{
        		UnityEngine.Debug.Log(anchor.name+"/"+_name);
        		if (anchor.name == _name)
        			return true;
        	}	

			return false;
        }
        
        // -------------------------------------------------------------------------------
    	// GetPortalAnchorPosition
    	// -------------------------------------------------------------------------------
        public static Vector3 GetPortalAnchorPosition(string _name)
        {
        	foreach (PortalAnchorEntry anchor in portalAnchors)
        		if (anchor.name == _name)
					return anchor.position;
			return Vector3.zero;
        }
        
        // -------------------------------------------------------------------------------
    	// RegisterPortalAnchor
    	// -------------------------------------------------------------------------------
        public static void RegisterPortalAnchor(string _name, Vector3 _position)
        {
            portalAnchors.Add(
            				new PortalAnchorEntry
            				{
            					name = _name,
            					position = _position
            				}
            );
            
        }

        // -------------------------------------------------------------------------------
    	// UnRegisterPortalAnchor
    	// -------------------------------------------------------------------------------
        public static void UnRegisterPortalAnchor(string _name)
        {
           
           for (int i = 0; i < portalAnchors.Count; i++)
           {
           		if (portalAnchors[i].name == _name)
           			portalAnchors.RemoveAt(i);
           }
            
        }

    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================