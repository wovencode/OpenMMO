//BY FHIZ
// Direction: @Client -> @Server
// Execution: @Server

using OpenMMO.Database;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnClientRequestUserLogin</c>.
        /// Triggered when the server receives a user login request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientRequestUserLogin(NetworkConnection conn, ClientRequestUserLogin msg)
		{
			ServerResponseUserLogin message = new ServerResponseUserLogin
            {
                action              = NetworkAction.UserLogin, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserLogin(msg.username, msg.password))
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
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserLogin), conn.Id(), "DENIED"); //DEBUG
				
			}
			
			conn.Send(message);
		}
		// @Server
        /// <summary>
        /// Method <c>LoginUser</c>.
        /// Run on the server.
        /// Logs the user in.
        /// </summary>
        /// <param name="conn"></param><param name="username"></param>
		protected void LoginUser(NetworkConnection conn, string username)
		{

            if (!onlineUsers.ContainsKey(conn))
            {
                onlineUsers.Add(conn, username);
                Debug.Log("<color=green><b>Account " + username + " logged in to server " + conn.address + "!</b></color>"
                    + "\n<b>Connection ID " + conn.connectionId + "</b>");
            }
            else
            {
                Debug.Log("<color=red><b>Account " + username + " was already logged into server " + conn.address + "!</b></color>"
                    + "\n<b>Connection ID " + conn.connectionId + "</b>");
            }
			
			state = NetworkState.Lobby;
			    
			DatabaseManager.singleton.LoginUser(username);
			
			this.InvokeInstanceDevExtMethods(nameof(LoginUser), conn, username); //HOOK
			debug.LogFormat(this.name, nameof(LoginUser), username); //DEBUG
		}
    }
}