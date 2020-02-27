
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
        [DevExtMethods(nameof(OnStartServer))]
        void OnStartServer_NetworkPortals()
        {
            NetworkServer.RegisterHandler<ClientMessageRequestAutoAuth>(OnClientMessageRequestAutoAuth, false);
        }
        
        // ========================== MESSAGE HANDLERS - AUTH ============================

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestAutoAuth
        // Direction: @Server -> @Client
        // Execution: @Server
        // -------------------------------------------------------------------------------      
        void OnClientMessageRequestAutoAuth(NetworkConnection conn, ClientMessageRequestAutoAuth msg)
		{

			ServerMessageResponseAutoAuth message = new ServerMessageResponseAutoAuth
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
			
			debug.LogFormat(this.name, nameof(OnClientMessageRequestAutoAuth), conn.Id(), msg.success.ToString(), msg.text); //DEBUG
		
		}

        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================