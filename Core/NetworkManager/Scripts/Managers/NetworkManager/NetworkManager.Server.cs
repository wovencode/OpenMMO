
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
            NetworkServer.RegisterHandler<ClientMessageRequestUserRegister>(OnClientMessageRequestUserRegister);
            NetworkServer.RegisterHandler<ClientMessageRequestUserDelete>(OnClientMessageRequestUserDelete);
            NetworkServer.RegisterHandler<ClientMessageRequestUserChangePassword>(OnClientMessageRequestUserChangePassword);
            NetworkServer.RegisterHandler<ClientMessageRequestUserConfirm>(OnClientMessageRequestUserConfirm);
            
            // ---- Player
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerLogin>(OnClientMessageRequestPlayerLogin);
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerRegister>(OnClientMessageRequestPlayerRegister);
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerDelete>(OnClientMessageRequestPlayerDelete);
            

			this.InvokeInstanceDevExtMethods(nameof(OnStartServer)); //HOOK
        	eventListeners.OnStartServer.Invoke();
        	
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
			
			if (!UserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserLogin(msg.username, msg.password))
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
			}
			
			conn.Send(message);
			
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
				DatabaseManager.singleton.SaveDataUser(msg.username, false);
				message.text = systemText.userRegisterSuccess;
			}
			else
			{
				message.text = systemText.userRegisterFailure;
				message.success = false;
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
        	
        	if (DatabaseManager.singleton.TryUserDelete(msg.username, msg.password))
			{
				message.text = systemText.userDeleteSuccess;
			}
			else
			{
				message.text = systemText.userDeleteFailure;
				message.success = false;
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
        	
        	if (DatabaseManager.singleton.TryUserChangePassword(msg.username, msg.oldPassword, msg.newPassword))
			{
				message.text = systemText.userChangePasswordSuccess;
			}
			else
			{
				message.text = systemText.userChangePasswordFailure;
				message.success = false;
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
        	
        	if (DatabaseManager.singleton.TryUserConfirm(msg.username, msg.password))
			{
				message.text = systemText.userConfirmSuccess;
			}
			else
			{
				message.text = systemText.userConfirmFailure;
				message.success = false;
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
			
			if (!UserLoggedIn(msg.username) && DatabaseManager.singleton.TryPlayerLogin(msg.playername, msg.username))
			{
				LoginPlayer(conn, msg.username, msg.playername);
				message.text = systemText.playerLoginSuccess;
			}
			else
			{
				message.text = systemText.playerLoginFailure;
				message.success = false;
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
				success = true,
				text			 	= "",
				causesDisconnect 	= false
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
			}
			else
			{
				message.text = systemText.playerDeleteFailure;
				message.success = false;
			}
					
        	conn.Send(message);
        	
        }

        // ============================== MAJOR ACTIONS ==================================
        
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
			if (!UserLoggedIn(username))
			{
				onlineUsers[conn] = name;
			    state = NetworkState.Lobby;
			    
			    DatabaseManager.singleton.LoginUser(username);
			    
			    this.InvokeInstanceDevExtMethods(nameof(LoginUser)); //HOOK
			}
			else
				ServerSendError(conn, systemText.userAlreadyOnline, true);
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
		protected void LoginPlayer(NetworkConnection conn, string username, string playername)
		{
	
			DatabaseManager.singleton.LoginPlayer(playername, username);
			
			string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playername);
			
			GameObject prefab = GetPlayerPrefab(prefabname);
			GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playername);
			
			NetworkServer.AddPlayerForConnection(conn, player);
			ValidatePlayerPosition(player);
			
			onlinePlayers[player.name] = player;
			state = NetworkState.Game;
			
			// -- Hooks & Events
			this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), conn, player, prefab, username, playername); //HOOK
			eventListeners.OnLoginPlayer.Invoke(conn);

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
			DatabaseManager.singleton.SaveDataPlayer(player, false);
			
			Destroy(player);
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================