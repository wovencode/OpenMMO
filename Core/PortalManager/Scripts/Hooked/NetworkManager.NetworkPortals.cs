using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Portals;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace OpenMMO
{
	
	// ===================================================================================
	// NetworkManager
	// ===================================================================================
	[RequireComponent(typeof(PortalManager))]
	public partial class NetworkManager
	{
   
		// -------------------------------------------------------------------------------
		// 
		// @Server
		// -------------------------------------------------------------------------------
		[DevExtMethods("OnStartServer")]
		void OnStartServer_NetworkPortals()
		{
			PortalManager.singleton.SpawnSubZones();
		}
		
/*
		// -------------------------------------------------------------------------------
		// 
		// -------------------------------------------------------------------------------
		[DevExtMethods("OnClientCharactersAvailable")]
		void OnClientCharactersAvailable_NetworkPortals()
		{
				// ------- OBSOLETE -------
				
		
			int index = message.characters.ToList().FindIndex(c => c.name == UCE_NetworkZone.autoSelectCharacter);

			if (index != -1)
			{
				// send character select message
				print("[Zones]: autoselect " + UCE_NetworkZone.autoSelectCharacter + "(" + index + ")");

				byte[] extra = BitConverter.GetBytes(index);
				ClientScene.AddPlayer(NetworkClient.connection, extra);

				// clear auto select
				UCE_NetworkZone.autoSelectCharacter = "";
			}
		
		}
*/

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
		
		
			// ------- OBSOLETE -------
			
		/*
			// where was the player saved the last time?
			string lastScene = Database.singleton.GetCharacterScene(player.name);

			if (lastScene != "" && lastScene != SceneManager.GetActiveScene().name)
			{
				print("[Zones]: " + player.name + " was last saved on another scene, transferring to: " + lastScene);

				// ask client to switch server
				conn.Send(
					new SwitchServerMsg
					{
						scenePath = lastScene,
						characterName = player.name
					}
				);

				// immediately destroy so nothing messes with the new
				// position and so it's not saved again etc.
				NetworkServer.Destroy(player);
			}
		*/
		}

		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================