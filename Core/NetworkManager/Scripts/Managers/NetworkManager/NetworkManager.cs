
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
	[RequireComponent(typeof(ConfigurationManager))]
	[RequireComponent(typeof(OpenMMO.Network.NetworkAuthenticator))]
	[DisallowMultipleComponent]
	public partial class NetworkManager : BaseNetworkManager
	{
		
		[Header("System Texts")]
		public NetworkManager_Lang systemText;
		
		[Header("Event Listeners")]
		public NetworkManager_Events eventListeners;
		
		// -------------------------------------------------------------------------------
		
		public static new OpenMMO.Network.NetworkManager singleton;

		public static Dictionary<string, GameObject> onlinePlayers = new Dictionary<string, GameObject>();
		protected Dictionary<NetworkConnection, string> onlineUsers = new Dictionary<NetworkConnection, string>();
		
		[HideInInspector]public string userName 	= "";
        [HideInInspector]public string userPassword	= "";
        [HideInInspector]public string newPassword	= "";
        [HideInInspector]public List<PlayerPreview> playerPreviews = new List<PlayerPreview>();
		[HideInInspector]public int maxPlayers				= 0;
		
		// You can use this to show/hide UI elements based on the network state (state == NetworkState.Game)
		[HideInInspector]public NetworkState state = NetworkState.Offline;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		public override void Awake()
		{
			singleton = this;
			base.Awake(); // required
			
			this.InvokeInstanceDevExtMethods("AwakePriority"); // must be first
			
			// -- decide how to start
#if _SERVER && _CLIENT
			StartHost();
#elif _SERVER
			StartServer();
#else
			StartClient();
#endif
			
			this.InvokeInstanceDevExtMethods(nameof(Awake)); // must be last
			
		}

		// -------------------------------------------------------------------------------
		public override void Start()
		{
			base.Start(); // required
			this.InvokeInstanceDevExtMethods(nameof(Start));
		}
		
		// -------------------------------------------------------------------------------
		void Update()
		{
			if (ClientScene.localPlayer != null)
				state = NetworkState.Game;
		}
		
		// -------------------------------------------------------------------------------
		public bool AccountLoggedIn(string _name)
		{
			foreach (KeyValuePair<string, GameObject> player in onlinePlayers)
				if (player.Value.name == _name) return true;
			
			return false;
		}
		
		// -------------------------------------------------------------------------------
		public string GetUserName(NetworkConnection conn)
		{
			foreach (KeyValuePair<NetworkConnection, string> user in onlineUsers)
				if (user.Key == conn) return user.Value;
			
			return "";
		}
		
		// -------------------------------------------------------------------------------
		public void ServerSendError(NetworkConnection conn, string error, bool disconnect)
		{
			conn.Send(new ErrorMsg{text=error, causesDisconnect=disconnect});
		}
		
		// -------------------------------------------------------------------------------
		void OnClientError(NetworkConnection conn, ErrorMsg message)
		{
			
			if (!String.IsNullOrWhiteSpace(message.text))
				UIPopupConfirm.singleton.Init(message.text);
			
			if (message.causesDisconnect)
			{
				conn.Disconnect();
				if (NetworkServer.active) StopHost();
			}
		}
		
		// -------------------------------------------------------------------------------
		public override void OnStopServer()
		{
			// -- this closes the database connection
			DatabaseManager.singleton.Destruct();	
		}
		
		// -------------------------------------------------------------------------------
		public bool IsConnecting() => NetworkClient.active && !ClientScene.ready;
		
		// -------------------------------------------------------------------------------
		public override void OnClientConnect(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnClientConnect), conn);
		}
		
		// -------------------------------------------------------------------------------
		public override void OnServerConnect(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnServerConnect), conn);
		}
		
		// -------------------------------------------------------------------------------
		public override void OnClientSceneChanged(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnClientSceneChanged), conn);
		}

		// -------------------------------------------------------------------------------
		public override void OnServerAddPlayer(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnServerAddPlayer), conn);
		}
	
		// -------------------------------------------------------------------------------
		public override void OnServerDisconnect(NetworkConnection conn)
		{

			// -- this logs out the user on the database
			DatabaseManager.singleton.LogoutUser(GetUserName(conn));

			if (conn.identity != null)
			{
			
				debug.Log("[NetworkManager] Logged out player: " + conn.identity.name);
				
				if (conn.identity.gameObject != null)
				{	
					
					this.InvokeInstanceDevExtMethods(nameof(OnServerDisconnect), conn);
					eventListeners.OnLogoutPlayer.Invoke(conn);
					
					string name = conn.identity.gameObject.name;
					onlinePlayers.Remove(name);
				}
					
			}
			
			onlineUsers.Remove(conn);
			base.OnServerDisconnect(conn);
			
		}
		
		// -------------------------------------------------------------------------------
		// OnClientDisconnect
		// @Client
		// -------------------------------------------------------------------------------
		public override void OnClientDisconnect(NetworkConnection conn)
		{
			base.OnClientDisconnect(conn);
			state = NetworkState.Offline;
			UIPopupConfirm.singleton.Init(systemText.clientDisconnected, Quit);
			this.InvokeInstanceDevExtMethods(nameof(OnClientDisconnect));
		}
		
		// -------------------------------------------------------------------------------
		// Quit
		// universal quit function
		// -------------------------------------------------------------------------------
		public void Quit()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================