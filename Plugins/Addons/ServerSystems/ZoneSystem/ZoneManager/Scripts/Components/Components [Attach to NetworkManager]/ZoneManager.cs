
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
using OpenMMO.Zones;
using OpenMMO.Debugging;
using OpenMMO.UI;

namespace OpenMMO.Zones
{

	// ===================================================================================
	// ZoneManager
	// ===================================================================================
	[RequireComponent(typeof(OpenMMO.Network.NetworkManager))]
	[RequireComponent(typeof(Mirror.TelepathyTransport))]
	[DisallowMultipleComponent]
	public partial class ZoneManager : MonoBehaviour
	{
	
		[Header("Options")]
		public bool active;
		
		[Header("Network Zones")]
		public NetworkZoneTemplate mainZone;
		public List<NetworkZoneTemplate> subZones;
		
		[Header("Settings")]
		[Tooltip("MainZone data save interval (in seconds)")]
		public float zoneIntervalMain = 60f;
		
		[Header("Debug Helper")]
		public DebugHelper debug = new DebugHelper();
		
		// -------------------------------------------------------------------------------
		
		public static ZoneManager singleton;
		
		protected OpenMMO.Network.NetworkManager 	networkManager;
		protected Mirror.TelepathyTransport 		networkTransport;
	
		protected ushort originalPort;
		protected int zoneIndex 					= -1;
		protected int securityToken 				= 0;
		protected NetworkZoneTemplate currentZone	= null;
		protected string mainZoneName				= "_mainZone";
		protected const string argZoneIndex 		= "-zone";
		protected string autoPlayerName 			= "";
		protected bool autoConnectClient 			= false;
		protected bool spawnedSubZones				= false;
		
