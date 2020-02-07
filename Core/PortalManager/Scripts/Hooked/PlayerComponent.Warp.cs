
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Portals;

namespace OpenMMO {
	
	// ===================================================================================
	// PlayerComponent
	// ===================================================================================
	public partial class PlayerComponent
	{
		
		[Header("NetworkZones")]
		public NetworkZoneTemplate startingZone;
		
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
		// Cmd_WarpRemote
		// Teleports to an anchor on another scene
		// @Client -> @Server
		// -------------------------------------------------------------------------------
		[Command]
		public void Cmd_WarpRemote(string anchorName, string zoneName)
		{
			if (!String.IsNullOrWhiteSpace(anchorName) && !String.IsNullOrWhiteSpace(zoneName))
				WarpRemote(anchorName, zoneName);
		}
		
		// -------------------------------------------------------------------------------
		// WarpRemote
		// Warp the player to another network zone (only available to players)
		// @Server
		// -------------------------------------------------------------------------------
		[ServerCallback]
		public void WarpRemote(string anchorName, string zoneName)
    	{
    		
    		NetworkZoneTemplate template = NetworkZoneTemplate.GetZoneBySceneName(zoneName);
    		
    		UpdateCooldown(20);
    		
    		DatabaseManager.singleton.SaveDataPlayer(this.gameObject);
    		
			// -- uses OpenMMO NetworkManager singleton instead of the Mirror one
    		OpenMMO.Network.NetworkManager.singleton.TrySwitchServerPlayer(this.gameObject.name, anchorName, zoneName);
    		/*
    		TODO, NOTE, BUG
    		wrong, thats client side, must be server side
    		also we must save anchorName and zoneName here
    		*/
    		
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