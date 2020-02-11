using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Portals;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// NetworkManager
	// ===================================================================================
	[RequireComponent(typeof(PortalManager))]
	public partial class NetworkManager
	{
   
		// -------------------------------------------------------------------------------
		// OnStartServer_NetworkPortals
		// @Server
		// -------------------------------------------------------------------------------
		[DevExtMethods("OnStartServer")]
		void OnStartServer_NetworkPortals()
		{
			GetComponent<PortalManager>().SpawnSubZones();
		}

		// -------------------------------------------------------------------------------
		// OnServerAddPlayer_NetworkPortals
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoginPlayer")]
		void LoginPlayer_NetworkPortals(NetworkConnection conn, GameObject player, GameObject prefab, string userName, string playerName)
		{
			
			PlayerComponent pc = player.GetComponent<PlayerComponent>();
			string zoneName = pc.tablePlayerZones.zonename;
			NetworkZoneTemplate currentZone = pc.currentZone;
			
			if (!String.IsNullOrWhiteSpace(zoneName) && zoneName != currentZone.name)
			{
				string anchorName = pc.tablePlayerZones.anchorname;
				pc.WarpRemote(anchorName, zoneName);
			}
		
		}

		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================