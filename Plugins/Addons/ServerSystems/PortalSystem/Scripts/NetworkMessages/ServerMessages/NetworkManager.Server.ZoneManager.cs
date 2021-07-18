//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Zones;
using OpenMMO.Database;
using System;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
	[RequireComponent(typeof(ZoneManager))]
	public partial class NetworkManager
	{
		// -------------------------------------------------------------------------------
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
		// @Server -> @Client
		// -------------------------------------------------------------------------------
		public void SwitchServerPlayer(NetworkConnection conn, string playername, string anchorName, string zoneName, int _token)
		{
            ServerResponseZoneSwitchServer message = new ServerResponseZoneSwitchServer
            {
                action              = NetworkAction.ZoneSwitchServer, //ADDED - DX4D
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
        // Direction: @Client -> @Server
        // Execution: @Server
		// -------------------------------------------------------------------------------     
        void OnClientRequestZoneAutoLogin(NetworkConnection conn, ClientRequestZoneAutoLogin msg)
		{
			ServerResponseZoneAutoLogin message = new ServerResponseZoneAutoLogin
			{
                action              = NetworkAction.ZoneAutoLogin, //ADDED - DX4D
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
        // Direction: @Client -> @Server
        // Execution: @Server
        // -------------------------------------------------------------------------------    
        void OnClientRequestZoneSwitchServer(NetworkConnection conn, ClientRequestZoneSwitchServer msg)
        {
        	ServerResponseZoneSwitchServer message = new ServerResponseZoneSwitchServer
            {
                action              = NetworkAction.ZoneSwitchServer, //ADDED - DX4D
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
        
        // -------------------------------------------------------------------------------
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

                if (pc.zoneInfo.startpos)                           // -- warp to start position
                {
                    pc.WarpLocal(LocationMarkerManager.singleton.GetArchetypeStartPositionAnchorName(player));
                    pc.zoneInfo.startpos = false;
                }
                else if (!String.IsNullOrWhiteSpace(anchorName))            // -- warp to anchor
                {
                    pc.WarpLocal(anchorName);
                    pc.zoneInfo.anchorname = "";
                }
            }
            else
            {
                ServerSendError(conn, systemText.unknownError, true);
            }
		}
	}
}