
using Wovencode;
using Wovencode.Network;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace Wovencode.Network
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
        public override void OnStartServer()
        {
        
        	// ---- Auth
            NetworkServer.RegisterHandler<ClientMessageRequestAuth>(OnClientMessageRequestAuth, false);
           
        }
            	
        // -------------------------------------------------------------------------------
        // OnServerAuthenticate
        // @Server
		// -------------------------------------------------------------------------------
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
        void OnClientMessageRequest(NetworkConnection conn, ClientMessageRequest msg)
        {
    	
        }
        
        // ========================== MESSAGE HANDLERS - AUTH ============================
        
        // -------------------------------------------------------------------------------
        // OnAuthRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------      
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