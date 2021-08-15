//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Network;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

using Mirror;
using UnityEngine;

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
            Debug.Log("[REGISTER NETWORK MESSAGES] - [CONNECTION SERVER] - [AUTOCONNECT] - "
                + "Registering Message Handlers to Server...");
            NetworkServer.RegisterHandler<ClientRequestAutoAuth>(OnClientMessageRequestAutoAuth, false);
        }
        
        // ========================== MESSAGE HANDLERS - AUTH ============================

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestAutoAuth
        // Direction: @Server -> @Client
        // Execution: @Server
        // -------------------------------------------------------------------------------      
        void OnClientMessageRequestAutoAuth(NetworkConnection conn, ClientRequestAutoAuth msg)
		{

			ServerResponseAutoAuth message = new ServerResponseAutoAuth
            {
                success             = true,
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
                message.success = true; //ADDED - DX4D
				base.OnServerAuthenticated.Invoke(conn);

                Debug.Log("[CONNECTION SERVER] - [AUTOCONNECT] - "
                    + "Authentication succeded for connection-" + conn.connectionId + " @" + conn.address);
            }
			
			conn.Send(message);
			
			if (!message.success)
            {
                Debug.Log(">>>ISSUE<<< [CONNECTION SERVER] - [AUTOCONNECT] - "
                    + "Authentication failed for connection-" + conn.connectionId + " @" + conn.address);

                conn.isAuthenticated = false;
				conn.Disconnect();
			}
			
			debug.LogFormat(this.name, nameof(OnClientMessageRequestAutoAuth), conn.Id(), msg.success.ToString(), msg.text); //DEBUG
		
		}

        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================