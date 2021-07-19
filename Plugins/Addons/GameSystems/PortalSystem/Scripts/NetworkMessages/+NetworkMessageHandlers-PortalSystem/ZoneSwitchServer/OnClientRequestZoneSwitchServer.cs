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