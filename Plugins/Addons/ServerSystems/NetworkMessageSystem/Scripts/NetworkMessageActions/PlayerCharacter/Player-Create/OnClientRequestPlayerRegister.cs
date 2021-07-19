//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Database;
using UnityEngine;
using System;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client -> @Server
        /// <summary>
        /// Event <c>OnClientRequestPlayerRegister</c>.
        /// Triggered by the server receiving a player regstration request from the client. 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientRequestPlayerRegister(NetworkConnection conn, ClientRequestPlayerRegister msg)
        {

            ServerResponsePlayerRegister message = new ServerResponsePlayerRegister
            {
                action = NetworkAction.PlayerRegister, //ADDED - DX4D
                success = true,
                text = "",
                causesDisconnect = false,
                playername = msg.playername
            };

            if (DatabaseManager.singleton.TryPlayerRegister(msg.playername, msg.username, msg.prefabname))
            {
                RegisterPlayer(msg.username, msg.playername, msg.prefabname);
                message.text = systemText.playerRegisterSuccess;
            }
            else
            {
                message.text = systemText.playerRegisterFailure;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientRequestPlayerRegister), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);

        }

        // @Server
        /// <summary>
        /// Method <c>RegisterPlayer</c>.
        /// Runs on the server.
        /// Registers the player.
        /// </summary>
        /// <param name="username"></param><param name="playername"></param><param name="prefabname"></param>
        protected void RegisterPlayer(string username, string playername, string prefabname)
		{
			GameObject prefab = GetPlayerPrefab(prefabname);
			GameObject player = Instantiate(prefab);
			player.name = playername;
			
			this.InvokeInstanceDevExtMethods(nameof(RegisterPlayer), player, username, prefabname); //HOOK
		
			DatabaseManager.singleton.CreateDefaultDataPlayer(player);
			
			// -- Save Player Data
			// isNew = true
			// Transaction = false
			DatabaseManager.singleton.SaveDataPlayer(player, true, false);
			
			Destroy(player);
			
			debug.LogFormat(this.name, nameof(RegisterPlayer), username, playername, prefabname); //DEBUG
		}   
    }
}