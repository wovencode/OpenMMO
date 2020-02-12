
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
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
        /// <summary>
        /// Public override event <c>OnStartClient</c>.
        /// Event triggered on client start.
        /// This Event occurs on the client.
        /// The even registers the authentication event.
        /// </summary>
        public override void OnStartClient()
        {
            // ---- Auth
            NetworkClient.RegisterHandler<ServerMessageResponseAuth>(OnServerMessageResponseAuth, false);     
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
        public override void OnClientAuthenticate(NetworkConnection conn)
        {
        	Invoke(nameof(ClientAuthenticate), connectDelay);		
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

            ClientMessageRequestAuth authRequestMessage = new ClientMessageRequestAuth
            {
                clientVersion = Application.version
            };

            NetworkClient.Send(authRequestMessage);
            
		}

        // ========================== MESSAGE HANDLERS - AUTH ============================
        
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseAuth</c>.
        /// Is triggered when the server returns a authentication response message.
        /// Either authenicates the client, disconnects the client and returns an error message if there is one. 
        /// If the authentication was succesful the client is readied.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
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