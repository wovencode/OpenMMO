//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Database;
using UnityEngine;
using System;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
    // NetworkManager
    // ===================================================================================
    public partial class NetworkManager
    {
        // ============================== MAJOR ACTIONS ==================================

        // -------------------------------------------------------------------------------
        // LogoutUser
        // @Server
        // -------------------------------------------------------------------------------
        protected void LogoutUser(NetworkConnection conn)
        {

            string username = GetUserName(conn);

            if (!String.IsNullOrWhiteSpace(username) && GetIsUserLoggedIn(username))
                DatabaseManager.singleton.LogoutUser(username);

            onlineUsers.Remove(conn);

            debug.LogFormat(this.name, nameof(LogoutUser), conn.Id()); //DEBUG

        }

        // -------------------------------------------------------------------------------
        // LogoutPlayerAndUser
        // @Server
        // -------------------------------------------------------------------------------
        protected void LogoutPlayerAndUser(NetworkConnection conn)
        {
            if (conn.identity != null)
            {

                GameObject player = conn.identity.gameObject;

                // -- logout the user as well (handled differently than LogoutUser)
                string userName = player.GetComponent<PlayerAccount>()._tablePlayer.username;

                DatabaseManager.singleton.LogoutUser(userName);
                onlineUsers.Remove(conn);

                // -- Hooks & Events
                this.InvokeInstanceDevExtMethods(nameof(OnServerDisconnect), conn); //HOOK
                eventListeners.OnLogoutPlayer.Invoke(conn); //EVENT

                onlinePlayers.Remove(player.name);

                debug.LogFormat(this.name, nameof(LogoutPlayerAndUser), conn.Id(), player.name, userName); //DEBUG

            }
            else
                LogoutUser(conn);
        }

        // -------------------------------------------------------------------------------
        // RegisterUser
        // @Server
        // -------------------------------------------------------------------------------
        protected void RegisterUser(string username)
        {
            // isNew = true
            // Transaction = false
            DatabaseManager.singleton.SaveDataUser(username, true, false);
            debug.LogFormat(this.name, nameof(RegisterUser), userName); //DEBUG
        }

        // -------------------------------------------------------------------------------
        // LoginUser
        // @Server
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Method <c>LoginUser</c>.
        /// Run on the server.
        /// Logs the user in.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        protected void LoginUser(NetworkConnection conn, string username)
        {
            if (!onlineUsers.ContainsKey(conn))
            {
                onlineUsers.Add(conn, username);
                LogAccountLoginSuccess(conn, username); //LOG SUCCESS
            }
            else
            {
                LogAccountLoginFailure(conn, username); //LOG FAILURE
            }

            state = NetworkState.Lobby;

            DatabaseManager.singleton.LoginUser(username);

            this.InvokeInstanceDevExtMethods(nameof(LoginUser), conn, username); //HOOK
            debug.LogFormat(this.name, nameof(LoginUser), username); //DEBUG

        }
        void LogAccountLoginSuccess(NetworkConnection conn, string username)
        {
            Debug.Log("[ACCOUNT SERVER] - "
                + "Login Success!"
                + "\n" + "Connection-" + conn.connectionId
                + " @" + conn.address
                + " attempted login with user account " + username);
        }
        void LogAccountLoginFailure(NetworkConnection conn, string username)
        {
            Debug.Log("<<<ISSUE>>> [ACCOUNT SERVER] - "
                + "Account Login Failure..."
                + "\n" + "Connection-" + conn.connectionId
                + " @" + conn.address
                + " attempted login with user account " + username);
        }

        // -------------------------------------------------------------------------------
        // LoginPlayer
        // @Server
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Method <c>LoginPlayer</c>.
        /// Run on the server.
        /// Logs in the player.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <param name="playername"></param>
        protected GameObject LoginPlayer(NetworkConnection conn, string username, string playername, int token)
        {
            Debug.Log("[CHARACTER SERVER] - "
                + "Account " + username + "'s" + " character " + playername + " is attempting to join the server...");

            if (conn == null)
            {
                conn = NetworkClient.connection; //ENSURE CONNECTION
                if (conn == null)
                {
                    Debug.Log("<<<ISSUE>>> [CHARACTER SERVER] - "
                        + "Join Server Failed - Client Not Connected..."
                        + "\n" + "LoginPlayer called by account " + username + " on character " + playername + " while not connected to server " + networkAddress.ToString());
                }
                else
                {
                    Debug.Log("<<<ISSUE>>> [CHARACTER SERVER] - "
                        + "Join Server Failure Bypassed - We used the client connection instead..."
                        + "\n" + "LoginPlayer connection was null - Assigned NetworkClient.connection instead.");
                }
            }

            //GET PLAYER ACCOUNT FROM PLAYER NAME
            string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playername);
            GameObject prefab = GetPlayerPrefab(prefabname);
            GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playername);
            if (!player)
            {
                Debug.Log("<<<ISSUE>>> [CHARACTER SERVER] - "
                    + "Player not found in database...");
            }
            PlayerAccount pc = player.GetComponent<PlayerAccount>();
            if (!pc)
            {
                Debug.Log("<<<ISSUE>>> [CHARACTER SERVER] - "
                    + "Player must have a PlayerAccount component...");
            }
            // -- check the security token if required
            if (token == 0 || (token > 0 && pc.zoneInfo.ValidateToken(token)))
            {

                // -- log the player in
                DatabaseManager.singleton.LoadPlayerFromDatabase(conn, player, playername, username);

                if (NetworkServer.AddPlayerForConnection(conn, player))
                {
                    onlinePlayers.Add(playername, player);

                    Debug.Log(
                        onlinePlayers.ContainsKey(playername) ?
                        ("[CHARACTER SERVER] - "
                         + playername + " joined with client @"
                         + conn.address
                        ) : (
                        "<<<ISSUE>>> [CHARACTER SERVER] - "
                         + playername + " failed to join with client @"
                         + conn.address)
                        );
                }
                else
                {
                    //TODO: We are in a zone
                    Debug.Log("<<<ISSUE>>> [CHARACTER SERVER] - "
                        + playername + " is already in the game");
                }
                state = NetworkState.Game;

                //TODO: Make sure this is the right hook, the old one wanted DatabaseManager.LoginPlayer based upon the method's parameters
                //this.InvokeInstanceDevExtMethods(nameof(DatabaseManager.LoadPlayerFromDatabase), conn, player, playername, username); //HOOK //ADDED - DatabaseManager - DX4D //REMOVED - DX4D
                this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), conn, playername, username, token); //HOOK //ADDED - DX4D
                eventListeners.OnLoginPlayer.Invoke(conn); //EVENT

                //debug.LogFormat(this.name, nameof(LoginPlayer), username, playername); //DEBUG //REMOVED - DX4D

                return player;

            }
            else
            {
                Debug.Log("<<<ISSUE>>> [CHARACTER SERVER] - "
                    + "Invalid Token");
            }

            return null;

        }

        // -------------------------------------------------------------------------------
        // RegisterPlayer
        // @Server
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Method <c>RegisterPlayer</c>.
        /// Runs on the server.
        /// Registers the player.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="playername"></param>
        /// <param name="prefabname"></param>
        protected void RegisterPlayer(string username, string playername, string prefabname)
        {
            Debug.Log("[CHARACTER SERVER] - "
                + "Registering new character " + playername + " on account " + username);

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
