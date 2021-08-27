//By Fhiz
//MODIFIED BY DX4D
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Zones;
using OpenMMO.Debugging;

namespace OpenMMO {
	
	// This partial section of PlayerComponent Manages Zone Travel
	public partial class PlayerAccount
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
        //@CLIENT
		[Client]
		public void WarpRemote(string _anchorname, string _zonename)
		{
			// -- refresh security token
			ZoneManager.singleton.RefreshToken();


			Cmd_WarpRemote(_anchorname, _zonename, ZoneManager.singleton.GetToken);
		}
		
		// -------------------------------------------------------------------------------
		// Cmd_WarpRemote
		// Teleports to an anchor on another scene
		// @Client -> @Server
		// -------------------------------------------------------------------------------
		[Command]
		protected void Cmd_WarpRemote(string _anchorname, string _zonename, int _token)
		{
			if (!String.IsNullOrWhiteSpace(_anchorname) && !String.IsNullOrWhiteSpace(_zonename))
				WarpRemote(_anchorname, _zonename, _token);
		}
		
		// -------------------------------------------------------------------------------
		// WarpRemote
		// Warp the player to another network zone (only available to players)
        //@SERVER
		[ServerCallback]
		public void WarpRemote(string _anchorname, string _zonename, int _token)
        {
            PlayerAccount player = this;// GetComponent<PlayerAccount>();

            Debug.Log("[ZONE SERVER] - Warping " + player.name + " to zone " + _zonename + " with token #" + _token + "..."); //DEBUG

            UpdateCooldown(GameRulesTemplate.singleton.remoteWarpDelay);
    		
    		//NetworkZoneTemplate template = NetworkZoneTemplate.GetZoneBySceneName(_zonename); //REMOVED - was not used - DX4D

    		// -- update anchor & zone
    		player.zoneInfo.anchorname 	= _anchorname;
    		player.zoneInfo.zonename	 	= _zonename;
            player.securityToken = _token; //ADDED DX4D //REMOVED DX4D

    		//securityToken = _token; // token must not be set in table, can be fetched via GetToken
    		
    		// -- save player
    		DatabaseManager.singleton.SaveDataPlayer(player.gameObject); //SAVE PLAYER
    		
    		Network.NetworkManager.singleton.SwitchServerPlayer(player.connectionToClient, player.gameObject.name, _anchorname, _zonename, _token); //SWITCH SERVER
    		
    		NetworkServer.Destroy(player.gameObject); //DESTROY PLAYER
    		
    		//DebugManager.LogFormat(this.name, nameof(WarpRemote), zoneName, anchorName); //DEBUG //REMOVED DX4D
    		
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

            //PORTAL ANCHOR
            if (AnchorManager.singleton.CheckPortalAnchor(anchorName))
            {
                base.Warp(AnchorManager.singleton.GetPortalAnchorPosition(anchorName));
                return; //ADDED DX4D
            }
            //START ANCHOR
            if (AnchorManager.singleton.CheckStartAnchor(anchorName))
            {
                base.Warp(AnchorManager.singleton.GetStartAnchorPosition(anchorName));
                return; //ADDED DX4D
            }
		}
	}
}
