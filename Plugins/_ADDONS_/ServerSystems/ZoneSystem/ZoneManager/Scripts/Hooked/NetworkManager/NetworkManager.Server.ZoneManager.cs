//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Network;
using System.Collections.Generic;

using OpenMMO.Zones;
using OpenMMO.Database;
using System;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
    // NetworkManager
    // ===================================================================================
    [RequireComponent(typeof(ZoneManager))]
    public partial class NetworkManager
    {

        // -----------------------------------------------------------------------------------
        // OnStartServer_NetworkPortals
        // @Server
        // -----------------------------------------------------------------------------------
        [DevExtMethods(nameof(OnStartServer))]
        void OnStartServer_NetworkPortals()
        {
            Debug.Log("[REGISTER NETWORK MESSAGES] - [CHARACTER SERVER] - [AUTOJOINPLAYER] - "
                + "Registering Message Handlers to Server..."); //DEBUG
            NetworkServer.RegisterHandler<ClientRequestPlayerAutoLogin>(OnClientMessageRequestPlayerAutoLogin);

            Debug.Log("[REGISTER NETWORK MESSAGES] - [ZONE SERVER] - [SWITCHSERVER] - "
                + "Registering Message Handlers to Server..."); //DEBUG
            NetworkServer.RegisterHandler<ClientRequestPlayerSwitchServer>(OnClientMessageRequestPlayerSwitchServer);

            ZoneManager zoneManager = GetComponent<ZoneManager>();
            if (!zoneManager) zoneManager = FindObjectOfType<ZoneManager>(); //ADDED DX4D
            if (zoneManager)
            {
                Debug.Log("[STARTUP] - [ZONE SERVER] - Launching Zone Servers..."); //DEBUG
                zoneManager.LaunchZoneServers();
                Debug.Log("[STARTUP] - [ZONE SERVER] - Finished launching Zone Servers!!!"); //DEBUG
            }
        }

        // -------------------------------------------------------------------------------
        // LoginPlayer_NetworkPortals
        // @Server
        // -------------------------------------------------------------------------------
        //[DevExtMethods(nameof(DatabaseManager.LoadPlayerFromDatabase))] //ADDED - DatabaseManager - DX4D
        [DevExtMethods(nameof(LoginPlayer))] //ADDED - LoginPlayer - DX4D
        void LoginPlayer_LoadPlayerFromDatabase_NetworkPortals(NetworkConnection conn, string _playername, string _username, int _token)
        {
            Debug.Log("[ZONE SERVER] [CHARACTER LOADING] - Loading character " + _playername + "..."); //DEBUG

            if (!ZoneManager.singleton.GetCanSwitchZone) return;

            ///*
            //GET PLAYER ACCOUNT FROM PLAYER NAME
            string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(_playername);
            //FETCH PREFAB
            GameObject prefab = GetPlayerPrefab(prefabname);
            //FETCH PLAYER OBJECT
            GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, _playername);
            if (!player) { Debug.Log("[ERROR] [ZONE SERVER] - Player not found in database..."); } //DEBUG
            //FETCH PLAYER ACCOUNT
            PlayerAccount pc = player.GetComponent<PlayerAccount>();
            if (!pc) { Debug.Log("[ERROR] [ZONE SERVER] - Player must have a PlayerAccount component..."); } //DEBUG
            //*/

            //PlayerAccount pc                  = player.GetComponent<PlayerAccount>();
            string _zonename = pc.zoneInfo.zonename;
            NetworkZoneTemplate _currentzone = pc.currentZone;

            if (!String.IsNullOrWhiteSpace(_zonename) && _zonename != _currentzone.name)
            {
                string anchorName = pc.zoneInfo.anchorname;

                Debug.Log("[ZONE SERVER] - " + "Warping " + _playername + " to " + _zonename + " with token " + _token); //DEBUG

                pc.WarpRemote(anchorName, _zonename, 0); //WARP PLAYER ACCOUNT
            }
        }

        // -------------------------------------------------------------------------------
        // SwitchServerPlayer
        // @Server -> @Client
        // @SERVER
        public void SwitchServerPlayer(NetworkConnection conn, string _playername, string _anchorname, string _zonename, int _token)
        {
            Debug.Log("[ZONE SERVER] - Sending Switch Server Message to Client... " + _playername + "#" + _token); //DEBUG

            ServerResponsePlayerSwitchServer message = new ServerResponsePlayerSwitchServer
            {
                playername = _playername,
                anchorname = _anchorname,
                zonename = _zonename,
                token = _token,
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (DatabaseManager.singleton.CanPlayerSwitchServer(_playername, _anchorname, _zonename, _token))
            {
                Debug.Log("[ZONE SERVER] - Verified ability to warp " + _playername + " to zone " + _zonename + " with token " + _token + "..."); //DEBUG
                message.text = systemText.SWITCH_SERVER_SUCCESS;
                message.success = true; //ADDED DX4D
            }
            else
            {
                Debug.Log("[ERROR] [ZONE SERVER] - Unable to warp " + _playername + " to zone " + _zonename + " with token " + _token + "..."); //DEBUG
                message.text = systemText.SWITCH_SERVER_FAILURE;
                message.success = false;
            }

            conn.Send(message);

            // -- Required: now log the user/player out server-side
            // -- it is never guaranteed that OnServerDisconnect is called correctly and in-time
            LogoutPlayerAndUser(conn);
            Debug.Log("[ZONE SERVER] - Logging out " + _playername + "@" + conn.address + " to prepare for zone transfer"); //DEBUG
        }

        // ======================== MESSAGE HANDLERS - PLAYER ============================

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerAutoLogin
        // Direction: @Client -> @Server
        //@SERVER
        void OnClientMessageRequestPlayerAutoLogin(NetworkConnection conn, ClientRequestPlayerAutoLogin msg)
        {

            ServerResponsePlayerAutoLogin message = new ServerResponsePlayerAutoLogin
            {
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (DatabaseManager.singleton.CanPlayerAutoLogin(msg.playername, msg.username))
            {
                Debug.Log("[ZONE SERVER] - Verified ability to Auto Login character " + msg.playername + " on account " + msg.username + " with token " + msg.token + "..."); //DEBUG
                AutoLoginPlayer(conn, msg.username, msg.playername, msg.token);
                message.text = systemText.CHARACTER_JOIN_SUCCESS;
                message.success = true; //ADDED DX4D
            }
            else
            {
                Debug.Log("[ERROR] [ZONE SERVER] - Unable to Auto Login character " + msg.playername + " on account " + msg.username + " with token " + msg.token + "..."); //DEBUG
                message.text = systemText.CHARACTER_JOIN_FAILURE;
                message.success = false;
            }

            conn.Send(message);

        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerSwitchServer
        // Direction: @Client -> @Server
        //@SERVER
        void OnClientMessageRequestPlayerSwitchServer(NetworkConnection conn, ClientRequestPlayerSwitchServer msg)
        {

            ServerResponsePlayerSwitchServer message = new ServerResponsePlayerSwitchServer
            {
                success = true,
                text = "",
                causesDisconnect = false,
                token = msg.token //ADDED DX4D
            };

            if (DatabaseManager.singleton.CanPlayerSwitchServer(msg.playername, msg.anchorname, msg.zonename, msg.token))
            {
                Debug.Log("[ZONE SERVER] - Verified ability to Switch Server for character " + msg.playername + " with token " + msg.token + "..."); //DEBUG
                message.text = systemText.SWITCH_SERVER_SUCCESS;
                message.success = true; //ADDED DX4D
            }
            else
            {
                Debug.Log("[ERROR] [ZONE SERVER] - Unable to Switch Server for character " + msg.playername + " with token " + msg.token + "..."); //DEBUG
                message.text = systemText.SWITCH_SERVER_FAILURE;
                message.success = false;
            }

            conn.Send(message);

        }

        // ============================== MAJOR ACTIONS ==================================

        // -------------------------------------------------------------------------------
        // AutoLoginPlayer
        //@SERVER
        protected void AutoLoginPlayer(NetworkConnection conn, string _username, string _playername, int _token)
        {

            GameObject player = LoginPlayer(conn, _username, _playername, _token);

            if (!player)
            {
                Debug.Log("[ERROR] [ZONE SERVER] - Auto Login failed for character " + _playername + " on account " + _username + " with token " + _token + "..."); //DEBUG
                ServerSendError(conn, systemText.AUTO_LOGIN_ERROR, true); //SEND ERROR TO CLIENT
                return;
            }
            PlayerAccount pc = player.GetComponent<PlayerAccount>();

            // -- log the user in (again)
            LoginUser(conn, _username);

            // -- update zone
            pc.zoneInfo.zonename = pc.currentZone.name;

            // -- warp to anchor location (if any)
            string anchorName = pc.zoneInfo.anchorname;

            if (pc.zoneInfo.startpos)                           // -- warp to start position
            {
                Debug.Log("[ZONE SERVER] - Warping character " + _playername + " on account " + _username + " with token " + _token + "..."); //DEBUG
                pc.WarpLocal(AnchorManager.singleton.GetArchetypeStartPositionAnchorName(player));
                pc.zoneInfo.startpos = false;
            }
            else if (!String.IsNullOrWhiteSpace(anchorName))            // -- warp to anchor
            {
                pc.WarpLocal(anchorName);
                pc.zoneInfo.anchorname = "";
            }
        }
    }
}
