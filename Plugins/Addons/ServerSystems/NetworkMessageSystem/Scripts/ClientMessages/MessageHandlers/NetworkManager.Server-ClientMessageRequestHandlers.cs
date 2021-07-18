//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Database;
using UnityEngine;
using System;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client -> @Server
        /// <summary>
        /// Event <c>OnClientRequestUserLogin</c>.
        /// Triggered when the server receives a user login request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientRequestUserLogin(NetworkConnection conn, ClientRequestUserLogin msg)
		{
			ServerResponseUserLogin message = new ServerResponseUserLogin
            {
                action              = NetworkAction.UserLogin, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserLogin(msg.username, msg.password))
			{
				LoginUser(conn, msg.username);
				
				// TODO: Add increased maxPlayers from user data later
				message.maxPlayers = GameRulesTemplate.singleton.maxPlayersPerUser;
				message.LoadPlayerPreviews(DatabaseManager.singleton.GetPlayers(msg.username));
				message.text = systemText.userLoginSuccess;
			}
			else
			{
				message.text = systemText.userLoginFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserLogin), conn.Id(), "DENIED"); //DEBUG
				
			}
			
			conn.Send(message);
		}
		
        // @Client -> @Server
        void OnClientRequestUserLogout(NetworkConnection conn, ClientRequestUserLogout msg)
		{
			LogoutUser(conn);
		}
		
        // @Client -> @Server
        /// <summary>
        /// Event <c>OnClientMessageRequestUserRegister</c>.
        /// Triggered when the server receives a user registration request from the client.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestUserRegister(NetworkConnection conn, ClientRequestUserRegister msg)
        {
        	ServerResponseUserRegister message = new ServerResponseUserRegister
            {
                action              = NetworkAction.UserRegister, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
        	if (DatabaseManager.singleton.TryUserRegister(msg.username, msg.password, msg.email, msg.deviceid))
			{
				RegisterUser(msg.username);
				message.text = systemText.userRegisterSuccess;
			}
			else
			{
				message.text = systemText.userRegisterFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserRegister), conn.Id(), "DENIED"); //DEBUG
				
			}

        	conn.Send(message);
        }

        // @Client -> @Server
        /// <summary>
        /// Event <c>OnClientRequestUserDelete</c>.
        /// Triggered when the server receives a user deletion request.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestUserDelete(NetworkConnection conn, ClientRequestUserDelete msg)
        {
        	
        	ServerResponseUserDelete message = new ServerResponseUserDelete
            {
                action              = NetworkAction.UserDelete, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
        	if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserDelete(msg.username, msg.password))
			{
				message.text = systemText.userDeleteSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserDelete), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.userDeleteFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserDelete), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        	
        }

        // @Client -> @Server
        /// <summary>
        /// Event <c>OnClientRequestUserChangePassword</c>.
        /// Triggered when the server receives a user change password request.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestUserChangePassword(NetworkConnection conn, ClientRequestUserChangePassword msg)
        {
        	
        	ServerResponseUserChangePassword message = new ServerResponseUserChangePassword
            {
                action              = NetworkAction.UserChangePassword, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
        	if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserChangePassword(msg.username, msg.oldPassword, msg.newPassword))
			{
				message.text = systemText.userChangePasswordSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserChangePassword), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.userChangePasswordFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserChangePassword), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        }

        // @Client -> @Server
        /// <summary>
        /// Event <c>OnClientRequestUserConfirm</c>.
        /// Triggered by the server receiving a user confirmation request from the client.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestUserConfirm(NetworkConnection conn, ClientRequestUserConfirm msg)
        {
        	
        	ServerResponseUserConfirm message = new ServerResponseUserConfirm
            {
                action              = NetworkAction.UserConfirm, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserConfirm(msg.username, msg.password))
			{
				message.text = systemText.userConfirmSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserConfirm), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.userConfirmFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserConfirm), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        }
        

		// @Server
		protected void RegisterUser(string username)
		{
			// isNew = true
			// Transaction = false
			DatabaseManager.singleton.SaveDataUser(username, true, false);
            Debug.Log("<color=green><b>Account " + username + " added to database!</b></color>");

            debug.LogFormat(this.name, nameof(RegisterUser), userName); //DEBUG //TODO: Evaluate this - username and userName are not the same variable
		}
        
		// @Server
        /// <summary>
        /// Method <c>LoginUser</c>.
        /// Run on the server.
        /// Logs the user in.
        /// </summary>
        /// <param name="conn"></param><param name="username"></param>
		protected void LoginUser(NetworkConnection conn, string username)
		{

            if (!onlineUsers.ContainsKey(conn))
            {
                onlineUsers.Add(conn, username);
                Debug.Log("<color=green><b>Account " + username + " logged in to server " + conn.address + "!</b></color>"
                    + "\n<b>Connection ID " + conn.connectionId + "</b>");
            }
            else
            {
                Debug.Log("<color=red><b>Account " + username + " was already logged into server " + conn.address + "!</b></color>"
                    + "\n<b>Connection ID " + conn.connectionId + "</b>");
            }
			
			state = NetworkState.Lobby;
			    
			DatabaseManager.singleton.LoginUser(username);
			
			this.InvokeInstanceDevExtMethods(nameof(LoginUser), conn, username); //HOOK
			debug.LogFormat(this.name, nameof(LoginUser), username); //DEBUG
		}

		// @Server
        protected void LogoutUser(NetworkConnection conn)
        {
        
        	string username = GetUserName(conn);
        	
			if (!String.IsNullOrWhiteSpace(username) && GetIsUserLoggedIn(username) )
				DatabaseManager.singleton.LogoutUser(username);
			
			onlineUsers.Remove(conn);
			
			debug.LogFormat(this.name, nameof(LogoutUser), conn.Id()); //DEBUG
        }
		
		// @Server
        protected void LogoutPlayerAndUser(NetworkConnection conn)
        {
            if (conn.identity != null)
            {
                GameObject player = conn.identity.gameObject;

                // -- logout the user as well (handled differently than LogoutUser) //TODO: Why is this handled differently?
                string userName = player.GetComponent<PlayerAccount>()._tablePlayer.username;

                DatabaseManager.singleton.LogoutUser(userName); //LOGOUT FROM DATABASE
                onlineUsers.Remove(conn);

                // -- Hooks & Events
                this.InvokeInstanceDevExtMethods(nameof(OnServerDisconnect), conn); //HOOK
                eventListeners.OnLogoutPlayer.Invoke(conn); //EVENT

                onlinePlayers.Remove(player.name);

                debug.LogFormat(this.name, nameof(LogoutPlayerAndUser), conn.Id(), player.name, userName); //DEBUG
            }
            else
            {
                LogoutUser(conn);
            }
        }
    }
}