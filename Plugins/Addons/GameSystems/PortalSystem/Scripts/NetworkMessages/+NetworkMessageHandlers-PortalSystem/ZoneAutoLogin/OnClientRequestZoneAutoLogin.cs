//BY FHIZ
//MODIFIED BY DX4D
//Direction @Server -> @Client
//Execution: @Server

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
		protected void AutoLoginPlayer(NetworkConnection conn, string username, string playername, int token)
		{
			GameObject player = PlayerCharacterLogin(conn, username, playername, token);

            if (player)
            {
                PlayerAccount pc = player.GetComponent<PlayerAccount>();

                // -- log the user in (again)
                LoginUser(conn, username); //TODO: Evaluate this - We should probably do this before the PlayerLogin above

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