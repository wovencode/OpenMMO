
using OpenMMO;
using OpenMMO.Network;
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
        
        	// ---- Auth
            NetworkServer.RegisterHandler<ClientMessageRequestAuth>(OnClientMessageRequestAuth, false);
           
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
        // @Client -> @Server
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
    	
        }

        // ========================== MESSAGE HANDLERS - AUTH ============================

        // -------------------------------------------------------------------------------
        // OnAuthRequestMessage
        // @Client -> @Server
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
			
			if (checkApplicationVersion && msg.clientVersion != Application.version)
			{
				message.text = systemText.versionMismatch;
            	message.success = false;
			}
			else
			{
				base.OnServerAuthenticated.Invoke(conn);
			}
			
			conn.Send(message);
			
			if (!message.success)
			{
				conn.isAuthenticated = false;
				conn.Disconnect();
			}
		
		}

        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================