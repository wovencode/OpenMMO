
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
using OpenMMO.Zones;

namespace OpenMMO.Zones
{

	// ===================================================================================
	// MultiZonePortal
	// ===================================================================================
	[DisallowMultipleComponent]
	public class MultiZonePortal : BasePortal
	{
	
		[Header("Teleportation")]
		[Tooltip("Target Network Zone to teleport to (optional)")]
		public NetworkZoneTemplate targetZone;
		[Tooltip("Anchor name in the target scene to teleport to")]
		public string[] targetAnchors;
		
		public string popupZoning 	= "Zoning, please wait...";
		
		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public override void OnTriggerEnter(Collider co)
		{

			PlayerAccount pc = co.GetComponentInParent<PlayerAccount>();
			
			if (pc == null || !pc.IsLocalPlayer)
				return;
			
			// -- can we switch zones?
			if (!ZoneManager.singleton.GetCanSwitchZone)
			{
				UIPopupNotify.singleton.Init(popupClosed);
				return;
			}
			
			if (!bypassConfirmation)
			{
			
				if (pc.IsCooldownElapsed)
					UIPopupPrompt.singleton.Init(String.Format(popupEnter, targetZone.title), OnClickConfirm);
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
		
			GameObject player = PlayerAccount.localPlayer;
			
			if (player == null)
				return;
			
			PlayerAccount pc = player.GetComponent<PlayerAccount>();
			
			int index = UnityEngine.Random.Range(0, targetAnchors.Length);
			string targetAnchor = targetAnchors[index];
			
			if (player && targetZone != null && !String.IsNullOrWhiteSpace(targetAnchor) && pc.IsCooldownElapsed)
				pc.WarpRemote(targetAnchor, targetZone.name);
			
			base.OnClickConfirm();
			
			if (UIPopupNotify.singleton)
				UIPopupNotify.singleton.Init(popupZoning, 10f, false);
			
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================