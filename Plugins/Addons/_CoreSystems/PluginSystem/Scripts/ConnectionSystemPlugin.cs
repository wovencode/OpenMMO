//BY DX4D

using System;
using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
    [CreateAssetMenu(menuName = "OpenMMO/Plugins/Connection System Plugin", order = 0)]
    public class ConnectionSystemPlugin : ScriptableNetworkPlugin
    {
        [Header("PLUGIN INFO")]
        [SerializeField] string _pluginName = "CONNECTION SYSTEM";
        public override string PLUGIN_NAME => _pluginName;

        #region C O M P O N E N T  L I N K S
        //NETWORK AUTHENTICATOR COMPONENT
        NetworkAuthenticator _authenticator;
        internal NetworkAuthenticator authenticator
        {
            get
            {
                if (!_authenticator) _authenticator = FindObjectOfType<NetworkAuthenticator>();
                return _authenticator;
            }
        }
        #endregion //COMPONENT LINKS

        #region M E S S A G E  H A N D L E R S
        //CLIENT HANDLERS
        [Client]
        internal override void HandleServerMessageOnClient<T>(T msg)
        {
            authenticator.OnServerMessageResponseAuth(msg);
        }
        [Server]
        internal override void HandleClientMessageOnServer<T>(NetworkConnection conn, T msg)
        {
            authenticator.OnClientMessageRequestAuth(conn, (ClientConnectRequest)msg);
        }
        #endregion

        #region M E S S A G E  H A N D L E R  R E G I S T R Y
        //REGISTER CLIENT HANDLERS
        [Client]
        internal override void RegisterClientMessageHandlers()
        {
            //#if _CLIENT
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "CONNECT");
            NetworkClient.RegisterHandler<ServerResponseAuth>(HandleServerMessageOnClient, false);
            //Debug.Log("[CLIENT] - [REGISTER MESSAGE HANDLERS] - [CONNECTION SYSTEM] - [CONNECT] - "
            //    + "Registering Message Handlers to Client...");
            //#endif //_CLIENT
        }
        //REGISTER SERVER HANDLERS
        [Server]
        internal override void RegisterServerMessageHandlers()
        {
            //#if _SERVER
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "CONNECT");
            NetworkServer.RegisterHandler<ClientRequestAuth>(HandleClientMessageOnServer, false);
            //Debug.Log("[SERVER] - [REGISTER MESSAGE HANDLERS] - [CONNECTION SYSTEM] - [CONNECT] - "
            //    + "Registering Message Handlers to Server...");
            //#endif //_SERVER
        }
        #endregion //MESSAGE HANDLER REGISTRY
    }


