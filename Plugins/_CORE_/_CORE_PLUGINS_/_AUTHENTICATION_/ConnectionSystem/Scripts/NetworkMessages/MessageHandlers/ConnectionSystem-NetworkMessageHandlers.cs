//BY DX4D

using System;
using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
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

            Response.AuthResponse message = new Response.AuthResponse
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
                Debug.Log(">>>ISSUE<<< [CONNECTION SERVER] - [CONNECT] - "
                    + "Authentication failed for connection-" + conn.connectionId + " @" + conn.address);

                conn.isAuthenticated = false;
                conn.Disconnect();

                debug.LogFormat(this.name, nameof(OnClientMessageRequestAuth), conn.Id(), "DENIED"); //DEBUG
            }
        }
    }
#endregion //NETWORK MESSAGE HANDLERS
}
