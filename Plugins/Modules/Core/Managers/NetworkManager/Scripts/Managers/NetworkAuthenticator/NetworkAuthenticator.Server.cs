
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Zones;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkAuthenticator
	// ===================================================================================
    public partial class NetworkAuthenticator
    {
    	
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

            NetworkServer.RegisterHandler<ClientMessageRequestAuth>(OnClientMessageRequestAuth, false);
            
        	this.InvokeInstanceDevExtMethods(nameof(OnStartServer)); //HOOK
        }
            	
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
        void OnClientMessageRequest(NetworkConnection conn, ClientMessageRequest msg)
        {
    		// do nothing (this message is never called directly)
        }

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
        void OnClientMessageRequestAuth(NetworkConnection conn, ClientMessageRequestAuth msg)
		{

			ServerMessageResponseAuth message = new ServerMessageResponseAuth
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			// -- Check for Network Portals
			// This prevents players from logging into a Network Zone. Directly logging
			// into a zone should not be possible and can only be done by warping to
			// that zone instead.
			bool portalChecked = true;
			
			if (GetComponent<ZoneManager>() != null && !GetComponent<ZoneManager>().GetIsMainZone)
				portalChecked = false;
			
			if ((checkApplicationVersion && msg.clientVersion != Application.version) || !portalChecked)
			{
				message.text = systemText.versionMismatch;
            	message.success = false;
			}
			else
			{
				base.OnServerAuthenticated.Invoke(conn); //EVENT
				debug.LogFormat(this.name, nameof(OnClientMessageRequestAuth), conn.Id(), "Authenticated"); //DEBUG
			}
			
			conn.Send(message);
			
			if (!message.success)
			{
				conn.isAuthenticated = false;
				conn.Disconnect();
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestAuth), conn.Id(), "DENIED"); //DEBUG
			}
		
		}

        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================