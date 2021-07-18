//BY FHIZ

using OpenMMO.UI;
using System;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
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
        void OnServerResponseError(ServerResponseError msg) //REMOVED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerMessageResponseUserChangePassword), conn.Id(), msg.success.ToString()); //DEBUG
            OnServerResponse(msg);
        }
        
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

        /// <summary>
        /// Event <c>OnServerMessageResponseUserLogin</c>.
        /// Triggered when the client receives a login response from the server.
        /// Checks for the response succes and either shows the player select or auto selects the players.
        /// Occurs on the client.
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponseUserLogin(NetworkConnection conn, ServerResponseUserLogin msg) //REMOVED - DX4D
        void OnServerResponseUserLogin(ServerResponseUserLogin msg) //ADDED - DX4D
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

        /// <summary>
        /// Event <c>OnServerMessageResponseUserRegister</c>.
        /// Triggered when the client receives a register response from the server.
        /// Checks whether the register request was succesful. 
        /// Doesn't login the player. To log the player in another request has to be made.
        /// Occurs on the client.
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponseUserRegister(NetworkConnection conn, ServerResponseUserRegister msg) //REMOVED - DX4D
        void OnServerMessageResponseUserRegister(ServerResponseUserRegister msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            // -- hide user registration window if succeeded
            if (msg.success)
        	{
        		UIWindowRegisterUser.singleton.Hide();
        	}
        	
        	debug.LogFormat(this.name, nameof(OnServerMessageResponseUserRegister), conn.Id(), msg.success.ToString()); //DEBUG
        	
        	OnServerResponse(msg);
        }

        /// <summary>
        /// Event <c>OnServerMessageResponseUserDelete</c>.
        /// Triggered when the client receives a user deletion response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponseUserDelete(NetworkConnection conn, ServerResponseUserDelete msg) //REMOVED - DX4D
        void OnServerMessageResponseUserDelete(ServerResponseUserDelete msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerMessageResponseUserDelete), conn.Id(), msg.success.ToString()); //DEBUG
        	OnServerResponse(msg);
        }

        /// <summary>
        /// Event <c>OnServerMessageResponseUserChangePassword</c>.
        /// Triggered when the client receives a user changed password response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponseUserChangePassword(NetworkConnection conn, ServerResponseUserChangePassword msg) //REMOVED - DX4D
        void OnServerMessageResponseUserChangePassword(ServerResponseUserChangePassword msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerMessageResponseUserChangePassword), conn.Id(), msg.success.ToString()); //DEBUG
        	OnServerResponse(msg);
        }

        /// <summary>
        /// Event <c>OnServerMessageResponseUserConfirm</c>.
        /// Triggered when the client receives a user changed on user confirmation response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponseUserConfirm(NetworkConnection conn, ServerResponseUserConfirm msg) //REMOVED - DX4D
        void OnServerResponseUserConfirm(ServerResponseUserConfirm msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerResponseUserConfirm), conn.Id(), msg.success.ToString()); //DEBUG
        	OnServerResponse(msg);
        }

        /// <summary>
        /// Event <c>OnServerMessageResponseUserPlayerPreviews</c>.
        /// Triggered when the client receives a UserPlayerPreviews response from the server.
        /// Updates the clients Player Previews list.
        /// Occurs on the client.
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponseUserPlayerPreviews(NetworkConnection conn, ServerResponseUserPlayerPreviews msg) //REMOVED - DX4D
        void OnServerResponseUserPlayerPreviews(ServerResponseUserPlayerPreviews msg) //ADDED - DX4D
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

        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerLogin</c>.
        /// Triggered when the client receives a player login response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponsePlayerLogin(NetworkConnection conn, ServerResponsePlayerLogin msg) //REMOVED - DX4D
        void OnServerResponsePlayerLogin(ServerResponsePlayerLogin msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerResponsePlayerLogin), conn.Id(), msg.success.ToString()); //DEBUG
        	
        	OnServerResponse(msg);
        }

        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerRegister</c>.
        /// Triggered when the client receives a player register response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponsePlayerRegister(NetworkConnection conn, ServerResponsePlayerRegister msg) //REMOVED - DX4D
        void OnServerResponsePlayerRegister(ServerResponsePlayerRegister msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            if (msg.success)
        	{
        		playerPreviews.Add(new PlayerPreview{name=msg.playername});
        		UIWindowPlayerSelect.singleton.UpdatePlayerPreviews(true);
        	}
        	
        	debug.LogFormat(this.name, nameof(OnServerResponsePlayerRegister), conn.Id(), msg.success.ToString()); //DEBUG
        	
        	OnServerResponse(msg);
        }

        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerDelete</c>.
        /// Triggered when the client receives a player delete response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponsePlayerDelete(NetworkConnection conn, ServerResponsePlayerDelete msg) //REMOVED - DX4D
        void OnServerResponsePlayerDelete(ServerResponsePlayerDelete msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerResponsePlayerDelete), conn.Id(), msg.success.ToString()); //DEBUG

            OnServerResponse(msg);
        }
    }
}