		// -------------------------------------------------------------------------------
    	// Awake
    	// -------------------------------------------------------------------------------
		void Awake()
    	{

    		singleton = this;
    		
    		networkManager 		= GetComponent<OpenMMO.Network.NetworkManager>();
    		networkTransport 	= GetComponent<Mirror.TelepathyTransport>();
    		
    		originalPort = networkTransport.port;
			
			SceneManager.sceneLoaded += OnSceneLoaded;
			
    		if (!active || GetIsMainZone || !GetCanSwitchZone)
    		{
    			currentZone = mainZone;
    			debug.LogFormat(this.name, nameof(Awake), "mainZone"); //DEBUG
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
    	// GetZonePort
    	// -------------------------------------------------------------------------------
    	public ushort GetZonePort
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
		
		// -------------------------------------------------------------------------------
		// GetToken
		// Fetches the current token or generates a new one if its 0.
		// -------------------------------------------------------------------------------
		public int GetToken
		{
			get
			{
				return securityToken;
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
		
		// ================================ OTHER PUBLIC =================================
		
		// -------------------------------------------------------------------------------
		// RefreshToken
		// Generates a new security token for server switch.
		// -------------------------------------------------------------------------------
		public void RefreshToken()
		{
			securityToken = UnityEngine.Random.Range(1000,9999);
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
    		
    		debug.LogFormat(this.name, nameof(InitAsSubZone), _template.name); //DEBUG
    	}
    	
		// -------------------------------------------------------------------------------
    	// SpawnSubZones
    	// -------------------------------------------------------------------------------
		public void SpawnSubZones()
		{
DebugManager.Log(">>>>spawn subzones");
			if (!GetIsMainZone || !GetCanSwitchZone || spawnedSubZones)
				return;

			InvokeRepeating(nameof(SaveZone), 0, zoneIntervalMain);
			
			for (int i = 0; i < subZones.Count; i++)
    			if (subZones[i] != currentZone)
    				SpawnSubZone(i);
    		
    		spawnedSubZones = true;
    		
    		debug.LogFormat(this.name, nameof(SpawnSubZones), subZones.Count.ToString()); //DEBUG
    		
		}

        // -------------------------------------------------------------------------------
        // SpawnSubZone
        // -------------------------------------------------------------------------------
        public enum AppExtension { exe, x86_64, app }
        protected void SpawnSubZone(int index)
        {
            AppExtension extension = AppExtension.exe;
            switch (Application.platform)
            {
                //STANDALONE
                case RuntimePlatform.WindowsPlayer: extension = AppExtension.exe; break;
                case RuntimePlatform.OSXPlayer: extension = AppExtension.app; break;
                case RuntimePlatform.LinuxPlayer: extension = AppExtension.x86_64; break;
                //EDITOR
                case RuntimePlatform.WindowsEditor: extension = AppExtension.exe; break;
                case RuntimePlatform.OSXEditor: extension = AppExtension.app; break;
                case RuntimePlatform.LinuxEditor: extension = AppExtension.x86_64; break;
                    //MOBILE + CONSOLE
                    /* //NOTE: Probably no reason to use these for a server...maybe in some cases though (couch co-op games etc)
                    case RuntimePlatform.WebGLPlayer: break; case RuntimePlatform.IPhonePlayer: break; case RuntimePlatform.Android: break;
                    case RuntimePlatform.PS4: break; case RuntimePlatform.XboxOne: break; case RuntimePlatform.Switch: break;*/
            }
            Process process = new Process();
            //NOTE: This is a workaround, we leave the switch logic above just in case we ever check this extension variable in the future to extend this method.
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                process.StartInfo.FileName = Tools.GetProcessPath; //OSX
                //TODO: This is a potential fix for the server executable being locked into requiring a specific name.
                //process.StartInfo.FileName = Path.GetFullPath(Tools.GetProcessPath) + "/" + Path.GetFileNameWithoutExtension(Tools.GetProcessPath) + "." + Path.GetExtension(Tools.GetProcessPath);

            }
            else
            {
                process.StartInfo.FileName = "server" + "." + extension.ToString(); //LINUX and WINDOWS
                //TODO: This is a potential fix for the server executable being locked into requiring a specific name.
                //process.StartInfo.FileName = Path.GetFileNameWithoutExtension(Tools.GetProcessPath) + "." + Path.GetExtension(Tools.GetProcessPath);
            }
            process.StartInfo.Arguments = argZoneIndex + " " + index.ToString();
            process.Start();

            debug.LogFormat(this.name, nameof(SpawnSubZone), index.ToString()); //DEBUG

        }

        // ====================== MESSAGE EVENT HANDLERS =================================

        // -------------------------------------------------------------------------------
        // OnServerMessageResponsePlayerSwitchServer
        // @Client
        // -------------------------------------------------------------------------------
        public void OnServerMessageResponsePlayerSwitchServer(NetworkConnection conn, ServerMessageResponsePlayerSwitchServer msg)
		{
			
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
					
					debug.LogFormat(this.name, nameof(OnServerMessageResponsePlayerSwitchServer), i.ToString()); //DEBUG
					
					return;
				}
				
			}
			
			debug.LogFormat(this.name, nameof(OnServerMessageResponsePlayerSwitchServer), "NOT FOUND"); //DEBUG
			
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
		
		// ===================================  OTHER ====================================
		
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
		
			if (NetworkServer.active && !GetIsMainZone)
				if (GetSubZoneTimeoutInterval > 0)
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
			if (!String.IsNullOrWhiteSpace(autoPlayerName))
				networkManager.TryAutoLoginPlayer(autoPlayerName, GetToken);
		}
		
		// ================================= OTHER =======================================
		
    	// -------------------------------------------------------------------------------
    	// SaveZone
    	// @Server
    	// -------------------------------------------------------------------------------
    	void SaveZone()
    	{
    		DatabaseManager.singleton.SaveZoneTime(mainZoneName);
    	}
    	
    	// -------------------------------------------------------------------------------
    	// CheckSubZone
    	// @Server
    	// -------------------------------------------------------------------------------
    	void CheckSubZone()
    	{
    		if (DatabaseManager.singleton.CheckZoneTimeout(mainZoneName, GetSubZoneTimeoutInterval))
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