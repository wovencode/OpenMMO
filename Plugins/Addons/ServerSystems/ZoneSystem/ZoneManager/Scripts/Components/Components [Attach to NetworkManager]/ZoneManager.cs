//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Zones;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.Debugging;
using OpenMMO.UI;

namespace OpenMMO.Zones
{

	// ===================================================================================
	// ZoneManager
	// ===================================================================================
	//[RequireComponent(typeof(OpenMMO.Network.NetworkManager))]
	//[RequireComponent(typeof(Mirror.TelepathyTransport))]
	[DisallowMultipleComponent]
	public partial class ZoneManager : MonoBehaviour
	{
        const int DEFAULT_PORT = 9999;
	
		[Header("Options")]
		public bool active;

        public ZoneConfigTemplate zoneConfig;
		
		[Header("Debug Helper")]
		public DebugHelper debug = new DebugHelper();
		
		// -------------------------------------------------------------------------------
		
		public static ZoneManager singleton;
		
		[SerializeField] protected Network.NetworkManager networkManager;
        [SerializeField] protected Transport networkTransport;
	
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

            networkManager = null; //RESET COMPONENT
            networkTransport = null; //RESET COMPONENT
            //if (!networkManager) networkManager = FindObjectOfType<Network.NetworkManager>();

            OnValidate(); //LOADS MISSING COMPONENTS (The ones we just reset)

            originalPort = GetTransportPort();
            //originalPort = networkTransport.port; //REPLACED - DX4D
			
			SceneManager.sceneLoaded += OnSceneLoaded;
			
    		//if (!active || GetIsMainZone || !GetCanSwitchZone) //REMOVED - GetCanSwitchZone already checks active - DX4D
    		if (GetIsMainZone || !GetCanSwitchZone)
    		{
    			currentZone = zoneConfig.mainZone;
    			debug.LogFormat(this.name, nameof(Awake), "mainZone"); //DEBUG
    			return;
    		}
    		
    		currentZone = zoneConfig.subZones[zoneIndex];

            foreach (NetworkZoneTemplate template in zoneConfig.subZones)
            {
                if (template == currentZone)
                {
                    InitAsSubZone(template);
                }
            }
    	}

        private void OnValidate()
        {
            if (!zoneConfig) zoneConfig = Resources.Load<ZoneConfigTemplate>("ZoneConfig/DefaultZoneConfig");

            //NETWORK MANAGER
            if (!networkManager) networkManager = GetComponent<Network.NetworkManager>();
            if (!networkManager) networkManager = FindObjectOfType<Network.NetworkManager>();

            //KCP
            if (!networkTransport) networkTransport = GetComponent<kcp2k.KcpTransport>();
            if (!networkTransport) networkTransport = FindObjectOfType<kcp2k.KcpTransport>();
            //TELEPATHY
            if (!networkTransport) networkTransport = GetComponent<TelepathyTransport>();
            if (!networkTransport) networkTransport = FindObjectOfType<TelepathyTransport>();
        }

        // ============================== GETTERS ========================================

        //GET TRANSPORT PORT
        //@SERVER
        ushort GetTransportPort()
        {
            //KCP TRANSPORT - GET PORT
            if (networkTransport is kcp2k.KcpTransport)
            {
                return (networkTransport as kcp2k.KcpTransport).Port;
            }
            //TELEPATHY TRANSPORT - GET PORT
            if (networkTransport is TelepathyTransport)
            {
                return (networkTransport as TelepathyTransport).port;
            }

            return DEFAULT_PORT; //NO TRANSPORTS - USE DEFAULT PORT
        }
        //UPDATE TRANSPORT PORT
        //@SERVER @CLIENT
        void UpdateTransportPort(ushort newPort)
        {
            const string SPACER = " - ";
            string TRANSPORT = "[UNDEFINED TRANSPORT]";
#if _CLIENT
            const string HEADER = "<b>[<color=green>CLIENT STARTUP</color>]</b>]";
#elif _SERVER
            const string HEADER = "[SERVER STARTUP]";
#else
            const string HEADER = "[STANDALONE STARTUP]";
#endif

            UnityEngine.Debug.Log(HEADER + SPACER
                + "Assigning port " + newPort + " to " + networkTransport.name + "...");

            //KCP TRANSPORT
            if (networkTransport is kcp2k.KcpTransport)
            {
                TRANSPORT = "[KCP Transport]";
                (networkTransport as kcp2k.KcpTransport).Port = newPort;
            }
            //TELEPATHY TRANSPORT
            else if (networkTransport is TelepathyTransport)
            {
                TRANSPORT = "[Telepathy Transport]";
                (networkTransport as TelepathyTransport).port = newPort;
            }

            UnityEngine.Debug.Log(HEADER + TRANSPORT + SPACER
                + "Port set to " + newPort + "!");
            //TODO: ADD MORE TRANSPORTS
            //TODO: There should be a better way to do this...
        }
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
                if (ServerConfigTemplate.singleton.GetNetworkType == NetworkType.HostAndPlay)
                {
                    return false;
                }
                return active;
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
				return zoneConfig.zoneSaveInterval * currentZone.zoneTimeoutMultiplier;
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
            //UPDATE TRANSPORTS
            UpdateTransportPort(GetZonePort);
            //networkTransport.port = GetZonePort; //REPLACED - DX4D