#region N E T W O R K  M E S S A G E S
    // ------------------------------

    //SERVER RESPONSE MESSAGES - HANDLED ON CLIENT
    // @Server -> @Client
    /// <summary><para>
    /// Public Partial class <c>ServerMessageResponseAuth</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </para></summary>
    public partial struct ServerResponseAuth : ServerResponse
    {
        public NetworkAction action => NetworkAction.Authenticate;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
    //CLIENT REQUEST MESSAGES - HANDLED ON SERVER
    // @Client -> @Server
    /// <summary><para>
    /// Public partial struct <c>ClientRequestAuth</c> inherits from <c>ClientRequest</c>. 
    /// Is used to authorize the client.
    /// Contains authorization request sent from client to server.
    /// </para></summary>
    public partial struct ClientRequestAuth : ClientConnectRequest
    {
        public NetworkAction action => NetworkAction.Authenticate;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _clientVersion;
        public string clientVersion { get { return _clientVersion; } set { _clientVersion = value; } }
    }
#endregion //NETWORK MESSAGES

#region  N E T W O R K  M E S S A G E  H A N D L E R S
    // ---------------------------------------------

    //NETWORK AUTHENTICATOR
    public partial class NetworkAuthenticator
    {
        //@CLIENT SIDED RESPONSE TO MESSAGE FROM @SERVER
        // Direction: @Server -> @Client
        // Execution: @Client
        /// <summary>
        /// Event <c>OnServerMessageResponseAuth</c>.
        /// Is triggered when the server returns a authentication response message.
        /// Either authenicates the client, disconnects the client and returns an error message if there is one. 
        /// If the authentication was succesful the client is readied.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        //void OnServerMessageResponseAuth(NetworkConnection conn, ServerMessageResponseAuth msg) //REMOVED - MIRROR UPDATE - DX4D
        internal void OnServerMessageResponseAuth(ServerResponse msg) //ADDED - MIRROR UPDATE - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - MIRROR UPDATE - DX4D

            // -- show popup if error message is not empty
            if (!String.IsNullOrWhiteSpace(msg.text)) UI.UIPopupConfirm.singleton.Init(msg.text);
            if (msg.causesDisconnect) DisconnectClient(conn); //FORCED DISCONNECT
            if (msg.success) ConnectClient(conn); //CONNECT SUCCESS
            //else DisconnectClient(conn); //CONNECT FAILURE
        }

        //@SERVER SIDED RESPONSE TO MESSAGE FROM @CLIENT
        // Direction: @Client -> @Server
        // Execution: @Server
        /// <summary>
        /// Event <c>OnClientMessageRequestAuth</c>.
        /// Triggered when the server received an authentiation request from a client.
        /// The event attempts to authenticate the client.
        /// Occurs on the server.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        internal void OnClientMessageRequestAuth(NetworkConnection conn, ClientConnectRequest msg)
            //where T: struct, ClientConnectRequest
        {

            ServerResponseAuth message = new ServerResponseAuth
            {
                success = true,
                text = "",
                causesDisconnect = false
            };


            //TODO: BEGIN - The following creates a dependancy with Network Zones - This needs to be decoupled

            // -- Check for Network Portals
            // This prevents players from logging into a Network Zone. Directly logging
            // into a zone should not be possible and can only be done by warping to
            // that zone instead.
            //if (GetComponent<ZoneManager>() != null && !GetComponent<ZoneManager>().GetIsMainZone) //REMOVED - DX4D
            //    portalChecked = false; //REMOVED - DX4D
            //bool portalChecked = true;
            Zones.ZoneManager zone = GetComponent<Zones.ZoneManager>(); //ADDED - DX4D
            //if (zone != null && !zone.GetIsMainZone) portalChecked = false; //ADDED - DX4D

            if (zone != null && !zone.GetIsMainZone)
            {
                message.text = systemText.illegalZone;
                message.success = false;
            }

            //TODO: END

            else if ((checkApplicationVersion && msg.clientVersion != Application.version))
            {
                message.text = systemText.versionMismatch;
                message.success = false;
            }
            else
            {
                message.success = true; //ADDED - DX4D
                base.OnServerAuthenticated.Invoke(conn); //EVENT

                Debug.Log("[CONNECTION SERVER] - [CONNECT] - "
                    + "Authentication succeded for connection-" + conn.connectionId + " @" + conn.address);
                debug.LogFormat(this.name, nameof(OnClientMessageRequestAuth), conn.Id(), "Authenticated"); //DEBUG
            }

            conn.Send(message);

            if (!message.success)
            {
                Debug.Log("<<<ISSUE>>> [CONNECTION SERVER] - [CONNECT] - "
                    + "Authentication failed for connection-" + conn.connectionId + " @" + conn.address);

                conn.isAuthenticated = false;
                conn.Disconnect();

                debug.LogFormat(this.name, nameof(OnClientMessageRequestAuth), conn.Id(), "DENIED"); //DEBUG
            }
        }
    }
#endregion //NETWORK MESSAGE HANDLERS

#region H E L P E R  M E T H O D S
    // --------------------------

    //NETWORK AUTHENTICATOR
    public partial class NetworkAuthenticator
    {
        //CONNECT CLIENT
        void ConnectClient(NetworkConnection conn)
        {
            CancelInvoke();
            base.OnClientAuthenticated.Invoke(conn);

            UI.UIWindowAuth.singleton.Hide();
            UI.UIWindowMainLoginMenu.singleton.Show();

            LogConnectionSuccess(conn); //LOG SUCCESS
        }
        //LOG CONNECT SUCCESS
        void LogConnectionSuccess(NetworkConnection conn)
        {
            Debug.Log("<b>[<color=green>CONNECTION CLIENT</color>]</b> - "
                + "<b>Connected successfully to Server!</b>"
                + "\n" + "Connection-" + conn.connectionId + " @" + conn.address + " connecting to Server @" + NetworkClient.serverIp + "...");

            debug.LogFormat(this.name, nameof(OnServerMessageResponseAuth), NetworkClient.connection.Id(), "Authenticated"); //DEBUG
        }

        //DISCONNECT CLIENT
        void DisconnectClient(NetworkConnection conn)
        {
            conn.isAuthenticated = false;
            conn.Disconnect();
            NetworkManager.singleton.StopClient();

            LogConnectionFailure(conn); //LOG FAILURE
        }
        //LOG CONNECT FAILURE
        void LogConnectionFailure(NetworkConnection conn)
        {
#if DEBUG
            Debug.Log("<b><<<ISSUE>>> [<color=red>CONNECTION CLIENT</color>]</b> - "
                + "<b>Failed to connect to Server...</b>"
                + "\n" + "Connection-" + conn.connectionId + " @" + conn.address + " connecting to Server @" + NetworkClient.serverIp + "...");
#endif
            debug.LogFormat(this.name, nameof(OnServerMessageResponseAuth), NetworkClient.connection.Id(), "DENIED"); //DEBUG
        }
    }
#endregion //HELPER METHODS
}
