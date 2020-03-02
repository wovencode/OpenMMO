
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
using OpenMMO.Debugging;

namespace OpenMMO.Zones
{

	// ===================================================================================
	// WarpPortal
	// ===================================================================================
	[DisallowMultipleComponent]
	public class WarpPortal : BasePortal
	{
	
		[Header("Teleportation")]
		[Tooltip("Anchor in the same scene to teleport to")]
		public string targetAnchor;
		
		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public override void OnTriggerEnter(Collider co)
		{
			
			if (targetAnchor == null)
			{
				DebugManager.LogWarning("You forgot to set an anchor to WarpPortal: "+this.name);
				return;
			}
			
			PlayerComponent pc = co.GetComponentInParent<PlayerComponent>();
			
			if (pc == null || !pc.IsLocalPlayer)
				return;
			
			if (!triggerOnEnter)
			{
				if (pc.CheckCooldown)
					UIPopupPrompt.singleton.Init(String.Format(popupEnter, targetAnchor), OnClickConfirm);
				else
					UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
			}
			else
				OnClickConfirm();
			
		}
		
		// -------------------------------------------------------------------------------
		// OnClickConfirm
		// @Client
		// -------------------------------------------------------------------------------
		public override void OnClickConfirm()
		{
		
			GameObject player = PlayerComponent.localPlayer;
			
			if (player == null)
				return;
			
			PlayerComponent pc = player.GetComponent<PlayerComponent>();
			
			if (player != null && targetAnchor != null && pc.CheckCooldown)
				pc.Cmd_WarpLocal(targetAnchor);
			
			base.OnClickConfirm();
			
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================