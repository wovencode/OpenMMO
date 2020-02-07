
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
	// MultiWarpPortal
	// ===================================================================================
	[DisallowMultipleComponent]
	public class MultiWarpPortal : BasePortal
	{
	
		[Header("Teleportation")]
		[Tooltip("Anchor in the same scene to teleport to")]
		public PortalAnchor[] targetAnchors;
		
		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public override void OnTriggerEnter(Collider co)
		{
			
			if (targetAnchors == null || targetAnchors.Length == 0)
			{
				debug.LogWarning("You forgot to add anchors to MultiWarpPortal: "+this.name);
				return;
			}
			
			GameObject player = PlayerComponent.localPlayer;
			
			if (player)
			{
			
				PlayerComponent pc = player.GetComponent<PlayerComponent>();
			
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
		}
		
		// -------------------------------------------------------------------------------
		// OnClickConfirm
		// @Client
		// -------------------------------------------------------------------------------
		public override void OnClickConfirm()
		{
			
			GameObject player = PlayerComponent.localPlayer;
			
			int index = UnityEngine.Random.Range(0, targetAnchors.Length);
			string targetAnchor = targetAnchors[index].name;

			if (player != null && !String.IsNullOrWhiteSpace(targetAnchor))
				player.GetComponent<PlayerComponent>().Cmd_WarpLocal(targetAnchor);
			
			base.OnClickConfirm();
			
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================