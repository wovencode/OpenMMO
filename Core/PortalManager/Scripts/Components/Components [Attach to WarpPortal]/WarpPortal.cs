
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.UI;
using OpenMMO.DebugManager;

namespace OpenMMO.Portals
{

	// ===================================================================================
	// WarpPortal
	// ===================================================================================
	[DisallowMultipleComponent]
	public class WarpPortal : BasePortal
	{
	
		[Header("Teleportation")]
		[Tooltip("Anchor in the same scene to teleport to")]
		public PortalAnchor targetAnchor;
		
		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public override void OnTriggerEnter(Collider co)
		{
			
			if (targetAnchor == null)
			{
				debug.LogWarning("You forgot to set an anchor to WarpPortal: "+this.name);
				return;
			}
			
			GameObject player = PlayerComponent.localPlayer;
			PlayerComponent pc = player.GetComponent<PlayerComponent>();
			
			if (player && !triggerOnEnter)
			{
				if (pc.CheckCooldown)
					UIPopupPrompt.singleton.Init(String.Format(popupEnter, targetAnchor.name), OnClickConfirm);
				else
					UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
			}
			else if (player)
				OnClickConfirm();
				
		}
		
		// -------------------------------------------------------------------------------
		// OnClickConfirm
		// @Client
		// -------------------------------------------------------------------------------
		public override void OnClickConfirm()
		{
		
			GameObject player = PlayerComponent.localPlayer;
						
			if (player && targetAnchor != null)
				player.GetComponent<PlayerComponent>().Cmd_WarpLocal(targetAnchor.name);
				
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================