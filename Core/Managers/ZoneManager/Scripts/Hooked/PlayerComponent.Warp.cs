
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Zones;
using OpenMMO.Debugging;

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
			ZoneManager.singleton.RefreshToken();
			Cmd_WarpRemote(anchorName, zoneName, ZoneManager.singleton.GetToken);
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
				WarpRemote(anchorName, zoneName, token);
		}
		
		// -------------------------------------------------------------------------------
		// WarpRemote
		// Warp the player to another network zone (only available to players)
		// @Server
		// -------------------------------------------------------------------------------
		[ServerCallback]
		public void WarpRemote(string anchorName, string zoneName, int token=0)
    	{
    		
    		UpdateCooldown(GameRulesTemplate.singleton.remoteWarpDelay);
    		
    		NetworkZoneTemplate template = NetworkZoneTemplate.GetZoneBySceneName(zoneName);
    		
    		// -- update anchor & zone
    		this.GetComponent<PlayerComponent>().tablePlayerZones.anchorname 	= anchorName;
    		this.GetComponent<PlayerComponent>().tablePlayerZones.zonename	 	= zoneName;
    		securityToken = token; // token must not be set in table, can be fetched via GetToken
    		
    		// -- save player
    		DatabaseManager.singleton.SaveDataPlayer(this.gameObject);
    		
    		OpenMMO.Network.NetworkManager.singleton.SwitchServerPlayer(this.connectionToClient, this.gameObject.name, anchorName, zoneName, securityToken);
    		
    		NetworkServer.Destroy(this.gameObject);
    		
    		DebugManager.LogFormat(this.name, nameof(WarpRemote), zoneName, anchorName); //DEBUG
    		
		}
		
		// -------------------------------------------------------------------------------
		// WarpLocal
		// Warp the player to the anchors position on the same scene
		// @Server
		// -------------------------------------------------------------------------------
		[ServerCallback]
		public void WarpLocal(string anchorName)
    	{
			
			UpdateCooldown(GameRulesTemplate.singleton.localWarpDelay);
			
    		if (AnchorManager.singleton.CheckPortalAnchor(anchorName))
        		base.Warp(AnchorManager.singleton.GetPortalAnchorPosition(anchorName));
        	
        	if (AnchorManager.singleton.CheckStartAnchor(anchorName))
        		base.Warp(AnchorManager.singleton.GetStartAnchorPosition(anchorName));
        	
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================