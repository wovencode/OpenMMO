
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
	[RequireComponent(typeof(OpenMMO.Network.NetworkManager))]
	[RequireComponent(typeof(Mirror.TelepathyTransport))]
	[DisallowMultipleComponent]
	public partial class PortalManager : MonoBehaviour
	{
	
		[Header("Options")]
		public bool active;
		
		[Header("Network Zones")]
		public NetworkZoneTemplate mainZone;
		public NetworkZoneTemplate[] subZones;
		
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
		
		protected NetworkZoneTemplate currentZone;
		
		public static List<PortalAnchorEntry> portalAnchors = new List<PortalAnchorEntry>();
		
		public static string autoSelectPlayer = "";
    	public static bool autoConnectClient;
		
		protected string mainZoneName			= "_mainZone";
		protected const string argZoneIndex 	= "zone";
		
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
    		
    		if (!active || GetIsMainZone)
    		{
    			currentZone = mainZone;
    			return;
    		}
    		
    		currentZone = subZones[zoneIndex];
    		    		
    		foreach (NetworkZoneTemplate template in subZones)
    		{
    			if (template == currentZone)
    				InitAsSubZone(template);
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
    	protected void InitAsSubZone(NetworkZoneTemplate _template)
    	{
    		isSubZone 						= true;
    		zoneName						= _template.scene.SceneName;
    		zoneTimeoutMultiplier			= _template.zoneTimeoutMultiplier;
    		networkTransport.port 			= (ushort)(originalPort + zoneIndex + 1);
    		networkManager.onlineScene 		= _template.scene.SceneName;
    		networkManager.StartServer();
    	}
    	
		// -------------------------------------------------------------------------------
    	// SpawnSubZones
    	// -------------------------------------------------------------------------------
		public void SpawnSubZones()
		{

			if (!GetIsMainZone || !active)
				return;
			
			InvokeRepeating(nameof(SaveZone), 0, zoneIntervalMain);
			
			for (int i = 0; i < subZones.Length; i++)
    			if (subZones[i] != currentZone)
    				SpawnSubZone(i);
    		
		}

		// -------------------------------------------------------------------------------
    	// SpawnSubZone
    	// -------------------------------------------------------------------------------
		protected void SpawnSubZone(int index)
		{
			Process process = new Process();
			process.StartInfo.FileName = Tools.GetProcessPath;
			process.StartInfo.Arguments = Tools.GetArgumentsString + " " + argZoneIndex + " " + index.ToString();
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
			
			foreach (NetworkZoneTemplate template in subZones)
    		{
				if (msg.zonename == template.name)
				{
					SceneManager.LoadScene(msg.zonename);
					autoConnectClient = true;
					return;
				}
				
			}
			
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
    			return (zoneIndex == -1);
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