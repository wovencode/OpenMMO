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
        [DevExtMethods(nameof(PlayerLogin))]
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
                pc.WarpRemote(anchorName, zoneName, token);
            }
        }
    }
}