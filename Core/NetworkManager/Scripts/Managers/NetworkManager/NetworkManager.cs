
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.UI;
using OpenMMO.Portals;
using System;
using System.Linq;
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
    /// <summary>
    /// Public Partial class <c>NetworkManager</c> inherits from <c>BaseNetworkManager</c>.
    /// Contains All the network functionality.
    /// </summary>
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
        // AwakePriority
        // -------------------------------------------------------------------------------
        /// <summary>
        /// <c>AwakePriority</c> function that is called at the beginning of the awake method before others are called.
        /// </summary>
        public virtual void AwakePriority()
        {
			this.InvokeInstanceDevExtMethods(nameof(AwakePriority)); //HOOK
        }

		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        /// <summary>
        /// <c>Awake</c> function that sets the singleton and starts the network manager.
        /// </summary>
		public override void Awake()
		{
			singleton = this;
			base.Awake(); // required

            AwakePriority(); // must be first
			
			// -- decide how to start
#if _SERVER && _CLIENT
			StartHost();
#elif _SERVER
			if (GetComponent<PortalManager>() != null && GetComponent<PortalManager>().GetIsMainZone)
			StartServer();
#else
			StartClient();
#endif
			
			this.InvokeInstanceDevExtMethods(nameof(Awake)); //HOOK // must be last
			
		}

		// -------------------------------------------------------------------------------
        /// <summary>
        /// Function that runs on Start after awake. Runs the base start inherited from the <c>BaseNetworkManager</c>.
        /// The start function is required to run the base start.
        /// </summary>
		public override void Start()
		{
			base.Start(); // required
			this.InvokeInstanceDevExtMethods(nameof(Start)); //HOOK
		}
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Function that runs on Update.
        /// Checks if the client scene is not the local player and sets the <c>NetworkState</c> to <c>NetworkState.Game</c>
        /// </summary>
		void Update()
		{
			if (ClientScene.localPlayer != null)
				state = NetworkState.Game;
		}
		
		// -------------------------------------------------------------------------------
		// GetIsUserLoggedIn
		// -------------------------------------------------------------------------------
		public bool GetIsUserLoggedIn(string userName)
		{
		
			bool online = false;
			
			// -- 1) lookup in dictionary of local server
			foreach (KeyValuePair<NetworkConnection, string> user in onlineUsers)
				if (user.Key != null && user.Value == userName)
					online = true;
			
			// -- 2) lookup in database if not online locally
			if (!online)
				online = DatabaseManager.singleton.GetUserOnline(userName);
			
			return online;
		}
		
		// -------------------------------------------------------------------------------
		// GetIsPlayerLoggedIn
		// -------------------------------------------------------------------------------
		public bool GetIsPlayerLoggedIn(string playerName)
		{
			
			bool online = false;
			
			// -- 1) lookup in dictionary of local server
			foreach (KeyValuePair<string, GameObject> player in onlinePlayers)
				if (player.Value != null && player.Value.name == playerName)
					online = true;
			
			// -- 2) lookup in database if not online locally
			if (!online)
				online = DatabaseManager.singleton.GetPlayerOnline(playerName);
				
			return online;
		}
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>GetUserName</c> returns a String.
        /// Finds a players username based on their <c>NetworkConnection</c>.
        /// </summary>
        /// <param name="conn"></param>
        /// <returns> Returns the user's username </returns>
		public string GetUserName(NetworkConnection conn)
		{
			foreach (KeyValuePair<NetworkConnection, string> user in onlineUsers)
			{
			debug.Log("----------GetUserName:"+user.Value);
			
				if (user.Key == conn) return user.Value;
			}
			
			return "";
		}
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public method <c>ServerSendError</c>.
        /// Sends an error using the user's <c>NetworkConnection</c>.
        /// Can send the user a error sting and disconnect them. 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="error"></param>
        /// <param name="disconnect"></param>
		public void ServerSendError(NetworkConnection conn, string error, bool disconnect)
		{
			conn.Send(new ErrorMsg{text=error, causesDisconnect=disconnect});
		}
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnClientError</c>.
        /// Triggers when the client receives an error response from the server.
        /// Reacts to the error message.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="message"></param>
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
        /// <summary>
        /// Public method <c>OnStopServer</c>.
        /// Closes the database connection.
        /// </summary>
		public override void OnStopServer()
		{
			
			// -- 1) saves all online players data
			DatabaseManager.singleton.SavePlayers();
			
			// -- 2) force log-out out all online players
			if (onlinePlayers != null)
				foreach (KeyValuePair<string, GameObject> player in onlinePlayers.ToList())
					if (player.Value != null)
						OnServerDisconnect(player.Value.GetComponent<PlayerComponent>().connectionToClient);
					
			// -- 3) closes the database connection
			DatabaseManager.singleton.Destruct();
		}
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>IsConnecting</c> returns a boolean.
        /// Checks whether the client is active and the client scene is ready
        /// </summary>
        /// <returns> Returns a boolean value detailing whether the user is connecting </returns>
		public bool IsConnecting() => NetworkClient.active && !ClientScene.ready;
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public event <c>OnClientConnect</c>.
        /// Triggered when the client connects.
        /// </summary>
        /// <param name="conn"></param>
		public override void OnClientConnect(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnClientConnect), conn); //HOOK
		}
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public event <c>OnServerConnect</c>.
        /// Triggered when the server connects.
        /// </summary>
        /// <param name="conn"></param>
		public override void OnServerConnect(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnServerConnect), conn); //HOOK
		}
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public event <c>OnClientSceneChanged</c>.
        /// Triggered when the client's scene changed.
        /// </summary>
        /// <param name="conn"></param>
		public override void OnClientSceneChanged(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnClientSceneChanged), conn); //HOOK
		}

		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public event <c>OnServerAddPlayer</c>.
        /// Triggered when the server adds a player.
        /// </summary>
        /// <param name="conn"></param>
		public override void OnServerAddPlayer(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnServerAddPlayer), conn); //HOOK
		}
	
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Publice event <c>OnServerDisconnect</c>.
        /// Triggered on the server when a client disconnects.
        /// </summary>
        /// <param name="conn"></param>
		public override void OnServerDisconnect(NetworkConnection conn)
		{

			// -- 1) log out the local user (if any)
			string username = GetUserName(conn);
			if (!String.IsNullOrWhiteSpace(username) && (GetIsUserLoggedIn(username) || conn.identity != null))
				DatabaseManager.singleton.LogoutUser(username);
			
			// -- 2) log out the player (if any)
			if (conn.identity != null && conn.identity.gameObject != null)
			{
				debug.Log("[NetworkManager] Logged out player: " + conn.identity.gameObject.name);
				
				this.InvokeInstanceDevExtMethods(nameof(OnServerDisconnect), conn); //HOOK
				eventListeners.OnLogoutPlayer.Invoke(conn);
				
				string name = conn.identity.gameObject.name;
				onlinePlayers.Remove(name);
			}
			
			onlineUsers.Remove(conn);
			base.OnServerDisconnect(conn);
			
		}
		
		// -------------------------------------------------------------------------------
		// OnClientDisconnect
		// @Client
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public event <c>OnClientDisconnect</c>.
        /// Triggered when the client disconnects.
        /// Runs on the client.
        /// </summary>
        /// <param name="conn"></param>
		public override void OnClientDisconnect(NetworkConnection conn)
		{
			// -- required: otherwise a zone switch would disconnect the client
			if (GetComponent<PortalManager>() != null && !GetComponent<PortalManager>().GetAutoConnect)
			{
				base.OnClientDisconnect(conn);
				state = NetworkState.Offline;
				UIPopupConfirm.singleton.Init(systemText.clientDisconnected, Quit);
			}
			this.InvokeInstanceDevExtMethods(nameof(OnClientDisconnect)); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// Quit
		// universal quit function
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public method <c>Quit</c>.
        /// Universal Quit function.
        /// </summary>
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