            networkManager.networkAddress 	= _template.server.ip;
    		networkManager.onlineScene 		= _template.scene.SceneName;

            UnityEngine.Debug.Log("[SERVER STARTUP] - "
                + "Loading Zone " + _template.title + " @" + _template.server.ip);
            networkManager.StartServer();
            UnityEngine.Debug.Log("[SERVER STARTUP] - "
                + _template.title + " loaded @" + _template.server.ip);

            debug.LogFormat(this.name, nameof(InitAsSubZone), _template.name); //DEBUG
    	}
    	
		// -------------------------------------------------------------------------------
    	// SpawnSubZones
    	// -------------------------------------------------------------------------------
		public void SpawnSubZones()
		{
DebugManager.Log(">>>>spawn subzones");
			if (!GetIsMainZone || !GetCanSwitchZone || spawnedSubZones) return;

			InvokeRepeating(nameof(SaveZone), 0, zoneConfig.zoneSaveInterval);

            for (int i = 0; i < zoneConfig.subZones.Count; i++)
            {
                if (zoneConfig.subZones[i] != currentZone)
                {
                    UnityEngine.Debug.Log("[SERVER STARTUP] - Spawned Sub-Zone - " + zoneConfig.subZones[i].name);
                    SpawnSubZone(i);
                }
            }
    		
    		spawnedSubZones = true;
    		
    		debug.LogFormat(this.name, nameof(SpawnSubZones), zoneConfig.subZones.Count.ToString()); //DEBUG
    		
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
            string filePath = process.StartInfo.WorkingDirectory + process.StartInfo.FileName;

            //UnityEngine.Debug.Log("[SERVER] - Zone Server - Launching Server @" + filePath + "...");

            if (!System.IO.File.Exists( process.StartInfo.FileName))
            {
                UnityEngine.Debug.Log("[SERVER ISSUE] - Zone Server - Could not find server file @" + filePath);
            }
            else
            {
                UnityEngine.Debug.Log("[SERVER STARTUP] - Zone Server - Launching Zone " + zoneConfig.subZones[index].title + " @" + filePath);
            }

            //NOTE: We started anyway, even if the file does not exist
            //TODO: Make this conditional (once the above condition is tested)
            process.Start(); //TODO: Validate that File Exists - DONE

            debug.LogFormat(this.name, nameof(SpawnSubZone), index.ToString()); //DEBUG

        }

        // ====================== MESSAGE EVENT HANDLERS =================================

        // -------------------------------------------------------------------------------
        // OnServerMessageResponsePlayerSwitchServer
        // @Client
        // -------------------------------------------------------------------------------
        //public void OnServerMessageResponsePlayerSwitchServer(NetworkConnection conn, ServerResponsePlayerSwitchServer msg) //REMOVED - DX4D
        public void OnServerMessageResponsePlayerSwitchServer(ServerResponsePlayerSwitchServer msg) //ADDED - DX4D
		{
            Network.NetworkManager newNetworkManager = networkManager;
            if (!newNetworkManager)
            {
                UnityEngine.Debug.Log("<b>[<color=blue>CLIENT</color>]</b> - "
                    + "<b>Creating New Network Manager...</b>");
                newNetworkManager = new Network.NetworkManager();
            }
			
            UnityEngine.Debug.Log("<b>[<color=blue>CLIENT</color>]</b> - "
                + "<b>Stopping Client connected to Network Manager...</b>");
			if (networkManager) networkManager.StopClient();

            UnityEngine.Debug.Log("<b>[<color=blue>CLIENT</color>]</b> - "
                + "<b>Shutting Down Network Client on the current Server...</b>");
			NetworkClient.Shutdown();


            UnityEngine.Debug.Log("<b>[<color=blue>CLIENT</color>]</b> - "
                + "<b>Shutting Down Active Network Manager...</b>");
            Network.NetworkManager.Shutdown();

            UnityEngine.Debug.Log("<b>[<color=blue>CLIENT</color>]</b> - "
                + "<b>Setting New Network Manager...</b>");
            Network.NetworkManager.singleton = newNetworkManager;//networkManager;
			
			autoPlayerName = msg.playername;
			
			for (int i = 0; i < zoneConfig.subZones.Count; i++)
    		{
				if (msg.zonename == zoneConfig.subZones[i].name)
                {
                    zoneIndex = i; //MOVED - Before UpdateTransportPort - DX4D
					autoConnectClient = true;

                    //UPDATE TRANSPORTS
                    UpdateTransportPort(GetZonePort);
                    //networkTransport.port = GetZonePort; //REPLACED - DX4D

                    UnityEngine.Debug.Log("<b>[<color=blue>CLIENT</color>]</b> - "
                        + "<b>Loading zone number " + zoneIndex + " - " + msg.zonename + "...</b>");

                    LoadSubZone(zoneIndex);


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
        //public void OnServerMessageResponsePlayerAutoLogin(NetworkConnection conn, ServerResponsePlayerAutoLogin msg) //REMOVED - DX4D
        public void OnServerMessageResponsePlayerAutoLogin(ServerResponsePlayerAutoLogin msg) //ADDED - DX4D
        {
        	
        	autoPlayerName = "";
        	
        	if (UIPopupNotify.singleton)
				UIPopupNotify.singleton.Hide();
        }

        // ===================================  OTHER ====================================

        // -------------------------------------------------------------------------------
        // ReloadSubZone
        // @Client
        // -------------------------------------------------------------------------------
        int subZoneIndex = 0;
        void LoadSubZone(int zoneIndex)
        {
            subZoneIndex = zoneIndex;
            Invoke(nameof(ReloadSubZone), 0.25f);
        }
		void ReloadSubZone()
        {
            string zoneName = zoneConfig.subZones[subZoneIndex].scene.SceneName;

            SceneManager.LoadScene(zoneName); //LOAD ZONE TO CURRENT SCENE
            
            UnityEngine.Debug.Log("<b>[<color=green>CLIENT</color>]</b> - "
                + "<b>Loaded zone number " + zoneIndex + " - " + zoneName + "!</b>");
		}
		
		// -------------------------------------------------------------------------------
    	// OnSceneLoaded
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{

            if (NetworkServer.active && !GetIsMainZone)
            {
                if (GetSubZoneTimeoutInterval > 0)
                {
                    InvokeRepeating(nameof(CheckSubZone), GetSubZoneTimeoutInterval, GetSubZoneTimeoutInterval);
                }
            }
		
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
		public void AutoLogin(NetworkConnection conn)
		{
			if (!String.IsNullOrWhiteSpace(autoPlayerName))
				networkManager.TryAutoLoginPlayer(conn, autoPlayerName, GetToken);
		}
		
		// ================================= OTHER =======================================
		
    	// -------------------------------------------------------------------------------
    	// SaveZone
    	// @Server
    	// -------------------------------------------------------------------------------
    	void SaveZone()
        {
            UnityEngine.Debug.Log("[SERVER SAVE] - "
                + "Saving Zone " + mainZoneName + "...");
            DatabaseManager.singleton.SaveZoneTime(mainZoneName); //SAVE ZONE TIME
            UnityEngine.Debug.Log("[SERVER SAVE] - "
                + "Saved Zone " + mainZoneName + "!");
        }
    	
    	// -------------------------------------------------------------------------------
    	// CheckSubZone
    	// @Server
    	// -------------------------------------------------------------------------------
    	void CheckSubZone()
    	{
            if (DatabaseManager.singleton.CheckZoneTimeout(mainZoneName, GetSubZoneTimeoutInterval))
            {
                UnityEngine.Debug.Log("[SERVER ZONE TIMEOUT] - "
                    + "Zone number " + subZoneIndex + " - " + zoneConfig.subZones[subZoneIndex].scene.SceneName + "'s master zone " + mainZoneName + " has gone offline...shutting down!");
                Application.Quit();
            }
    	}
    	
        // -------------------------------------------------------------------------------
    	// OnDestroy
    	// -------------------------------------------------------------------------------
        void OnDestroy()
        {
        	CancelInvoke();
        }
	}
}
