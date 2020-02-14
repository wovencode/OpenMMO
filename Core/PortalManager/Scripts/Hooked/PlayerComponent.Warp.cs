
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
		
		protected int _token = 0;
		
		// -------------------------------------------------------------------------------
		// GetToken
		// Fetches the current token or generates a new one if its 0.
		// -------------------------------------------------------------------------------
		public int GetToken
		{
			get
			{
				if (_token == 0)
					RefreshToken();
				return _token;
			}
		}
		
		// -------------------------------------------------------------------------------
		// RefreshToken
		// Generates a new security token for server switch.
		// -------------------------------------------------------------------------------
		public void RefreshToken()
		{
			_token = UnityEngine.Random.Range(1000,9999);
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
		// Cmd_WarpRemote
		// Teleports to an anchor on another scene
		// @Client -> @Server
		// -------------------------------------------------------------------------------
		[Command]
		public void Cmd_WarpRemote(string anchorName, string zoneName)
		{
			if (!String.IsNullOrWhiteSpace(anchorName) && !String.IsNullOrWhiteSpace(zoneName))
			{
				// -- refresh security token
				RefreshToken();
				
				// -- issue warp
				WarpRemote(anchorName, zoneName, GetToken);
			}
		}
		
		// -------------------------------------------------------------------------------
		// WarpRemote
		// Warp the player to another network zone (only available to players)
		// @Server
		// -------------------------------------------------------------------------------
		[ServerCallback]
		public void WarpRemote(string anchorName, string zoneName, int token)
    	{
    		
    		NetworkZoneTemplate template = NetworkZoneTemplate.GetZoneBySceneName(zoneName);
    		
    		UpdateCooldown(20);
    		
    		// -- set security token
    		_token = token;
    		
    		// -- set anchor & zone
    		this.GetComponent<PlayerComponent>().tablePlayerZones.anchorname = anchorName;
    		this.GetComponent<PlayerComponent>().tablePlayerZones.zonename = zoneName;
    		
    		// -- save player
    		DatabaseManager.singleton.SaveDataPlayer(this.gameObject);
    		
    		OpenMMO.Network.NetworkManager.singleton.SwitchServerPlayer(this.connectionToClient, this.gameObject.name, anchorName, zoneName, _token);
    		
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