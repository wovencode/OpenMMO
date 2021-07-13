using System.Collections;
using System;
using UnityEngine;
using OpenMMO.UI;

namespace OpenMMO.Zones
{
    public abstract class NetworkAction : ScriptableObject
    {

        [Header("CONFIRMATION POPUP")]
        [Tooltip("Skip the confirmation popup and teleport right away?")]
        public bool bypassConfirmation;

        [Header("POPUP TEXT")]
        public string popupEnter;// = "Activate {0}?";
        public string popupWait;// = "Please wait {0} seconds!";
        public string popupClosed;// = "This action is unavailable.";

        public string infoEntered;// = "You activated {0}";

        //private string _targetName;
        public string targetName;
        //{
        //    get { return _targetName; }
        //    set { _targetName = value; }
        //}
        //VALIDATE USER
        /*public virtual bool ValidateUser(MobileComponent mob)
        {
            if (mob == null) return false; //ONLY TRIGGERED BY MOBILES
            if (!mob.IsLocalPlayer) return false; //ONLY TRIGGERED BY LOCAL CLIENTS

            return true; //PASSED ALL VALIDATION
        }*/

        //ACTIVATE
        // @Client / @Server
        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <returns>Trigger Success?</returns>
        public virtual bool EnterRange(Collider co)
        {
            return OnRangeEntered(co); //ACTIVATED
        }

        //ON RANGE ENTERED
        public virtual bool OnRangeEntered(Collider co)
        {
            PlayerAccount pc = co.GetComponentInParent<PlayerAccount>(); //GET PLAYER ACCOUNT COMPONENT
            return Use(pc);
            /*if (bypassConfirmation)
            {
                if (pc.CheckCooldown) return Use(pc);
                else UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
            }
            else
            {
                if (pc.CheckCooldown) UIPopupPrompt.singleton.Init(String.Format(popupEnter, targetLocationMarker), OnUsed);
                else UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
            }*/
        }

        //DEACTIVATE
        // @Client / @Server
        public virtual bool LeaveRange(Collider co)
        {
            OnRangeLeft(co); return true; //DEACTIVATED
        }
        public virtual void OnRangeLeft(Collider co)
        {
            UIPopupPrompt.singleton.Hide(); //HIDE POPUP
        }

        //USE @Client
        private bool Use(PlayerAccount pc)
        {
            if (CanUse(pc))
            {
                OnUsed(pc);
                return true; //USED
            }

            return false; //NOT USED
        }
        //CAN USE
        /// <summary>Checks if this action can be used</summary>
        /// <returns>Can this action be used?</returns>
        internal virtual bool CanUse(PlayerAccount pc)
        {
            if (!pc) return false; //NOBODY TO USE
            if (!pc.IsLocalPlayer) return false; //ONLY TRIGGERED BY LOCAL CLIENTS

            if (targetName == string.Empty) return false; //NO TARGET MARKER
            if (!bypassConfirmation)
            {
                UIPopupPrompt.singleton.Init(String.Format(popupEnter, targetName), OnConfirmed);
                return false; //NOTE: We return false here so that the action will not trigger twice //TODO: This can be better
            }
            if (!pc.CheckCooldown)
            {
                UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
                return false; //ON COOLDOWN
            }

            return true; //CAN BE USED
        }
        //ON CONFIRMED
        internal virtual void OnConfirmed()
        {
            GameObject player = PlayerAccount.localPlayer;
            if (player == null) return;

            PlayerAccount pc = player.GetComponent<PlayerAccount>();
            if (pc == null) return;

            OnUsed(pc);
        }
        //ON USED
        /// <summary>The OnUsed method is called once the action passes validation (CanUse)</summary>
        internal abstract void OnUsed(PlayerAccount pc);
    }
}