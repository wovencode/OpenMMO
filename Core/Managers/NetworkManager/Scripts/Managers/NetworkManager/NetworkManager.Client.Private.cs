
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using OpenMMO.Zones;
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
        	
            // ---- User Messages
            // @Server -> @Client
            NetworkClient.RegisterHandler<ServerMessageResponseUserLogin>(OnServerMessageResponseUserLogin);
            NetworkClient.RegisterHandler<ServerMessageResponseUserRegister>(OnServerMessageResponseUserRegister);
            NetworkClient.RegisterHandler<ServerMessageResponseUserDelete>(OnServerMessageResponseUserDelete);
            NetworkClient.RegisterHandler<ServerMessageResponseUserChangePassword>(OnServerMessageResponseUserChangePassword);
            NetworkClient.RegisterHandler<ServerMessageResponseUserConfirm>(OnServerMessageResponseUserConfirm);
            NetworkClient.RegisterHandler<ServerMessageResponseUserPlayerPreviews>(OnServerMessageResponseUserPlayerPreviews);
            
            // ---- Player Messages
            // @Server -> @Client
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerLogin>(OnServerMessageResponsePlayerLogin);
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerRegister>(OnServerMessageResponsePlayerRegister);
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerDelete>(OnServerMessageResponsePlayerDelete);
            
            // --- Error Message
            // @Server -> @Client
            NetworkClient.RegisterHandler<ServerMessageResponse>(OnServerMessageResponse);
            
            this.InvokeInstanceDevExtMethods(nameof(OnStartClient)); //HOOK
            eventListeners.OnStartClient.Invoke(); //EVENT
            
        }
        
        // ===============================================================================
        // ============================= MESSAGE HANDLERS ================================
        // ===============================================================================
        
        // -------------------------------------------------------------------------------
		// OnServerMessageResponse
		// Direction: @Server -> @Client
		// Execution: @Client
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
            
            debug.LogFormat(this.name, nameof(OnServerMessageResponse), conn.Id(), msg.causesDisconnect.ToString(), msg.text); //DEBUG
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
                maxPlayers = msg.maxPlayers;

                // -- Show Player Select if there are players
                // -- Show Player Creation if there are no players
                if (msg.players.Length > 0)
                    UIWindowPlayerSelect.singleton.Show();
                else
                    UIWindowPlayerCreate.singleton.Show();

                UIWindowLoginUser.singleton.Hide();

                debug.LogFormat(this.name, nameof(OnServerMessageResponseUserLogin), conn.Id(), msg.players.Length.ToString()); //DEBUG

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
        	}
        	
        	debug.LogFormat(this.name, nameof(OnServerMessageResponseUserRegister), conn.Id(), msg.success.ToString()); //DEBUG
        	
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
        	debug.LogFormat(this.name, nameof(OnServerMessageResponseUserDelete), conn.Id(), msg.success.ToString()); //DEBUG
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
        	debug.LogFormat(this.name, nameof(OnServerMessageResponseUserChangePassword), conn.Id(), msg.success.ToString()); //DEBUG
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
        	debug.LogFormat(this.name, nameof(OnServerMessageResponseUserConfirm), conn.Id(), msg.success.ToString()); //DEBUG
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
			
			debug.LogFormat(this.name, nameof(OnServerMessageResponseUserPlayerPreviews), conn.Id(), msg.players.Length.ToString()); //DEBUG
			
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
        	
        	debug.LogFormat(this.name, nameof(OnServerMessageResponsePlayerLogin), conn.Id(), msg.success.ToString()); //DEBUG
        	
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
        	if (msg.success)
        	{
        		playerPreviews.Add(new PlayerPreview{name=msg.playername});
        		UIWindowPlayerSelect.singleton.UpdatePlayerPreviews(true);
        	}
        	
        	debug.LogFormat(this.name, nameof(OnServerMessageResponsePlayerRegister), conn.Id(), msg.success.ToString()); //DEBUG
        	
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
        	
        	debug.LogFormat(this.name, nameof(OnServerMessageResponsePlayerDelete), conn.Id(), msg.success.ToString()); //DEBUG
        	
        	OnServerMessageResponse(conn, msg);
        }

        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================