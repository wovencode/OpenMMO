//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Zones;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
	[RequireComponent(typeof(ZoneManager))]
	public partial class NetworkManager
	{
   		// -----------------------------------------------------------------------------------
		// @Server
		// -----------------------------------------------------------------------------------
		[DevExtMethods(nameof(OnStartServer))]
		void OnStartServer_NetworkPortals()
		{
			
			NetworkServer.RegisterHandler<ClientRequestZoneSwitchServer>(OnClientRequestZoneSwitchServer);
            NetworkServer.RegisterHandler<ClientRequestZoneAutoLogin>(OnClientRequestZoneAutoLogin);
            
            if (GetComponent<ZoneManager>() != null)
   				GetComponent<ZoneManager>().SpawnSubZones();
		}
	}
}