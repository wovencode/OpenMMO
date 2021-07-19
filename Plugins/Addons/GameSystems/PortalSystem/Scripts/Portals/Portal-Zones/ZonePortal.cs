//BY FHIZ

using System;
using UnityEngine;
using OpenMMO.UI;

namespace OpenMMO.Zones
{

	// ===================================================================================
	// ZonePortal
	// ===================================================================================
	[DisallowMultipleComponent]
	public class ZonePortal : ActionTrigger
	{
	
		[Header("Teleportation")]
		[Tooltip("Target Network Zone to teleport to (optional)")]
		public NetworkZoneTemplate targetZone;
		[Tooltip("Anchor name in the target scene to teleport to")]
		public string targetAnchor;
		
		
		public string zoningMessagePopup = "Zoning, please wait...";
		
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
				UIPopupNotify.singleton.Init(action.popupClosed);
				return;
			}

            if (!action.bypassConfirmation)
            {
                if (pc.CheckCooldown)
                    UIPopupPrompt.singleton.Init(String.Format(action.popupEnter, targetZone.title), OnClickConfirm);
                else
                    UIPopupNotify.singleton.Init(String.Format(action.popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
            }
            else
            {
                OnClickConfirm();
            }
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
			
			PlayerAccount pc = player.GetComponentInParent<PlayerAccount>();
			
			if (player && targetZone != null && !String.IsNullOrWhiteSpace(targetAnchor) && pc.CheckCooldown)
				pc.WarpRemote(targetAnchor, targetZone.name);
			
			base.OnClickConfirm();
			
			if (UIPopupNotify.singleton)
				UIPopupNotify.singleton.Init(zoningMessagePopup, 5f, false);
			
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================