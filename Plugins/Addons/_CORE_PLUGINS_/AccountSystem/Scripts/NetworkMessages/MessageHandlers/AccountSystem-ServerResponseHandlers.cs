//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Zones;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

using OpenMMO.UI;
using System;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
    public partial class NetworkManager
    {
        // ===============================================================================
        // ============================== ERROR HANDLERS =================================
        // ===============================================================================

        // -------------------------------------------------------------------------------
        // OnServerResponseError
        // Direction: @Server -> @Client
        // Execution: @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerResponseError</c>.
        /// Triggered when the server sends a response to the client.
        /// Occurs on the client.
        /// Checks for errors.
        /// </summary>
        /// <param name="msg"></param>
        internal void OnServerResponseError(Response.Error msg) //REMOVED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerMessageResponseUserChangePassword), conn.Id(), msg.success.ToString()); //DEBUG
            OnServerResponse(msg);
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
        /// <param name="msg"></param>
        //void OnServerMessageResponse(NetworkConnection conn, ServerResponse msg) //REMOVED - DX4D
        void OnServerResponse(ServerResponse msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

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
            
            debug.LogFormat(this.name, nameof(OnServerResponse), conn.Id(), msg.causesDisconnect.ToString(), msg.text); //DEBUG
        }

        // ========================== MESSAGE HANDLERS - USER ============================

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserLogin</c>.
        /// Triggered when the client receives a login response from the server.
        /// Checks for the response succes and either shows the player select or auto selects the players.
        /// Occurs on the client.
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponseUserLogin(NetworkConnection conn, ServerResponseUserLogin msg) //REMOVED - DX4D
        internal void OnServerResponseUserLogin(ServerLoginUserResponse msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D
            
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

                debug.LogFormat(this.name, nameof(OnServerResponseUserLogin), conn.Id(), msg.players.Length.ToString()); //DEBUG

            }
        	
        	OnServerResponse(msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserRegister</c>.
        /// Triggered when the client receives a register response from the server.
        /// Checks whether the register request was succesful. 
        /// Doesn't login the player. To log the player in another request has to be made.
        /// Occurs on the client.
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponseUserRegister(NetworkConnection conn, ServerResponseUserRegister msg) //REMOVED - DX4D
        internal void OnServerMessageResponseUserRegister(ServerResponse msg) //ADDED - DX4D
        {
            //NetworkConnection conn = NetworkClient.connection; //REMOVED ADDED - DX4D

            // -- hide user registration window if succeeded
            if (msg.success)
        	{
        		UIWindowRegisterUser.singleton.Hide();
        	}
        	
        	//debug.LogFormat(this.name, nameof(OnServerMessageResponseUserRegister), conn.Id(), msg.success.ToString()); //DEBUG //REMOVED DX4D
        	
        	OnServerResponse(msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserDelete</c>.
        /// Triggered when the client receives a user deletion response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponseUserDelete(NetworkConnection conn, ServerResponseUserDelete msg) //REMOVED - DX4D
        internal void OnServerMessageResponseUserDelete(ServerResponse msg) //ADDED - DX4D
        {
            //NetworkConnection conn = NetworkClient.connection; //REMOVED ADDED - DX4D

            //debug.LogFormat(this.name, nameof(OnServerMessageResponseUserDelete), conn.Id(), msg.success.ToString()); //DEBUG //REMOVED - DX4D
        	OnServerResponse(msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserChangePassword</c>.
        /// Triggered when the client receives a user changed password response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponseUserChangePassword(NetworkConnection conn, ServerResponseUserChangePassword msg) //REMOVED - DX4D
        internal void OnServerMessageResponseUserChangePassword(ServerResponse msg) //ADDED - DX4D
        {
            //NetworkConnection conn = NetworkClient.connection; //REMOVED ADDED - DX4D

            //debug.LogFormat(this.name, nameof(OnServerMessageResponseUserChangePassword), conn.Id(), msg.success.ToString()); //DEBUG //REMOVED DX4D
        	OnServerResponse(msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponseUserConfirm</c>.
        /// Triggered when the client receives a user changed on user confirmation response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponseUserConfirm(NetworkConnection conn, ServerResponseUserConfirm msg) //REMOVED - DX4D
        internal void OnServerResponseUserConfirm(ServerResponse msg) //ADDED - DX4D
        {
            //NetworkConnection conn = NetworkClient.connection; //REMOVED ADDED - DX4D

            //debug.LogFormat(this.name, nameof(OnServerResponseUserConfirm), conn.Id(), msg.success.ToString()); //DEBUG //REMOVED - DX4D
        	OnServerResponse(msg);
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
        /// <param name="msg"></param>
        //void OnServerResponseUserPlayerPreviews(NetworkConnection conn, ServerResponseUserPlayerPreviews msg) //REMOVED - DX4D
        internal void OnServerResponseUserPlayerPreviews(ServerPlayerPreviewsResponse msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            if (msg.success)
        	{
				playerPreviews.Clear();
				playerPreviews.AddRange(msg.players);
				maxPlayers	= msg.maxPlayers;
			}
			
			debug.LogFormat(this.name, nameof(OnServerResponseUserPlayerPreviews), conn.Id(), msg.players.Length.ToString()); //DEBUG
			
        	OnServerResponse(msg);
        }
    }
}
