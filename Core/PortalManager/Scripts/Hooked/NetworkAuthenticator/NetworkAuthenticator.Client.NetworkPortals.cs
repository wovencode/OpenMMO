
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using OpenMMO.Portals;
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
        // OnStartClient
        // @Client
		// -------------------------------------------------------------------------------
       	[DevExtMethods(nameof(OnStartClient))]
        void OnStartClient_NetworkPortals()
        {
            NetworkClient.RegisterHandler<ServerMessageResponseAutoAuth>(OnServerMessageResponseAutoAuth, false);  
            
        }
        
        // -------------------------------------------------------------------------------
        // OnClientAuthenticate_NetworkPortals
        // @Client
        // -------------------------------------------------------------------------------
       	[DevExtMethods(nameof(OnClientAuthenticate))]
        void OnClientAuthenticate_NetworkPortals(NetworkConnection conn)
        {
        	if (GetComponent<PortalManager>() != null && GetComponent<PortalManager>().GetAutoConnect)
        		Invoke(nameof(ClientAutoAuthenticate), connectDelay);		 
        }
    	
		// -------------------------------------------------------------------------------
        // ClientAutoAuthenticate
        // Direction: @Client -> @Server
        // Execution: @Client
		// -------------------------------------------------------------------------------
		public void ClientAutoAuthenticate()
		{

            ClientMessageRequestAutoAuth msg = new ClientMessageRequestAutoAuth
            {
                clientVersion = Application.version
            };

            NetworkClient.Send(msg);
            
		}

        // ========================== MESSAGE HANDLERS - AUTH ============================
        
        // -------------------------------------------------------------------------------
       	// OnServerMessageResponseAutoAuth
		// Direction: @Server -> @Client
		// Execution: @Client
       	// -------------------------------------------------------------------------------
        void OnServerMessageResponseAutoAuth(NetworkConnection conn, ServerMessageResponseAutoAuth msg)
        {
        	
        	// -- show popup if error message is not empty
        	if (!String.IsNullOrWhiteSpace(msg.text))
               	UIPopupConfirm.singleton.Init(msg.text);
    		
        	// -- disconnect and un-authenticate if anything went wrong
            if (!msg.success || msg.causesDisconnect)
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                NetworkManager.singleton.StopClient();
            }
            
            // -- ready client
            if (msg.success && !msg.causesDisconnect)
            {
            	CancelInvoke();
               	base.OnClientAuthenticated.Invoke(conn);
               	PortalManager.singleton.AutoLogin();
            }
            
            debug.LogFormat(this.name, nameof(OnServerMessageResponseAutoAuth), msg.success.ToString(), msg.text); //DEBUG
        	
        }
        
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================