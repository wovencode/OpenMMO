
using Wovencode;
using Wovencode.Network;
using Wovencode.UI;
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
    	
    	[Header("Client Settings")]
    	public bool checkApplicationVersion = true;
    	[Range(0,99)]
    	public int connectDelayMin = 4;
    	[Range(0,99)]
    	public int connectDelayMax = 8;
    	[Range(1,999)]
    	public int connectTimeout = 60;
    	
    	[HideInInspector] public int connectDelay;
    	
        // -------------------------------------------------------------------------------
        // OnStartClient
        // @Client
		// -------------------------------------------------------------------------------
        public override void OnStartClient()
        {
            // ---- Auth
            NetworkClient.RegisterHandler<ServerMessageResponseAuth>(OnServerMessageResponseAuth, false);      
        }
        
        // -------------------------------------------------------------------------------
        // OnClientAuthenticate
        // @Client
		// -------------------------------------------------------------------------------
        public override void OnClientAuthenticate(NetworkConnection conn)
        {
        	Invoke(nameof(ClientAuthenticate), connectDelay);		
        }
        
		// -------------------------------------------------------------------------------
        // ClientAuthenticate
        // @Client -> @Server
		// -------------------------------------------------------------------------------
		public void ClientAuthenticate()
		{

            ClientMessageRequestAuth authRequestMessage = new ClientMessageRequestAuth
            {
                clientVersion = Application.version
            };

            NetworkClient.Send(authRequestMessage);
            
		}

        // ========================== MESSAGE HANDLERS - AUTH ============================
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponseAuth(NetworkConnection conn, ServerMessageResponseAuth msg)
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
               	UIWindowAuth.singleton.Hide();
               	UIWindowMain.singleton.Show();
            }
        	
        }
        
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================