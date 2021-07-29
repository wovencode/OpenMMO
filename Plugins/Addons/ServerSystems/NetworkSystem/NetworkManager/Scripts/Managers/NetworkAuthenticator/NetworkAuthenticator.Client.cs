//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.UI;
using OpenMMO.Zones;
using UnityEngine;
using System;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
    // NetworkAuthenticator
    // ===================================================================================
    public partial class NetworkAuthenticator
    {

        [Header("Client Settings")]
        public bool checkApplicationVersion = true;
        [Range(0, 99)]
        public int connectDelayMin = 4;
        [Range(0, 99)]
        public int connectDelayMax = 8;
        [Range(1, 999)]
        public int connectTimeout = 999;

        [HideInInspector] public int connectDelay;

        // -------------------------------------------------------------------------------
        // OnStartClient
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public override event <c>OnStartClient</c>.
        /// Event triggered on client start.
        /// This Event occurs on the client.
        /// The even registers the authentication event.
        /// </summary>
        public override void OnStartClient()
        {
            NetworkClient.RegisterHandler<ServerResponseAuth>(OnServerMessageResponseAuth, false);

            this.InvokeInstanceDevExtMethods(nameof(OnStartClient)); //HOOK
        }

        // -------------------------------------------------------------------------------
        // OnClientAuthenticate
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public override event <c>OnClientAuthenticate</c>.
        /// This event is triggered upon requestion authentication.
        /// Invokes a authentication request to trigger.
        /// </summary>
        /// <param name="conn"></param>
        public override void OnClientAuthenticate() //FIX - MIRROR UPDATE - NetworkConnection conn parameter Replaced with NetworkClient.connection - DX4D
        {
            ZoneManager zoneManager = GetComponent<ZoneManager>();
            if (!zoneManager) zoneManager = FindObjectOfType<ZoneManager>(); //ADDED DX4D

            if (zoneManager != null && !zoneManager.GetAutoConnect)
            {
                Invoke(nameof(ClientAuthenticate), connectDelay);
            }

            this.InvokeInstanceDevExtMethods(nameof(OnClientAuthenticate)); //HOOK //, conn); //FIX - MIRROR UPDATE - conn parameter is no longer passed through - it was replaced with NetworkClient.connection - DX4D
        }

        // -------------------------------------------------------------------------------
        // ClientAuthenticate
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public Method <c>ClientAuthenticate</c>
        /// Sends a authentication request message from the client to the server.
        /// </summary>
        public void ClientAuthenticate()
        {

            ClientRequestAuth msg = new ClientRequestAuth
            {
                clientVersion = Application.version
            };

#if DEBUG
            Debug.Log("<b>[<color=blue>CLIENT</color>]</b> - "
                + "<b>Attempting to Join Server...</b>"
                + "\n" + "Connection-" + NetworkClient.connection.connectionId + " @" + NetworkClient.connection.address + " connecting to Server @" + NetworkClient.serverIp + "...");
#endif
            NetworkClient.Send<ClientRequestAuth>(msg);

            debug.LogFormat(this.name, nameof(ClientAuthenticate)); //DEBUG
        }

        // ========================== MESSAGE HANDLERS - AUTH ============================

        //CLIENT SIDED RESPONSE TO MESSAGE FROM SERVER 
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseAuth</c>.
        /// Is triggered when the server returns a authentication response message.
        /// Either authenicates the client, disconnects the client and returns an error message if there is one. 
        /// If the authentication was succesful the client is readied.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        //void OnServerMessageResponseAuth(NetworkConnection conn, ServerMessageResponseAuth msg) //REMOVED - MIRROR UPDATE - DX4D
        void OnServerMessageResponseAuth(ServerResponseAuth msg) //ADDED - MIRROR UPDATE - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - MIRROR UPDATE - DX4D

            // -- show popup if error message is not empty
            if (!String.IsNullOrWhiteSpace(msg.text)) UIPopupConfirm.singleton.Init(msg.text);
            if (msg.causesDisconnect) DisconnectClient(conn); //FORCED DISCONNECT
            if (msg.success) ConnectClient(conn); //CONNECT SUCCESS
            //else DisconnectClient(conn); //CONNECT FAILURE
        }

        //CONNECT CLIENT
        void ConnectClient(NetworkConnection conn)
        {
            CancelInvoke();
            base.OnClientAuthenticated.Invoke(conn);

            UIWindowAuth.singleton.Hide();
            UIWindowMainLoginMenu.singleton.Show();

            LogConnectionSuccess(conn); //LOG SUCCESS
        }
        //LOG CONNECT SUCCESS
        void LogConnectionSuccess(NetworkConnection conn)
        {
            Debug.Log("<b>[<color=green>CLIENT</color>]</b> - "
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
            Debug.Log("<b>[<color=red>CLIENT</color>]</b> - "
                + "<b>Failed to connect to Server...</b>"
                + "\n" + "Connection-" + conn.connectionId + " @" + conn.address + " connecting to Server @" + NetworkClient.serverIp + "...");
#endif
            debug.LogFormat(this.name, nameof(OnServerMessageResponseAuth), NetworkClient.connection.Id(), "DENIED"); //DEBUG
        }
    }
}