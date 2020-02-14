
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.Portals;
using OpenMMO.DebugManager;
using OpenMMO.UI;

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
		public List<NetworkZoneTemplate> subZones;
		
		[Header("Settings")]
		[Tooltip("MainZone data save interval (in seconds)")]
		public float zoneIntervalMain = 10f;
		
		[Header("Debug Helper")]
		public DebugHelper debug;
		
		// -------------------------------------------------------------------------------
		
		public static PortalManager singleton;
		
		protected OpenMMO.Network.NetworkManager 	networkManager;
		protected Mirror.TelepathyTransport 		networkTransport;
	
		protected ushort originalPort;
		protected int zoneIndex 					= -1;
		protected int playersOnline;
		protected NetworkZoneTemplate currentZone	= null;
		protected string autoPlayerName 			= "";
    	protected bool autoConnectClient 			= false;
		protected string mainZoneName				= "_mainZone";
		protected const string argZoneIndex 		= "-zone";
		
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
    		
    		originalPort = networkTransport.port;
			
			SceneManager.sceneLoaded += OnSceneLoaded;
			
    		if (!active || GetIsMainZone || !GetCanSwitchZone)
    		{
    			currentZone = mainZone;
    			return;
    		}
    		
    		currentZone = subZones[zoneIndex];
    		    		
    		foreach (NetworkZoneTemplate template in subZones)
    			if (template == currentZone)
    				InitAsSubZone(template);
    		
    	}
    	
    	// ============================== GETTERS ========================================
    	
    	// -------------------------------------------------------------------------------
    	// GetIsMainZone
    	// -------------------------------------------------------------------------------
    	public bool GetIsMainZone
    	{
    		get
    		{
    			if (zoneIndex == -1)
    				zoneIndex = Tools.GetArgumentInt(argZoneIndex);
    			return (zoneIndex == -1);
    		}
    	}
    	
		// -------------------------------------------------------------------------------
    	// GetSubZoneTimeoutInterval
    	// -------------------------------------------------------------------------------
		protected float GetSubZoneTimeoutInterval
		{
			get {
				return zoneIntervalMain * currentZone.zoneTimeoutMultiplier;
			}
		}
		
    	// -------------------------------------------------------------------------------
    	// GetZonePort
    	// -------------------------------------------------------------------------------
    	protected ushort GetZonePort
    	{
    		get
    		{
    			return (ushort)(originalPort + zoneIndex + 1);
    		}
    	}
		
		// -------------------------------------------------------------------------------
    	// GetAutoConnect
    	// -------------------------------------------------------------------------------
		public bool GetAutoConnect
		{
			get
			{
				return autoConnectClient || !String.IsNullOrWhiteSpace(autoPlayerName);
			}
		}
		
		// -------------------------------------------------------------------------------
    	// GetCanSwitchZone
    	// -------------------------------------------------------------------------------
		public bool GetCanSwitchZone
		{
			get
			{
				if (ProjectConfigTemplate.singleton.GetNetworkType != NetworkType.HostAndPlay)
					return active;
				return false;
			}
		}
		
		// =========================== MAIN METHODS ======================================
		
    	// -------------------------------------------------------------------------------
    	// InitAsSubZone
    	// -------------------------------------------------------------------------------
    	protected void InitAsSubZone(NetworkZoneTemplate _template)
    	{
    		networkTransport.port 			= GetZonePort;
    		networkManager.networkAddress 	= _template.server.ip;
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
debug.Log("SpawnSubZones");		
			InvokeRepeating(nameof(SaveZone), 0, zoneIntervalMain);
			
			for (int i = 0; i < subZones.Count; i++)
    			if (subZones[i] != currentZone)
    				SpawnSubZone(i);
    		
		}

		// -------------------------------------------------------------------------------
    	// SpawnSubZone
    	// -------------------------------------------------------------------------------
		protected void SpawnSubZone(int index)
		{	
			
			debug.Log("Spawning Sub Zone...");
			
			// DEBUG START
			String[] args = System.Environment.GetCommandLineArgs();
			debug.Log("-->PATH: "+ Tools.GetProcessPath);
			debug.Log("-->ARGS: " + argZoneIndex + " " + index.ToString());
			// DEBUG END
		
			Process process = new Process();
			process.StartInfo.FileName 	= Tools.GetProcessPath; //Path.GetFullPath(".") + "/test.app"; //Application.dataPath; //Tools.GetProcessPath;
			process.StartInfo.Arguments = argZoneIndex + " " + index.ToString(); // Tools.GetArgumentsString + " " + argZoneIndex + " " + index.ToString();
			process.Start();
		}
		
		// ====================== MESSAGE EVENT HANDLERS =================================
		
		// -------------------------------------------------------------------------------
    	// OnServerMessageResponsePlayerSwitchServer
    	// @Client
    	// -------------------------------------------------------------------------------
		public void OnServerMessageResponsePlayerSwitchServer(NetworkConnection conn, ServerMessageResponsePlayerSwitchServer msg)
		{
			
			if (NetworkServer.active)
				return;
			
			networkManager.StopClient();
			
			NetworkClient.Shutdown();
			OpenMMO.Network.NetworkManager.Shutdown();
			OpenMMO.Network.NetworkManager.singleton = networkManager;
			
			autoPlayerName = msg.playername;
			
			for (int i = 0; i < subZones.Count; i++)
    		{
				if (msg.zonename == subZones[i].name)
				{
					zoneIndex = i;
					networkTransport.port = GetZonePort;
					autoConnectClient = true;
					Invoke(nameof(ReloadScene), 0.25f);
					return;
				}
				
			}
			
		}
		
		// -------------------------------------------------------------------------------
    	// OnServerMessageResponsePlayerAutoLogin
    	// @Client
    	// -------------------------------------------------------------------------------
        public void OnServerMessageResponsePlayerAutoLogin(NetworkConnection conn, ServerMessageResponsePlayerAutoLogin msg)
        {
        	
        	autoPlayerName = "";
        	
        	if (UIPopupNotify.singleton)
				UIPopupNotify.singleton.Hide();
        }
		
		// ======================  =================================
		
		// -------------------------------------------------------------------------------
    	// ReloadScene
    	// @Client
    	// -------------------------------------------------------------------------------
		void ReloadScene()
		{
			SceneManager.LoadScene(subZones[zoneIndex].scene.SceneName);
		}
		
		// -------------------------------------------------------------------------------
    	// OnSceneLoaded
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
		
			if (NetworkServer.active)
				if (currentZone.scene.SceneName == scene.name && GetSubZoneTimeoutInterval > 0)
					InvokeRepeating(nameof(CheckSubZone), GetSubZoneTimeoutInterval, GetSubZoneTimeoutInterval);
		
			if (autoConnectClient)
			{
				networkManager.StartClient();
				autoConnectClient = false;
			}
		
		}
		
		// -------------------------------------------------------------------------------
    	// AutoLogin
    	// @Client
    	// -------------------------------------------------------------------------------
		public void AutoLogin()
		{
			networkManager.TryAutoLoginPlayer(autoPlayerName);
		}
		
		// ================================= OTHER =======================================
		
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
    		if (DatabaseManager.singleton.LoadZoneTime(mainZoneName) > GetSubZoneTimeoutInterval)
    			Application.Quit();
    	}
    	
        // -------------------------------------------------------------------------------
    	// OnDestroy
    	// -------------------------------------------------------------------------------
        void OnDestroy()
        {
        	CancelInvoke();
        }
        
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================