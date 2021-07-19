//BY FHIZ
// Direction: @Client -> @Server
// Execution: @Server

using OpenMMO.Database;
using UnityEngine;
using System;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnClientUserLogout</c>
        /// Triggered when the server receives a user logout request from the client.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestUserLogout(NetworkConnection conn, ClientRequestUserLogout msg)
		{
			LogoutUser(conn);
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