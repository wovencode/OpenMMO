//By Fhiz
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Zones;
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
	[RequireComponent(typeof(ZoneManager))]
	public partial class NetworkManager
	{
   		
   		// -----------------------------------------------------------------------------------
		// OnStartServer_NetworkPortals
		// @Server
		// -----------------------------------------------------------------------------------
		[DevExtMethods(nameof(OnStartServer))]
		void OnStartServer_NetworkPortals()
		{
			
			NetworkServer.RegisterHandler<ClientRequestPlayerSwitchServer>(OnClientMessageRequestPlayerSwitchServer);
            NetworkServer.RegisterHandler<ClientRequestPlayerAutoLogin>(OnClientMessageRequestPlayerAutoLogin);
            
            if (GetComponent<ZoneManager>() != null)
   				GetComponent<ZoneManager>().SpawnSubZones();
		}
   		
		// -------------------------------------------------------------------------------
		// LoginPlayer_NetworkPortals
		// @Server
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoginPlayer))]
		void LoginPlayer_NetworkPortals(NetworkConnection conn, GameObject player, string playerName, string userName)
		{
			
			if (!ZoneManager.singleton.GetCanSwitchZone)
				return;
			
			PlayerAccount pc 				= player.GetComponent<PlayerAccount>();
			string zoneName 				= pc.zoneInfo.zonename;
			NetworkZoneTemplate currentZone = pc.currentZone;
			
			if (!String.IsNullOrWhiteSpace(zoneName) && zoneName != currentZone.name)
			{
				string anchorName = pc.zoneInfo.anchorname;
				
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

            ServerResponsePlayerSwitchServer message = new ServerResponsePlayerSwitchServer
            {
                action              = NetworkAction.PlayerSwitchServer, //ADDED - DX4D
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
        	
        	// -- Required: now log the user/player out server-side
        	// -- it is never guaranteed that OnServerDisconnect is called correctly and in-time
        	LogoutPlayerAndUser(conn);
		
		}
		
		// ======================== MESSAGE HANDLERS - PLAYER ============================
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerAutoLogin
        // Direction: @Client -> @Server
        // Execution: @Server
		// -------------------------------------------------------------------------------     
        void OnClientMessageRequestPlayerAutoLogin(NetworkConnection conn, ClientRequestPlayerAutoLogin msg)
		{
			
			ServerResponsePlayerAutoLogin message = new ServerResponsePlayerAutoLogin
			{
                action              = NetworkAction.PlayerAutoLogin, //ADDED - DX4D
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
        void OnClientMessageRequestPlayerSwitchServer(NetworkConnection conn, ClientRequestPlayerSwitchServer msg)
        {
        	
        	ServerResponsePlayerSwitchServer message = new ServerResponsePlayerSwitchServer
            {
                action              = NetworkAction.PlayerSwitchServer, //ADDED - DX4D
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
			
			GameObject player = LoginPlayer(conn, username, playername, token);
			
			if (player)
			{
				
				PlayerAccount pc = player.GetComponent<PlayerAccount>();
				
				// -- log the user in (again)
				LoginUser(conn, username);
				
				// -- update zone
				pc.zoneInfo.zonename = pc.currentZone.name;
				
				// -- warp to anchor location (if any)
				string anchorName = pc.zoneInfo.anchorname;

				if (pc.zoneInfo.startpos) 							// -- warp to start position
				{
					pc.WarpLocal(AnchorManager.singleton.GetArchetypeStartPositionAnchorName(player));
					pc.zoneInfo.startpos = false;
				}
				else if (!String.IsNullOrWhiteSpace(anchorName)) 			// -- warp to anchor
				{
					pc.WarpLocal(anchorName);
					pc.zoneInfo.anchorname = "";
				}
				
			}
			else
				ServerSendError(conn, systemText.unknownError, true);
		
		}
		
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================