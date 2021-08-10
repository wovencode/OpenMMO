//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Zones;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
    // NetworkAuthenticator
    // ===================================================================================
    public partial class NetworkAuthenticator
    {
        /* //DEPRECIATED - DX4D
        // -------------------------------------------------------------------------------
        // OnStartServer
        // @Server
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public ovverride event <c>OnStartServer</c>.
        /// Triggered on server start.
        /// Event occurs on the server.
        /// Sets up the user authentication event.
        /// </summary>
        public override void OnStartServer()
        {
            Debug.Log("[REGISTER NETWORK MESSAGES] - [CONNECTION SERVER] - [CONNECT] - "
                + "Registering Message Handlers to Server...");
            NetworkServer.RegisterHandler<ClientRequestAuth>(OnClientMessageRequestAuth, false);

            this.InvokeInstanceDevExtMethods(nameof(OnStartServer)); //HOOK
        }*/

        // -------------------------------------------------------------------------------
        // OnServerAuthenticate
        // @Server
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public override event <c>OnServerAuthenticate</c>.
        /// Does nothing. Waits for the <c>AuthRequestMessage</c> to be received from the client.
        /// Event occurs on the server. 
        /// </summary>
        /// <param name="conn"></param>
        public override void OnServerAuthenticate(NetworkConnection conn)
        {
            // do nothing...wait for AuthRequestMessage from client
        }

        // ===============================================================================
        // ============================= MESSAGE HANDLERS ================================
        // ===============================================================================

        // -------------------------------------------------------------------------------
        // OnClientMessageRequest
        // Direction: @Client -> @Server
        // Execution: @Server
        // --------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnClientMessageRequest</c>.
        /// Event called when a <c>ClientMessageRequest</c> is received on the server.
        /// Occurs on the server.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequest(NetworkConnection conn, ClientRequest msg)
        {
            // do nothing (this message is never called directly)
        }

        /* //DEPRECIATED - DX4D
        // ========================== MESSAGE HANDLERS - AUTH ============================

        // -------------------------------------------------------------------------------
        // OnAuthRequestMessage
        // Direction: @Client -> @Server
        // Execution: @Server
        // -------------------------------------------------------------------------------      
        /// <summary>
        /// Event <c>OnClientMessageRequestAuth</c>.
        /// Triggered when the server received an authentiation request from a client.
        /// The event attempts to authenticate the client.
        /// Occurs on the server.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequestAuth(NetworkConnection conn, ClientRequestAuth msg)
        {

            ServerResponseAuth message = new ServerResponseAuth
            {
                action = NetworkAction.Authenticate,
                success = true,
                text = "",
                causesDisconnect = false
            };

            // -- Check for Network Portals
            // This prevents players from logging into a Network Zone. Directly logging
            // into a zone should not be possible and can only be done by warping to
            // that zone instead.
            bool portalChecked = true;
            ZoneManager zone = GetComponent<ZoneManager>(); //ADDED - DX4D
            if (zone != null && !zone.GetIsMainZone) portalChecked = false; //ADDED - DX4D

            //if (GetComponent<ZoneManager>() != null && !GetComponent<ZoneManager>().GetIsMainZone) //REMOVED - DX4D
            //    portalChecked = false; //REMOVED - DX4D

            if ((checkApplicationVersion && msg.clientVersion != Application.version) || !portalChecked)
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
        }*/
    }
}