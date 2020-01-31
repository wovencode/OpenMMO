
using Wovencode;
using Wovencode.Network;
using Wovencode.Database;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace Wovencode.Network
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
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerSwitchServer>(OnClientMessageRequestPlayerSwitchServer);
        	
			this.InvokeInstanceDevExtMethods(nameof(OnStartServer));
        	eventListeners.OnStartServer.Invoke();
        	
        }
           
        // ===============================================================================
        // ============================= MESSAGE HANDLERS ================================
        // ===============================================================================
        
        // -------------------------------------------------------------------------------
		// OnClientMessageRequest
		// @Client -> @Server
		// --------------------------------------------------------------------------------
        void OnClientMessageRequest(NetworkConnection conn, ClientMessageRequest msg)
        {
    		// do nothing (this message is never called directly)
        }
        
        // ========================== MESSAGE HANDLERS - USER ============================
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserLogin
        // @Client -> @Server
		// -------------------------------------------------------------------------------      
        void OnClientMessageRequestUserLogin(NetworkConnection conn, ClientMessageRequestUserLogin msg)
		{
			
			ServerMessageResponseUserLogin message = new ServerMessageResponseUserLogin
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryUserLogin(msg.username, msg.password))
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
        void OnClientMessageRequestPlayerLogin(NetworkConnection conn, ClientMessageRequestPlayerLogin msg)
		{
			
			ServerMessageResponsePlayerLogin message = new ServerMessageResponsePlayerLogin
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryPlayerLogin(msg.playername, msg.username))
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
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerSwitchServer
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
       	void OnClientMessageRequestPlayerSwitchServer(NetworkConnection conn, ClientMessageRequestPlayerSwitchServer msg)
        {
        	
        	ServerMessageResponsePlayerSwitchServer message = new ServerMessageResponsePlayerSwitchServer
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerSwitchServer(msg.username, msg.token))
			{
				message.text = systemText.playerSwitchServerSuccess;
			}
			else
			{
				message.text = systemText.playerSwitchServerFailure;
				message.success = false;
			}
			
        	conn.Send(message);
        	
        }
        
        // ============================= COMMON ACTIONS ==================================
        
        // -------------------------------------------------------------------------------
		// LoginUser
		// @Server
		// -------------------------------------------------------------------------------
		protected void LoginUser(NetworkConnection conn, string name)
		{
			onlineUsers[conn] = name;
			state = NetworkState.Lobby;
			this.InvokeInstanceDevExtMethods(nameof(LoginUser));
		}
		
		// -------------------------------------------------------------------------------
		// LoginPlayer
		// @Server
		// -------------------------------------------------------------------------------
		protected void LoginPlayer(NetworkConnection conn, string username, string playername)
		{
			if (!AccountLoggedIn(playername))
			{
			
				string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playername);
				GameObject prefab = GetPlayerPrefab(prefabname);
				
				this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), prefab, username);

				GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playername);
				ValidatePlayerPosition(player);
				NetworkServer.AddPlayerForConnection(conn, player);
				onlinePlayers[player.name] = player;
				state = NetworkState.Game;
				
			}
			else
				ServerSendError(conn, systemText.userAlreadyOnline, true);
		}
		
		// -------------------------------------------------------------------------------
		// RegisterPlayer
		// @Server
		// -------------------------------------------------------------------------------
		protected void RegisterPlayer(string username, string playername, string prefabname)
		{

			GameObject prefab = GetPlayerPrefab(prefabname);
			GameObject player = Instantiate(prefab);
			player.name = playername;
			
			this.InvokeInstanceDevExtMethods(nameof(RegisterPlayer), player, username, prefabname);
		
			DatabaseManager.singleton.CreateDefaultDataPlayer(player);
			DatabaseManager.singleton.SaveDataPlayer(player, false);
			
			Destroy(player);
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================