
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
	// NetworkManager
	// ===================================================================================
    public partial class NetworkManager
    {
    
        // -------------------------------------------------------------------------------
        // OnStartClient
        // @Client
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Pubilc override event <c>OnStartClient</c>.
        /// Triggered when the client starts.
        /// Occurs on client.
        /// Registers all the user request and server response handlers.
        /// </summary>
        public override void OnStartClient()
        {
        	
            // ---- User
            NetworkClient.RegisterHandler<ServerMessageResponseUserLogin>(OnServerMessageResponseUserLogin);
            NetworkClient.RegisterHandler<ServerMessageResponseUserRegister>(OnServerMessageResponseUserRegister);
            NetworkClient.RegisterHandler<ServerMessageResponseUserDelete>(OnServerMessageResponseUserDelete);
            NetworkClient.RegisterHandler<ServerMessageResponseUserChangePassword>(OnServerMessageResponseUserChangePassword);
            NetworkClient.RegisterHandler<ServerMessageResponseUserConfirm>(OnServerMessageResponseUserConfirm);
            NetworkClient.RegisterHandler<ServerMessageResponseUserPlayerPreviews>(OnServerMessageResponseUserPlayerPreviews);
            
            // ---- Player
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerLogin>(OnServerMessageResponsePlayerLogin);
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerRegister>(OnServerMessageResponsePlayerRegister);
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerDelete>(OnServerMessageResponsePlayerDelete);
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerSwitchServer>(OnServerMessageResponsePlayerSwitchServer);
            
            this.InvokeInstanceDevExtMethods(nameof(OnStartClient));
            eventListeners.OnStartClient.Invoke();
            
        }
        
        // ===============================================================================
        // ============================= MESSAGE HANDLERS ================================
        // ===============================================================================
        
        // -------------------------------------------------------------------------------
		// OnServerMessageResponse
		// @Server -> @Client
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponse</c>.
        /// Triggered when the server sends a response to the client.
        /// Occurs on the client.
        /// Checks for errors.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponse(NetworkConnection conn, ServerMessageResponse msg)
        {
    		
    		// -- show popup if error message is not empty
        	if (!String.IsNullOrWhiteSpace(msg.text))
               	UIPopupConfirm.singleton.Init(msg.text);
    		
        	// -- disconnect and un-authenticate if anything went wrong
            if (msg.causesDisconnect)
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                NetworkManager.singleton.StopClient();
            }
            
        }

        // ========================== MESSAGE HANDLERS - USER ============================

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserLogin</c>.
        /// Triggered when the client receives a login response from the server.
        /// Checks for the response succes and either shows the player select or auto selects the players.
        /// Occurs on the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponseUserLogin(NetworkConnection conn, ServerMessageResponseUserLogin msg)
        {
        	
        	if (msg.success)
        	{
				playerPreviews.Clear();
				playerPreviews.AddRange(msg.players);
				maxPlayers	= msg.maxPlayers;
				
				// -- Check Auto select Player
				int playerIndex = playerPreviews.FindIndex(x => x.name == PortalManager.autoSelectPlayer);
				
				if (playerIndex != -1)
				{
					// -- Skip Player Select
					ClientScene.AddPlayer(NetworkClient.connection);
					PortalManager.autoSelectPlayer = "";
				}
				else
				{
					// -- Show Player Select
					UIWindowLoginUser.singleton.Hide();
					UIWindowPlayerSelect.singleton.Show();
        		}
        		
        	}
        	
        	OnServerMessageResponse(conn, msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserRegister</c>.
        /// Triggered when the client receives a register response from the server.
        /// Checks whether the register request was succesful. 
        /// Doesn't login the player. To log the player in another request has to be made.
        /// Occurs on the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponseUserRegister(NetworkConnection conn, ServerMessageResponseUserRegister msg)
        {
        	
        	// -- hide user registration window if succeeded
        	if (msg.success)
        	{
        		UIWindowRegisterUser.singleton.Hide();
        		UIWindowMain.singleton.Show();
        	}
        		
        	OnServerMessageResponse(conn, msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserDelete</c>.
        /// Triggered when the client receives a user deletion response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponseUserDelete(NetworkConnection conn, ServerMessageResponseUserDelete msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserChangePassword</c>.
        /// Triggered when the client receives a user changed password response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponseUserChangePassword(NetworkConnection conn, ServerMessageResponseUserChangePassword msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserConfirm</c>.
        /// Triggered when the client receives a user changed on user confirmation response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponseUserConfirm(NetworkConnection conn, ServerMessageResponseUserConfirm msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }

        // -------------------------------------------------------------------------------
        // OnServerMessageResponseUserPlayerPreviews
        // updates the clients player previews list
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserPlayerPreviews</c>.
        /// Triggered when the client receives a UserPlayerPreviews response from the server.
        /// Updates the clients Player Previews list.
        /// Occurs on the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponseUserPlayerPreviews(NetworkConnection conn, ServerMessageResponseUserPlayerPreviews msg)
        {
        
        	if (msg.success)
        	{
				playerPreviews.Clear();
				playerPreviews.AddRange(msg.players);
				maxPlayers	= msg.maxPlayers;
			}
			
        	OnServerMessageResponse(conn, msg);
        }

        // ======================== MESSAGE HANDLERS - PLAYER ============================

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerLogin</c>.
        /// Triggered when the client receives a player login response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponsePlayerLogin(NetworkConnection conn, ServerMessageResponsePlayerLogin msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerRegister</c>.
        /// Triggered when the client receives a player register response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponsePlayerRegister(NetworkConnection conn, ServerMessageResponsePlayerRegister msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerDelete</c>.
        /// Triggered when the client receives a player delete response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponsePlayerDelete(NetworkConnection conn, ServerMessageResponsePlayerDelete msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// **WORK IN PROGRESS**
        /// Event <c>OnServerMessageResponsePlayerSwitchServer</c>.
        /// Triggered when the client receives a player switch server response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnServerMessageResponsePlayerSwitchServer(NetworkConnection conn, ServerMessageResponsePlayerSwitchServer msg)
        {
        	
        	//RequestPlayerSwitchServer(conn, userName);
        	OnServerMessageResponse(conn, msg);
        }      
        
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================