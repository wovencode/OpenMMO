
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Portals;
using OpenMMO.DebugManager;

namespace OpenMMO {
	
	// ===================================================================================
	// PlayerComponent
	// ===================================================================================
	public partial class PlayerComponent
	{
		
		[Header("NetworkZones")]
		public NetworkZoneTemplate startingZone;
		
		protected int securityToken = 0;
		
		// -------------------------------------------------------------------------------
		// GetToken
		// Fetches the current token
		// -------------------------------------------------------------------------------
		public int GetToken
		{
			get
			{
				return securityToken;
			}
		}
		
		// -------------------------------------------------------------------------------
		// currentZone
		// no need to cache the current zone, we can fetch it via the active scene name
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public NetworkZoneTemplate currentZone
		{
			get
			{
				if (SceneManager.GetActiveScene() != null)
					return NetworkZoneTemplate.GetZoneBySceneName(SceneManager.GetActiveScene().name);
				else
					return startingZone;
			}
		}
		
		// -------------------------------------------------------------------------------
		// Cmd_WarpLocal
		// Teleports to an anchor on the same scene
		// @Client -> @Server
		// -------------------------------------------------------------------------------
		[Command]
		public void Cmd_WarpLocal(string anchorName)
		{
			if (PortalManager.CheckPortalAnchor(anchorName))
				WarpLocal(anchorName);
		}
		
		// -------------------------------------------------------------------------------
		// WarpRemote
		// Public
		// @Client
		// -------------------------------------------------------------------------------
		[Client]
		public void WarpRemote(string anchorName, string zoneName)
		{
			// -- refresh security token
			PortalManager.singleton.RefreshToken();
			Cmd_WarpRemote(anchorName, zoneName, PortalManager.singleton.GetToken);
		}
		
		// -------------------------------------------------------------------------------
		// Cmd_WarpRemote
		// Teleports to an anchor on another scene
		// @Client -> @Server
		// -------------------------------------------------------------------------------
		[Command]
		protected void Cmd_WarpRemote(string anchorName, string zoneName, int token)
		{
			if (!String.IsNullOrWhiteSpace(anchorName) && !String.IsNullOrWhiteSpace(zoneName))
			{
				WarpRemote(anchorName, zoneName, token);
			}
		}
		
		// -------------------------------------------------------------------------------
		// WarpRemote
		// Warp the player to another network zone (only available to players)
		// @Server
		// -------------------------------------------------------------------------------
		[ServerCallback]
		public void WarpRemote(string anchorName, string zoneName, int token=0)
    	{
    		
    		NetworkZoneTemplate template = NetworkZoneTemplate.GetZoneBySceneName(zoneName);
    		
    		UpdateCooldown(20);
    		
    		// -- set anchor & zone
    		this.GetComponent<PlayerComponent>().tablePlayerZones.anchorname = anchorName;
    		this.GetComponent<PlayerComponent>().tablePlayerZones.zonename = zoneName;
    		securityToken = token; // token must not be set in table, can be fetched via GetToken
    		
    		debug.Log("SERVER TOKEN IS: " + securityToken);
    		
    		// -- save player
    		DatabaseManager.singleton.SaveDataPlayer(this.gameObject);
    		
    		OpenMMO.Network.NetworkManager.singleton.SwitchServerPlayer(this.connectionToClient, this.gameObject.name, anchorName, zoneName, securityToken);
    		
    		NetworkServer.Destroy(this.gameObject);
    		
		}
		
		// -------------------------------------------------------------------------------
		// WarpLocal
		// Warp the player to the anchors position on the same scene
		// @Server
		// -------------------------------------------------------------------------------
		[ServerCallback]
		public void WarpLocal(string anchorName)
    	{

    		if (PortalManager.CheckPortalAnchor(anchorName))
        		base.Warp(PortalManager.GetPortalAnchorPosition(anchorName));
        	
        	UpdateCooldown(20);
        	
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================