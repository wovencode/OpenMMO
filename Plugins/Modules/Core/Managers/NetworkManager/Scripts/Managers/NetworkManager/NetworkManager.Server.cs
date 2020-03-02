
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
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
		// OnStartServer
		// @Server
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public Event <c>OnStartServer</c>.
        /// Triggered when the server starts.
        /// Registers all the server's event handlers.
        /// Runs on server.
        /// </summary>
        public override void OnStartServer()
        {
        
            // ---- User
            NetworkServer.RegisterHandler<ClientMessageRequestUserLogin>(OnClientMessageRequestUserLogin);
            NetworkServer.RegisterHandler<ClientMessageRequestUserLogout>(OnClientMessageRequestUserLogout);
            NetworkServer.RegisterHandler<ClientMessageRequestUserRegister>(OnClientMessageRequestUserRegister);
            NetworkServer.RegisterHandler<ClientMessageRequestUserDelete>(OnClientMessageRequestUserDelete);
            NetworkServer.RegisterHandler<ClientMessageRequestUserChangePassword>(OnClientMessageRequestUserChangePassword);
            NetworkServer.RegisterHandler<ClientMessageRequestUserConfirm>(OnClientMessageRequestUserConfirm);
            
            // ---- Player
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerLogin>(OnClientMessageRequestPlayerLogin);
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerRegister>(OnClientMessageRequestPlayerRegister);
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerDelete>(OnClientMessageRequestPlayerDelete);
            

			this.InvokeInstanceDevExtMethods(nameof(OnStartServer)); //HOOK
        	eventListeners.OnStartServer.Invoke(); //EVENT
        	
        }
           
        // ===============================================================================
        // ============================= MESSAGE HANDLERS ================================
        // ===============================================================================
        
        // -------------------------------------------------------------------------------
		// OnClientMessageRequest
		// @Client -> @Server
		// --------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnClientMessageRequest</c>.
        /// Triggerd when the server receives a Client message request from the client.
        /// Doesn't do anything as htis message is never called directly.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequest(NetworkConnection conn, ClientMessageRequest msg)
        {
    		// do nothing (this message is never called directly)
        }

        // ========================== MESSAGE HANDLERS - USER ============================

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserLogin
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnClientMessageRequestUserLogin</c>.
        /// Triggered when the server receives a user login request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequestUserLogin(NetworkConnection conn, ClientMessageRequestUserLogin msg)
		{
			
			ServerMessageResponseUserLogin message = new ServerMessageResponseUserLogin
			{
				success = true,
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
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestUserLogin), conn.Id(), "DENIED"); //DEBUG
				
			}
			
			conn.Send(message);
			
		}
		
		// -------------------------------------------------------------------------------
        // OnClientMessageRequestUserLogout
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        void OnClientMessageRequestUserLogout(NetworkConnection conn, ClientMessageRequestUserLogout msg)
		{
			LogoutUser(conn);
		}
		
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserRegister
        // @Client -> @Server
        // -------------------------------------------------------------------------------    
        /// <summary>
        /// Event <c>OnClientMessageRequestUserRegister</c>.
        /// Triggered when the server receives a user registration request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequestUserRegister(NetworkConnection conn, ClientMessageRequestUserRegister msg)
        {
        	
        	ServerMessageResponseUserRegister message = new ServerMessageResponseUserRegister
			{
				success = true,
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
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestUserRegister), conn.Id(), "DENIED"); //DEBUG
				
			}

        	conn.Send(message);
        	
        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserDelete
        // @Client -> @Server
        // -------------------------------------------------------------------------------    
        /// <summary>
        /// Event <c>OnClientMessageRequestUserDelete</c>.
        /// Triggered when the server receives a user deletion request.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequestUserDelete(NetworkConnection conn, ClientMessageRequestUserDelete msg)
        {
        	
        	ServerMessageResponseUserDelete message = new ServerMessageResponseUserDelete
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
        	if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserDelete(msg.username, msg.password))
			{
				message.text = systemText.userDeleteSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestUserDelete), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.userDeleteFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestUserDelete), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        	
        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserChangePassword
        // @Client -> @Server
        // -------------------------------------------------------------------------------  
        /// <summary>
        /// Event <c>OnClientMessageRequestUserChangePassword</c>.
        /// Triggered when the server receives a user change password request.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequestUserChangePassword(NetworkConnection conn, ClientMessageRequestUserChangePassword msg)
        {
        	
        	ServerMessageResponseUserChangePassword message = new ServerMessageResponseUserChangePassword
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
        	if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserChangePassword(msg.username, msg.oldPassword, msg.newPassword))
			{
				message.text = systemText.userChangePasswordSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestUserChangePassword), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.userChangePasswordFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestUserChangePassword), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        	
        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserConfirm
        // @Client -> @Server
        // -------------------------------------------------------------------------------    
        /// <summary>
        /// Event <c>OnClientMessageRequestUserConfirm</c>.
        /// Triggered by the server receiving a user confirmation request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequestUserConfirm(NetworkConnection conn, ClientMessageRequestUserConfirm msg)
        {
        	
        	ServerMessageResponseUserConfirm message = new ServerMessageResponseUserConfirm
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserConfirm(msg.username, msg.password))
			{
				message.text = systemText.userConfirmSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestUserConfirm), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.userConfirmFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestUserConfirm), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        	
        }
        
        // ======================== MESSAGE HANDLERS - PLAYER ============================
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerLogin
        // @Client -> @Server
		// -------------------------------------------------------------------------------     
        /// <summary>
        /// Event <c>OnClientMessageRequestPlayerLogin</c>.
        /// Triggered by the server receiving a player login request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequestPlayerLogin(NetworkConnection conn, ClientMessageRequestPlayerLogin msg)
		{
			
			ServerMessageResponsePlayerLogin message = new ServerMessageResponsePlayerLogin
			{
				success 			= true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			// -- check for GetIsUserLoggedIn because that covers all players on the account
			if (GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryPlayerLogin(msg.playername, msg.username))
			{
				LoginPlayer(conn, msg.username, msg.playername, 0); //dont check for token
				message.text = systemText.playerLoginSuccess;
			}
			else
			{
				message.text = systemText.playerLoginFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestPlayerLogin), conn.Id(), "DENIED"); //DEBUG
				
			}
			
			conn.Send(message);
			
		}

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerRegister
        // @Client -> @Server
        // -------------------------------------------------------------------------------    
        /// <summary>
        /// Event <c>OnClientMessageRequestPlayerRegister</c>.
        /// Triggered by the server receiving a player regstration request from the client. 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequestPlayerRegister(NetworkConnection conn, ClientMessageRequestPlayerRegister msg)
        {
        	
        	ServerMessageResponsePlayerRegister message = new ServerMessageResponsePlayerRegister
			{
				success 			= true,
				text			 	= "",
				causesDisconnect 	= false,
				playername 			= msg.playername
			};
        	
        	if (DatabaseManager.singleton.TryPlayerRegister(msg.playername, msg.username, msg.prefabname))
			{
				RegisterPlayer(msg.username, msg.playername, msg.prefabname);
				message.text = systemText.playerRegisterSuccess;
			}
			else
			{
				message.text = systemText.playerRegisterFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestPlayerRegister), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        	
        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerDelete
        // @Client -> @Server
        // -------------------------------------------------------------------------------   
        /// <summary>
        /// Event <c>OnClientMessageRequestPlayerDelete</c>.
        /// Triggered by the server receiving a player deletion request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequestPlayerDelete(NetworkConnection conn, ClientMessageRequestPlayerDelete msg)
        {
        	
        	ServerMessageResponsePlayerDelete message = new ServerMessageResponsePlayerDelete
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerDeleteSoft(msg.playername, msg.username))
			{
				message.text = systemText.playerDeleteSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestPlayerDelete), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.playerDeleteFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientMessageRequestPlayerDelete), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        	
        }

        // ============================== MAJOR ACTIONS ==================================
        
        // -------------------------------------------------------------------------------
		// LogoutUser
		// @Server
		// -------------------------------------------------------------------------------
        protected void LogoutUser(NetworkConnection conn)
        {
        
        	string username = GetUserName(conn);
        	
			if (!String.IsNullOrWhiteSpace(username) && GetIsUserLoggedIn(username) )
				DatabaseManager.singleton.LogoutUser(username);
			
			onlineUsers.Remove(conn);
			
			debug.LogFormat(this.name, nameof(LogoutUser), conn.Id()); //DEBUG
			
        }
        
        // -------------------------------------------------------------------------------
		// LogoutPlayerAndUser
		// @Server
		// -------------------------------------------------------------------------------
        protected void LogoutPlayerAndUser(NetworkConnection conn)
        {
        	if (conn.identity != null)
			{
				
				GameObject player = conn.identity.gameObject;
				
				// -- logout the user as well (handled differently than LogoutUser)
				string userName = player.GetComponent<PlayerComponent>().tablePlayer.username;
				
				DatabaseManager.singleton.LogoutUser(userName);
				onlineUsers.Remove(conn);
				
				// -- Hooks & Events
				this.InvokeInstanceDevExtMethods(nameof(OnServerDisconnect), conn); //HOOK
				eventListeners.OnLogoutPlayer.Invoke(conn); //EVENT
				
				onlinePlayers.Remove(player.name);
				
				debug.LogFormat(this.name, nameof(LogoutPlayerAndUser), conn.Id(), player.name, userName); //DEBUG
				
			}
			else
				LogoutUser(conn);
        }
        
        // -------------------------------------------------------------------------------
		// RegisterUser
		// @Server
		// -------------------------------------------------------------------------------
		protected void RegisterUser(string username)
		{
			// isNew = true
			// Transaction = false
			DatabaseManager.singleton.SaveDataUser(username, true, false);
			debug.LogFormat(this.name, nameof(RegisterUser), userName); //DEBUG
		}
        
        // -------------------------------------------------------------------------------
		// LoginUser
		// @Server
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Method <c>LoginUser</c>.
        /// Run on the server.
        /// Logs the user in.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
		protected void LoginUser(NetworkConnection conn, string username)
		{
			
			onlineUsers.Add(conn, username);
			
			state = NetworkState.Lobby;
			    
			DatabaseManager.singleton.LoginUser(username);
			
			this.InvokeInstanceDevExtMethods(nameof(LoginUser)); //HOOK
			debug.LogFormat(this.name, nameof(LoginUser), username); //DEBUG
			
		}
		
		// -------------------------------------------------------------------------------
		// LoginPlayer
		// @Server
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Method <c>LoginPlayer</c>.
        /// Run on the server.
        /// Logs in the player.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <param name="playername"></param>
		protected GameObject LoginPlayer(NetworkConnection conn, string username, string playername, int token)
		{

			string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playername);
			
			GameObject prefab = GetPlayerPrefab(prefabname);
			GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playername);
			
			PlayerComponent pc = player.GetComponent<PlayerComponent>();
			
			// -- check the security token if required
			if (token == 0 || (token > 0 && pc.tablePlayerZones.ValidateToken(token)) )
			{
			
				// -- log the player in
				DatabaseManager.singleton.LoginPlayer(conn, player, playername, username);
			
				NetworkServer.AddPlayerForConnection(conn, player);
			
				onlinePlayers.Add(player.name, player);
				
				state = NetworkState.Game;
			
				this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), conn, player, playername, username); //HOOK
				eventListeners.OnLoginPlayer.Invoke(conn); //EVENT
			
				debug.LogFormat(this.name, nameof(LoginPlayer), username, playername); //DEBUG
				
				return player;
				
			}
			
			return null;
			
		}
		
		// -------------------------------------------------------------------------------
		// RegisterPlayer
		// @Server
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Method <c>RegisterPlayer</c>.
        /// Runs on the server.
        /// Registers the player.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="playername"></param>
        /// <param name="prefabname"></param>
		protected void RegisterPlayer(string username, string playername, string prefabname)
		{

			GameObject prefab = GetPlayerPrefab(prefabname);
			GameObject player = Instantiate(prefab);
			player.name = playername;
			
			this.InvokeInstanceDevExtMethods(nameof(RegisterPlayer), player, username, prefabname); //HOOK
		
			DatabaseManager.singleton.CreateDefaultDataPlayer(player);
			
			// -- Save Player Data
			// isNew = true
			// Transaction = false
			DatabaseManager.singleton.SaveDataPlayer(player, true, false);
			
			Destroy(player);
			
			debug.LogFormat(this.name, nameof(RegisterPlayer), username, playername, prefabname); //DEBUG
			
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================