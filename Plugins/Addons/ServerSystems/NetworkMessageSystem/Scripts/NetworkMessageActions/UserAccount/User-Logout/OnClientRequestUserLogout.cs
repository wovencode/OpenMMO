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
    }
}