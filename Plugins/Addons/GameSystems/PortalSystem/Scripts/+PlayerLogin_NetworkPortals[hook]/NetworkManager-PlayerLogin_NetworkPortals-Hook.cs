//BY FHIZ
//MODIFIED BY DX4D
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
        [DevExtMethods(nameof(PlayerCharacterLogin))] //ADDED - DX4D
        //[DevExtMethods(nameof(DatabaseManager.LoginPlayer))] //REMOVED - DX4D
        //void PlayerLogin_NetworkPortals(NetworkConnection conn, GameObject player, string playerName, string userName)
        void PlayerLogin_NetworkPortals(NetworkConnection conn, string playerName, string userName, int token)
        {
            if (!ZoneManager.singleton.GetCanSwitchZone) return;

            string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playerName);

            GameObject prefab = GetPlayerPrefab(prefabname);
            GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playerName);

            PlayerAccount pc = player.GetComponent<PlayerAccount>();
            string zoneName = pc.zoneInfo.zonename;
            NetworkZoneTemplate currentZone = pc.currentZone;

            if (!String.IsNullOrWhiteSpace(zoneName) && zoneName != currentZone.name)
            {
                string anchorName = pc.zoneInfo.anchorname;

                // -- issue warp (no token required as it is server side)
                //NOTE: You MUST pass 0 as the token here or the player character will not start in the right zone.
                pc.WarpRemote(anchorName, zoneName, 0);
            }
        }
    }
}