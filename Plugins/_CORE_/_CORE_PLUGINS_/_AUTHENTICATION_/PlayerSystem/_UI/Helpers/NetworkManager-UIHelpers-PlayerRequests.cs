//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
    // NetworkManager
    // ===================================================================================
    public partial class NetworkManager
    {
        // L O G I N  P L A Y E R

        // -------------------------------------------------------------------------------
        // TryLoginPlayer
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryLoginPlayer</c>.
        /// Tries to login the player.
        /// Runs on the client. 
        /// </summary>
        /// <param name="username"></param>
        // @Client
        public void TryLoginPlayer(string playername)
        {
            //RequestPlayerLogin(NetworkClient.connection, username, userName); //REMOVED - DX4D
            RequestPlayerLogin(playername, userName); //ADDED - DX4D
        }
        // -------------------------------------------------------------------------------
        // RequestPlayerLogin
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Protected override function <c>RequestPlayerLogin</c> that returns a boolean.
        /// Sends a player login request to the server.
        /// Checks whether the player login request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playername"></param><param name="username"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        // @Client
        //protected override bool RequestPlayerLogin(NetworkConnection conn, string playername, string username) //REMOVED - DX4D
        protected override bool RequestPlayerLogin(string playername, string username) //ADDED - DX4D
        {

            //if (!base.RequestPlayerLogin(conn, playername, username)) //REMOVED - DX4D
            if (!CanPlayerLogin(playername, username)) return false; //ADDED - DX4D

            Request.PlayerLoginRequest message = new Request.PlayerLoginRequest
            {
                playername = playername,
                username = username,
                success = true
            };

            // must be readied here, not in the response - otherwise it generates a warning
            //ClientScene.Ready(conn); //REMOVED - DX4D
            if (!NetworkClient.ready) NetworkClient.Ready(); //ADDED - DX4D

            //NetworkConnection conn = NetworkClient.connection; //REMOVED - DX4D
            //conn.Send(message); //REMOVED - DX4D

            NetworkClient.connection.Send(message); //ADDED - DX4D

            //debug.LogFormat(this.name, nameof(RequestPlayerLogin), conn.Id(), username, playername); //DEBUG //REMOVED - DX4D

            return true;
        }

        // R E G I S T E R  P L A Y E R

        /// <summary>
        /// Public function <c>TryRegisterPlayer</c>.
        /// Tries to register the player.
        /// Runs on the client.
        /// </summary>
        /// <param name="playerName"></param><param name="prefabName"></param>
        // @Client
        public void TryRegisterPlayer(string playerName, string prefabName)
        {
            //RequestPlayerRegister(NetworkClient.connection, playerName, userName, prefabName); //REMOVED - DX4D
            RequestPlayerRegister(playerName, userName, prefabName); //ADDED - DX4D
        }
        /// <summary>
        /// Protected override function <c>RequestPlayerRegister</c> that returns a boolean.
        /// Sends a player register request to the server.
        /// Checks whether the player register request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="playerName"></param><param name="userName"></param><param name="prefabName"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        // @Client
        //protected override bool RequestPlayerRegister(NetworkConnection conn, string playerName, string userName, string prefabName) //REMOVED - DX4D
        protected override bool RequestPlayerRegister(string playerName, string userName, string prefabName) //ADDED - DX4D
        {
            //if (!base.RequestPlayerRegister(conn, playerName, userName, prefabName)) //REMOVED - DX4D
            if (!CanPlayerRegister(playerName, userName, prefabName)) return false; //ADDED - DX4D

            Request.PlayerRegisterRequest message = new Request.PlayerRegisterRequest
            {
                playername = playerName,
                username = userName,
                prefabname = prefabName,
                success = true
            };

            //NetworkConnection conn = NetworkClient.connection; //REMOVED - DX4D
            //conn.Send(message); //REMOVED - DX4D

            NetworkClient.connection.Send(message); //ADDED - DX4D

            //debug.LogFormat(this.name, nameof(RequestPlayerRegister), conn.Id(), playerName, prefabName); //DEBUG //REMOVED - DX4D

            return true;

        }
        /// <summary>Can we register a new player with the provided name?
        /// Public function <c>CanRegisterPlayer</c>.
        /// Checks whether the player can be registered with the provided name.
        /// Runs on the client.
        /// //Called by <see cref="UI.UIWindowPlayerCreate"/>
        /// </summary>
        /// <param name="playername"></param>
        /// <returns></returns>
        // @Client
        public bool CanRegisterPlayer(string playername)
        {
            return Tools.IsAllowedName(playername);
        }

        // D E L E T E  P L A Y E R

        /// <summary>
        /// Public function <c>TryDeletePlayer</c>.
        /// Tries to delete the player.
        /// Runs on the client.
        /// </summary>
        /// <param name="playerName"></param>
        // @Client
#if _CLIENT && _SERVER //HOST AND PLAY
        bool deleting = false;
        public void TryDeletePlayer(string playerName)
        {
            if (deleting) { deleting = false; return; } //SKIP SECOND DELETE IN HOST MODE (Fixes Double Deletion Bug)
#else //CLIENT ONLY
        public void TryDeletePlayer(string playerName)
        {
#endif
            //if (RequestPlayerDelete(NetworkClient.connection, playerName, userName)) //REMOVED - DX4D
            if (RequestPlayerDelete(playerName, userName)) //ADDED - DX4D
            {
                for (int i = 0; i < playerPreviews.Count; i++)
                {
                    if (playerPreviews[i].playername == playerName)
                    {
                        Debug.Log("[CLIENT][DELETE] Deleting character " + playerName + " at slot " + i);
                        playerPreviews.RemoveAt(i);
                        return; //ADDED - WE ONLY WANT TO DELETE ONE CHARACTER - DX4D
                    }
                }
            }
        }
        /// <summary>
        /// Protected override function <c>RequestPlayerDelete</c> that returns a boolean.
        /// Sends a player deletion request to the server.
        /// Checks whether the player deletion request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="playerName"></param><param name="userName"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        // @Client
        //protected override bool RequestPlayerDelete(NetworkConnection conn, string playerName, string userName, int action=1) //REMOVED - DX4D
        //protected override bool RequestPlayerDelete(string playerName, string userName, int action = 1) //REMOVED - DX4D
        protected override bool RequestPlayerDelete(string playerName, string userName) //ADDED - DX4D
        {
            //if (!base.RequestPlayerDelete(conn, playerName, userName)) //REMOVED - DX4D
            if (!CanPlayerDelete(playerName, userName)) return false; //ADDED - DX4D

            Request.PlayerDeleteRequest message = new Request.PlayerDeleteRequest
            {
                playername = playerName,
                username = userName,
                success = true
            };

            //NetworkConnection conn = NetworkClient.connection; //REMOVED - DX4D
            //conn.Send(message); //REMOVED DX4D

            NetworkClient.connection.Send(message); //ADDED - DX4D

            //debug.LogFormat(this.name, nameof(RequestPlayerDelete), conn.Id(), userName); //DEBUG //REMOVED - DX4D

            return true;
        }
    }
}
