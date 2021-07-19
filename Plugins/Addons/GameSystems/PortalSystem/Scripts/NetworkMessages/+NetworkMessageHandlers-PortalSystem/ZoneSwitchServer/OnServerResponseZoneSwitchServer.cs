//BY FHIZ
//MODIFIED BY DX4D

using Mirror;
using UnityEngine;
using OpenMMO.Network;

namespace OpenMMO.Zones
{
    public partial class ZoneManager : MonoBehaviour
    {
        // @Client
        //public void OnServerMessageResponsePlayerSwitchServer(NetworkConnection conn, ServerResponsePlayerSwitchServer msg) //REMOVED - DX4D
        public void OnServerResponseZoneSwitchServer(ServerResponseZoneSwitchServer msg) //ADDED - DX4D
		{
			
			networkManager.StopClient();
			
			NetworkClient.Shutdown();
			OpenMMO.Network.NetworkManager.Shutdown();
			OpenMMO.Network.NetworkManager.singleton = networkManager;
			
			autoPlayerName = msg.playername;
			
			for (int i = 0; i < zoneConfig.subZones.Count; i++)
    		{
				if (msg.zonename == zoneConfig.subZones[i].name)
				{
					zoneIndex = i;
					networkTransport.port = GetZonePort;
					autoConnectClient = true;
					Invoke(nameof(ReloadScene), 0.25f);
					
					debug.LogFormat(this.name, nameof(OnServerResponseZoneSwitchServer), i.ToString()); //DEBUG
					
					return;
				}
				
			}
			
			debug.LogFormat(this.name, nameof(OnServerResponseZoneSwitchServer), "NOT FOUND"); //DEBUG
		}
	}
}