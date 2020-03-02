
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
	// MultiWarpPortal
	// ===================================================================================
	[DisallowMultipleComponent]
	public class MultiWarpPortal : BasePortal
	{
	
		[Header("Teleportation")]
		[Tooltip("Anchor in the same scene to teleport to")]
		public string[] targetAnchors;
		
		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public override void OnTriggerEnter(Collider co)
		{
			
			if (targetAnchors == null || targetAnchors.Length == 0)
			{
				DebugManager.LogWarning("You forgot to add anchors to MultiWarpPortal: "+this.name);
				return;
			}
			
			PlayerComponent pc = co.GetComponentInParent<PlayerComponent>();
			
			if (pc == null || !pc.IsLocalPlayer)
				return;
			
			if (!triggerOnEnter)
			{
				if (pc.CheckCooldown)
					UIPopupPrompt.singleton.Init(popupEnter, OnClickConfirm);
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
			
			int index = UnityEngine.Random.Range(0, targetAnchors.Length);
			string targetAnchor = targetAnchors[index];

			if (player != null && !String.IsNullOrWhiteSpace(targetAnchor) && pc.CheckCooldown)
				pc.Cmd_WarpLocal(targetAnchor);
			
			base.OnClickConfirm();
			
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================