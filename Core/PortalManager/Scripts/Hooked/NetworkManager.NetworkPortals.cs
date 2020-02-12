using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Portals;
using OpenMMO.Database;
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
   		
   		// -----------------------------------------------------------------------------------
		// OnStartClient_NetworkPortals
		// @Client
		// -----------------------------------------------------------------------------------
		[DevExtMethods("OnStartClient")]
		void OnStartClient_NetworkPortals()
		{
			NetworkClient.RegisterHandler<ServerMessageResponsePlayerSwitchServer>(GetComponent<PortalManager>().OnServerMessageResponsePlayerSwitchServer, false);
		}
   		
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
		// LoginPlayer_NetworkPortals
		// @Server
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
		// SwitchServerPlayer
		// @Server -> @Client
		// -------------------------------------------------------------------------------
		public void SwitchServerPlayer(NetworkConnection conn, string playername, string anchorName, string zoneName)
		{

			ServerMessageResponsePlayerSwitchServer message = new ServerMessageResponsePlayerSwitchServer
			{
				playername			= playername,
				anchorname 			= anchorName,
				zonename 			= zoneName,
				success 			= true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerSwitchServer(playername))
			{
				message.text = systemText.playerSwitchServerSuccess;
			}
			else
			{
				message.text = systemText.playerSwitchServerFailure;
				message.success = false;
			}
			
        	conn.Send(message);
		
		}
		
		
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================