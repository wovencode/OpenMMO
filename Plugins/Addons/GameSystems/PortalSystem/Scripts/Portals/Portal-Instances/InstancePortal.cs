//BY DX4D
//TODO: UNFINISHED - 

using System;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO.UI;

namespace OpenMMO.Zones
{
    [DisallowMultipleComponent]
    public class InstancePortal : ActionTrigger
    {
        [Header("Teleportation")]
        [Tooltip("Target Network Zone to teleport to (optional)")]
        public NetworkZoneTemplate targetZone;
        [Tooltip("Anchor name in the target scene to teleport to")]
        public string targetAnchor;

        public string zoningMessagePopup = "Entering Instance, please wait...";

        public List<NetworkZoneTemplate> targetInstances = new List<NetworkZoneTemplate>();
        private Dictionary<uint, bool> _instancesOccupied = new Dictionary<uint, bool>();

        //ENTER
        public override void OnTriggerEnter(Collider co)
        {
            PlayerAccount pc = co.GetComponentInParent<PlayerAccount>();

            if (pc == null || !pc.IsLocalPlayer) return; //NO LOCAL PLAYER

            // -- can we switch zones?
            if (!ZoneManager.singleton.GetCanSwitchZone)
            {
                UIPopupNotify.singleton.Init(action.popupClosed);
                return;
            }

            if (!action.bypassConfirmation)
            {
                if (pc.CheckCooldown) UIPopupPrompt.singleton.Init(String.Format(action.popupEnter, targetZone.title), OnClickConfirm);
                else UIPopupNotify.singleton.Init(String.Format(action.popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
            }
            else
            {
                OnClickConfirm();
            }
            //base.OnTriggerEnter(co);
        }
        //EXIT
        public override void OnTriggerExit(Collider co)
        {
            base.OnTriggerExit(co);
        }
        //ON CONFIRMATION
        public override void OnClickConfirm()
        {
            GameObject player = PlayerAccount.localPlayer;

            if (player == null) return; //NO LOCAL PLAYER

            PlayerAccount pc = player.GetComponentInParent<PlayerAccount>();

            if (player && targetZone != null && !String.IsNullOrWhiteSpace(targetAnchor) && pc.CheckCooldown)
            {
                pc.WarpRemote(targetAnchor, targetZone.name);
            }

            base.OnClickConfirm();

            if (UIPopupNotify.singleton)
            {
                UIPopupNotify.singleton.Init(zoningMessagePopup, 5f, false);
            }
            //base.OnClickConfirm();
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