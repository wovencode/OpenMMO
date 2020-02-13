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
		// OnStartServer_NetworkPortals
		// @Server
		// -----------------------------------------------------------------------------------
		[DevExtMethods("OnStartServer")]
		void OnStartServer_NetworkPortals()
		{
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerAutoLogin>(OnClientMessageRequestPlayerAutoLogin);
            
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
		
		// ======================== MESSAGE HANDLERS - PLAYER ============================
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerLogin
        // @Client -> @Server
		// -------------------------------------------------------------------------------     
        void OnClientMessageRequestPlayerAutoLogin(NetworkConnection conn, ClientMessageRequestPlayerAutoLogin msg)
		{
			
			ServerMessageResponsePlayerAutoLogin message = new ServerMessageResponsePlayerAutoLogin
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryPlayerLogin(msg.playername, msg.username))
			{
				LoginPlayer(conn, msg.username, msg.playername);
				message.text = systemText.playerLoginSuccess;
			}
			else
			{
				message.text = systemText.playerLoginFailure;
				message.success = false;
			}
					
			conn.Send(message);
			
		}
		
		
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================