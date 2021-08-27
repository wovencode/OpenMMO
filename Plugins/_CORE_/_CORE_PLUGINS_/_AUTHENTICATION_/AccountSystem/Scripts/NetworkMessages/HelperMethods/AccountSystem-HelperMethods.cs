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
            {
                DatabaseManager.singleton.LogoutUser(username);
            }

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
            {
                LogoutUser(conn);
            }
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
            Debug.Log("[MASTER SERVER] [LOGIN] - "
                + "Login Success!"
                + "\n" + "Connection-" + conn.connectionId
                + " @" + conn.address
                + " attempted login with user account " + username);
        }
        void LogAccountLoginFailure(NetworkConnection conn, string username)
        {
            Debug.Log("[ERROR] [MASTER SERVER] [LOGIN] - "
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
        /// <param name="_username"></param>
        /// <param name="playername"></param>
        protected GameObject LoginPlayer(NetworkConnection conn, string _username, string _playername, int _token)
        {
            //INITIALIZE DEBUG LOGGING
            System.Text.StringBuilder debugLog = new System.Text.StringBuilder("[MASTER SERVER] [CHARACTER JOIN]"
                + "\n" + _username + "@" + ((conn != null) ? conn.address : "<disconnected>")
                + " > server@" + networkAddress.ToString()
                + " - " + _playername
                + "#" + _token + "...");
            
            //VALIDATE CLIENT CONNECTION TO SERVER
            if (conn == null)
            {
                conn = NetworkClient.connection; //ENSURE CONNECTION
                if (conn == null)
                {
                    debugLog.Append("\n" + "[ERROR] - Client not connected to server " + networkAddress.ToString());
                    ShowDebugLog(debugLog); //DEBUG
                    //return null; //CANCEL LOGIN
                }
                else
                {
                    debugLog.Append("\n" + "[WARNING] - Connect Failure Bypassed - We used NetworkClient.connection instead...");
                }
            }

            //LOAD PLAYER OBJECT FROM DATABASE
            string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(_playername);
            GameObject prefab = GetPlayerPrefab(prefabname);
            GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, _playername);

            //VALIDATE PLAYER OBJECT LOADED FROM DATABASE
            if (!player)
            {
                debugLog.Append("\n" + "[ERROR] - Player Object not found in database...");
                ShowDebugLog(debugLog); //DEBUG
                //return null; //CANCEL LOGIN
            }

            //LOAD PLAYER ACCOUNT FROM PLAYER OBJECT
            PlayerAccount pc = player.GetComponent<PlayerAccount>();

            //VALIDATE PLAYER ACCOUNT LOADED FROM PLAYER OBJECT
            if (!pc)
            {
                debugLog.Append("\n" + "[ERROR] - Player Object does not have a PlayerAccount component attached...");
                ShowDebugLog(debugLog); //DEBUG
                //return null; //CANCEL LOGIN
            }

            //VALIDATE TOKEN
            // -- check the security token if required
            if (_token == 0 || (_token > 0 && pc.zoneInfo.ValidateToken(_token)))
            {
                // -- log the player in
                DatabaseManager.singleton.ConnectToServer(conn, player, _playername, _username);

                if (NetworkServer.AddPlayerForConnection(conn, player))
                {
                    onlinePlayers.Add(_playername, player);

                    if (!onlinePlayers.ContainsKey(_playername))
                    {
                        debugLog.Append("\n" + "[ERROR] - " + _username + "@" + conn.address + " failed to add " + _playername + " to server @" + networkAddress.ToString());
                    }
                    //debugLog.Append("\n"
                    //    + (onlinePlayers.ContainsKey(playername) ? (username + "@"+ conn.address + " added " + playername + " to server @" + networkAddress.ToString())
                    //    : ("[ERROR] - " + username + "@" + conn.address + " failed to add " + playername + " to server @" + networkAddress.ToString()))
                    //    );
                }
                else
                {
                    //TODO: We are in a zone
                    debugLog.Append("\n" + "[ERROR] - " + _playername + " is already in the game");
                    ShowDebugLog(debugLog); //DEBUG
                    //return null; //CANCEL LOGIN
                }

                state = NetworkState.Game; //SET NETWORK STATE

                //TODO: Make sure this is the right hook, the old one wanted DatabaseManager.LoginPlayer based upon the method's parameters
                //this.InvokeInstanceDevExtMethods(nameof(DatabaseManager.LoadPlayerFromDatabase), conn, player, playername, username); //HOOK //ADDED - DatabaseManager - DX4D //REMOVED - DX4D
                this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), conn, _playername, _username, _token); //HOOK //ADDED - DX4D
                eventListeners.OnLoginPlayer.Invoke(conn); //EVENT

                //debug.LogFormat(this.name, nameof(LoginPlayer), username, playername); //DEBUG //REMOVED - DX4D
                debugLog.Append("\n" + "[SUCCESS] - " + player.name + " joined the server successfully!");
                ShowDebugLog(debugLog); //DEBUG
                return player;

            }
            else
            {
                debugLog.Append("\n" + "[ERROR] - Invalid Token " + _token + " used by account " + _username + " for joining the server @" + networkAddress.ToString());
                ShowDebugLog(debugLog); //DEBUG
                //return null; //CANCEL LOGIN
            }

            return null; //REMOVED - All success/failure cases are handled now - DX4D

        }
        //DEBUG LOGGING
        void ShowDebugLog(System.Text.StringBuilder _log)
        {
            Debug.Log(_log.ToString()); //DEBUG
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
            Debug.Log("[MASTER SERVER] [CHARACTER REGISTER] - "
                + "Registering new character " + playername + " of type " + prefabname + " on account " + username);

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
