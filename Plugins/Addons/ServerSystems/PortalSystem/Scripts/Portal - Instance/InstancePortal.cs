//BY DX4D

using UnityEngine;
using System.Collections.Generic;

namespace OpenMMO.Zones
{
    [DisallowMultipleComponent]
    public class InstancePortal : ZonePortal
    {
        public List<NetworkZoneTemplate> targetInstances = new List<NetworkZoneTemplate>();
        public Dictionary<uint, bool> instancesOccupied = new Dictionary<uint, bool>();
        //ENTER
        public override void OnTriggerEnter(Collider co)
        {
            base.OnTriggerEnter(co);
        }
        //EXIT
        public override void OnTriggerExit(Collider co)
        {
            base.OnTriggerExit(co);
        }
        //ON CONFIRMATION
        public override void OnClickConfirm()
        {
            base.OnClickConfirm();
        }
    }
	/*
		[Header("Teleportation")]
		[Tooltip("Target Network Zone to teleport to (optional)")]
		public NetworkZoneTemplate targetZone;
		[Tooltip("Anchor name in the target scene to teleport to")]
		public string targetAnchor;
		
		
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
			
				if (pc.CheckCooldown)
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
			
			PlayerAccount pc = player.GetComponentInParent<PlayerAccount>();
			
			if (player && targetZone != null && !String.IsNullOrWhiteSpace(targetAnchor) && pc.CheckCooldown)
				pc.WarpRemote(targetAnchor, targetZone.name);
			
			base.OnClickConfirm();
			
			if (UIPopupNotify.singleton)
				UIPopupNotify.singleton.Init(popupZoning, 5f, false);
			
		}
		*/
}