//BY DX4D

using System.Collections;
using System;
using UnityEngine;
using OpenMMO.UI;

namespace OpenMMO.Zones
{
    [CreateAssetMenu(menuName = "OpenMMO/NetworkAction/Teleport", order = 1)]
    public class TeleportAction : BaseNetworkAction
    {
        //ON USED
        internal override void OnUsed(PlayerAccount pc)
        {
            pc.Cmd_WarpLocal(targetName);
            Debug.Log(pc.name + " teleported to " + targetName);
        }
    }
}
    /*
    // OnTriggerEnter
    // @Client / @Server
    // -------------------------------------------------------------------------------
    public override bool OnActivated(Collider co)
        {

            if (targetLocationMarker == null)
            {
                Debug.LogWarning("You must add a telaport marker to portal: " + this.name);
                return false;
            }

            PlayerAccount pc = co.GetComponentInParent<PlayerAccount>();

            if (pc == null || !pc.IsLocalPlayer)
                return false;

            if (!bypassConfirmation)
            {
                if (pc.CheckCooldown)
                    UIPopupPrompt.singleton.Init(String.Format(popupEnter, targetLocationMarker), OnClickConfirm);
                else
                    UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
            }
            else
                if (pc.CheckCooldown)
                OnClickConfirm();
            else
                UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));

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

            if (player != null && targetLocationMarker != null && pc.CheckCooldown)
                pc.Cmd_WarpLocal(targetLocationMarker);

            base.OnClickConfirm();

        }
    }
}*/