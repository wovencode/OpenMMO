using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Portals;
using OpenMMO.Database;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// NetworkManager
	// ===================================================================================
	[RequireComponent(typeof(PortalManager))]
	public partial class NetworkManager
	{
   		
   		// -----------------------------------------------------------------------------------
		// OnStartServer_NetworkPortals
		// @Server
		// -----------------------------------------------------------------------------------
		[DevExtMethods(nameof(OnStartServer))]
		void OnStartServer_NetworkPortals()
		{
			
			NetworkServer.RegisterHandler<ClientMessageRequestPlayerSwitchServer>(OnClientMessageRequestPlayerSwitchServer);
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerAutoLogin>(OnClientMessageRequestPlayerAutoLogin);
            
            if (GetComponent<PortalManager>() != null)
   				GetComponent<PortalManager>().SpawnSubZones();
		}
   		
		// -------------------------------------------------------------------------------
		// LoginPlayer_NetworkPortals
		// @Server
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoginPlayer))]
		void LoginPlayer_NetworkPortals(NetworkConnection conn, GameObject player, GameObject prefab, string userName, string playerName)
		{
			
			if (!PortalManager.singleton.GetCanSwitchZone)
				return;
			
			PlayerComponent pc 				= player.GetComponent<PlayerComponent>();
			string zoneName 				= pc.tablePlayerZones.zonename;
			NetworkZoneTemplate currentZone = pc.currentZone;
			
			if (!String.IsNullOrWhiteSpace(zoneName) && zoneName != currentZone.name)
			{
				string anchorName = pc.tablePlayerZones.anchorname;
				
				// -- issue warp (no token required as it is server side)
				pc.WarpRemote(anchorName, zoneName, 0);
				
			}
		
		}
		
		// -------------------------------------------------------------------------------
		// SwitchServerPlayer
		// @Server -> @Client
		// -------------------------------------------------------------------------------
		public void SwitchServerPlayer(NetworkConnection conn, string playername, string anchorName, string zoneName, int _token)
		{

			ServerMessageResponsePlayerSwitchServer message = new ServerMessageResponsePlayerSwitchServer
			{
				playername			= playername,
				anchorname 			= anchorName,
				zonename 			= zoneName,
				token				= _token,
				success 			= true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerSwitchServer(playername, anchorName, zoneName, _token))
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
		
		// ======================== MESSAGE HANDLERS - PLAYER ============================
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerAutoLogin
        // Direction: @Client -> @Server
        // Execution: @Server
		// -------------------------------------------------------------------------------     
        void OnClientMessageRequestPlayerAutoLogin(NetworkConnection conn, ClientMessageRequestPlayerAutoLogin msg)
		{
			
			ServerMessageResponsePlayerAutoLogin message = new ServerMessageResponsePlayerAutoLogin
			{
				success 			= true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryPlayerAutoLogin(msg.playername, msg.username))
			{
				AutoLoginPlayer(conn, msg.username, msg.playername, msg.token);
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
        // OnClientMessageRequestPlayerSwitchServer
        // Direction: @Client -> @Server
        // Execution: @Server
        // -------------------------------------------------------------------------------    
        void OnClientMessageRequestPlayerSwitchServer(NetworkConnection conn, ClientMessageRequestPlayerSwitchServer msg)
        {
        	
        	ServerMessageResponsePlayerSwitchServer message = new ServerMessageResponsePlayerSwitchServer
			{
				success 			= true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerSwitchServer(msg.playername, msg.anchorname, msg.zonename, msg.token))
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
        
        // ============================== MAJOR ACTIONS ==================================
        
        // -------------------------------------------------------------------------------
		// AutoLoginPlayer
		// @Server
		// -------------------------------------------------------------------------------
		protected void AutoLoginPlayer(NetworkConnection conn, string username, string playername, int token)
		{
		
			string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playername);
			GameObject prefab = GetPlayerPrefab(prefabname);
			
			// -- load player from database
			GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playername);
			
			PlayerComponent pc = player.GetComponent<PlayerComponent>();
			
			// -- re-validate the security token
			if (pc.tablePlayerZones.ValidateToken(token))
			{
			
				// -- update zone
				pc.tablePlayerZones.zonename = pc.currentZone.name;
				
				NetworkServer.AddPlayerForConnection(conn, player);
				
				// -- warp to anchor location
				// -- OR use start position if anchor is empty
				string anchorName = pc.tablePlayerZones.anchorname;
				
				if (!String.IsNullOrWhiteSpace(anchorName))
					pc.WarpLocal(anchorName);
				else
					player.transform.position = GetStartPosition(player).position;
				
				ValidatePlayerPosition(player);
				
				onlinePlayers[player.name] = player;
				state = NetworkState.Game;
				
				// Hooks & Events
				this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), conn, player, prefab, username, playername); //HOOK
				eventListeners.OnLoginPlayer.Invoke(conn); // -- same as OnLoginPlayer
				
			}
			else
				ServerSendError(conn, systemText.unknownError, true);
		
		}
		
                
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================