//BY FHIZ
//MODIFIED BY DX4D
//Direction: @Client -> @Server
//Execution: @Server

using OpenMMO.Database;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnClientRequestPlayerLogin</c>.
        /// Triggered by the server receiving a player login request from the client.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestPlayerLogin(NetworkConnection conn, ClientRequestPlayerLogin msg)
		{
			
			ServerResponsePlayerLogin message = new ServerResponsePlayerLogin
            {
                action              = NetworkAction.PlayerLogin, //ADDED - DX4D
                success 			= true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			// -- check for GetIsUserLoggedIn because that covers all players on the account
			if (GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryPlayerLogin(msg.playername, msg.username))
			{
				PlayerCharacterLogin(conn, msg.username, msg.playername, 0); //dont check for token
				message.text = systemText.playerLoginSuccess;
			}
			else
			{
				message.text = systemText.playerLoginFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestPlayerLogin), conn.Id(), "DENIED"); //DEBUG
			}
			
			conn.Send(message);
		}

		// @Server
        /// <summary>
        /// Method <c>PlayerCharacterLogin</c>.
        /// Run on the server.
        /// Logs in the player.
        /// </summary>
        /// <param name="conn"></param><param name="username"></param><param name="playername"></param><param name="token"></param>
        protected GameObject PlayerCharacterLogin(NetworkConnection conn, string username, string playername, int token)
		{
			string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playername);
			
			GameObject prefab = GetPlayerPrefab(prefabname);
			GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playername);
			
			PlayerAccount pc = player.GetComponent<PlayerAccount>();
			
			// -- check the security token if required
			if (token == 0 || (token > 0 && pc.zoneInfo.ValidateToken(token)) )
			{
			
				// -- log the player in
				DatabaseManager.singleton.LoginPlayer(conn, player, playername, username);
                
                if (NetworkServer.AddPlayerForConnection(conn, player))
                {
                    onlinePlayers.Add(player.name, player);
                    Debug.Log("<color=green><b>Character " + playername + " joined server " + conn.address + "!</b></color>"
                        + "\n<b>Connection ID " + conn.connectionId + "</b>");
                }
                else
                {
                    Debug.Log("<color=red><b>Character " + playername + " has already joined server " + conn.address + "!</b></color>"
                        + "\n<b>Connection ID " + conn.connectionId + "</b>");
                }
				state = NetworkState.Game;
			
				this.InvokeInstanceDevExtMethods(nameof(PlayerCharacterLogin), conn, playername, username, token); //HOOK
				eventListeners.OnLoginPlayer.Invoke(conn); //EVENT

#if DEBUG
                Debug.Log(!player ? "! LOGIN FAILED !" : "! LOGIN SUCCESS !"
                    + "\n" + name
                    + "\n" + nameof(PlayerCharacterLogin)
                    + "\n" + username
                    + "\n" + playername
                    );
#endif
                debug.LogFormat(this.name, nameof(PlayerCharacterLogin), username, playername); //DEBUG
				
				return player;
			}
			
			return null;
		}

        //TODO LOGOUTPLAYER
        
    }
}