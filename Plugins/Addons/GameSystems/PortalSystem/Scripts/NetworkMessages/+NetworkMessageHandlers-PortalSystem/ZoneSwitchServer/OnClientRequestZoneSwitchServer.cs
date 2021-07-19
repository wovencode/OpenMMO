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
	}
}