
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
	
		//[HideInInspector] public bool isSubZone;
		
		protected ushort originalPort;
		//protected string zoneName = "";
		protected int zoneIndex = -1;
		protected int playersOnline;
		//protected float zoneTimeoutMultiplier;
		
		protected NetworkZoneTemplate currentZone;
		
		protected string autoPlayerName = "";
    	protected bool autoConnectClient;
		
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
    		
    		originalPort = networkTransport.port;
    		
    		SceneManager.sceneLoaded += OnSceneLoaded;
    		
    		if (!active || GetIsMainZone)
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
				return autoConnectClient && !String.IsNullOrWhiteSpace(autoPlayerName);
			}
		}
		
		// -------------------------------------------------------------------------------
    	// GetCanSwitchZone
    	// -------------------------------------------------------------------------------
		public bool GetCanSwitchZone
		{
			get
			{
#if !_SERVER && _CLIENT
				return true;
#else
				return false;
#endif
			}
		}
		
		// =========================== MAIN METHODS ======================================
		
    	// -------------------------------------------------------------------------------
    	// InitAsSubZone
    	// -------------------------------------------------------------------------------
    	protected void InitAsSubZone(NetworkZoneTemplate _template)
    	{
    		//isSubZone 						= true;
    		//zoneName						= _template.name;
    		//zoneTimeoutMultiplier			= _template.zoneTimeoutMultiplier;
    		//networkManager.StopServer();
    		networkTransport.port 			= GetZonePort;
debug.Log("IM LISTENING AT PORT:"+networkTransport.port);
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
    	// OnServerMessageResponsePlayerSwitchServer
    	// @Client
    	// -------------------------------------------------------------------------------
		public void OnServerMessageResponsePlayerSwitchServer(NetworkConnection conn, ServerMessageResponsePlayerSwitchServer msg)
		{
			
			if (NetworkServer.active)
				return;
			
			networkManager.StopClient();
			
			//NetworkClient.Shutdown();
			OpenMMO.Network.NetworkManager.Shutdown();
			OpenMMO.Network.NetworkManager.singleton = networkManager;
			
			
			autoPlayerName = msg.playername;
			
			for (int i = 0; i < subZones.Length; i++)
    		{
				if (msg.zonename == subZones[i].name)
				{
				
					debug.Log("Loading scene: "+subZones[i].scene.SceneName);
					
					zoneIndex = i;
					
					autoConnectClient = true;
					
					//networkTransport.Shutdown();
					networkTransport.port = GetZonePort;
				
				debug.Log("Auto connecting to port:"+GetZonePort);
				
					
					
				debug.Log("Network is active/inactive: "+networkManager.isNetworkActive);
					Invoke(nameof(ReloadScene), 1f);
					
					return;
				}
				
			}
			
			
			
		}
		
		// -------------------------------------------------------------------------------
    	// ReloadScene
    	// @Client
    	// -------------------------------------------------------------------------------
		void ReloadScene()
		{
			networkManager.StartClient();
			debug.Log("Network is active/inactive: "+networkManager.isNetworkActive);
			SceneManager.LoadScene(subZones[zoneIndex].scene.SceneName);
		}
		
		// -------------------------------------------------------------------------------
    	// OnSceneLoaded
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
		
		debug.Log("SCENE LOADED");
		
			if (NetworkServer.active)
				if (currentZone.scene.SceneName == scene.name && GetSubZoneTimeoutInterval > 0)
					InvokeRepeating(nameof(CheckSubZone), GetSubZoneTimeoutInterval, GetSubZoneTimeoutInterval);
		
			if (autoConnectClient)
			{
				
				debug.Log("NetworkClient: "+ NetworkClient.isConnected);
				debug.Log("NetworkManager: "+networkManager.isNetworkActive);
				
				OpenMMO.Network.NetworkAuthenticator.singleton.ClientAuthenticate();
				
				networkManager.TryAutoLoginPlayer(autoPlayerName);
				
				autoConnectClient = false;
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