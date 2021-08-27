//BY FHIZ
//MODIFIED BY DX4D

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;

using System;
using UnityEngine;
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
                DebugManager.LogWarning("You must set an anchor to WarpPortal: " + this.name);
                return;
            }

            PlayerAccount pc = co.GetComponentInParent<PlayerAccount>();

            if (pc == null || !pc.IsLocalPlayer) { return; }

            if (!bypassConfirmation)
            {
                if (pc.IsCooldownElapsed)
                {
                    UIPopupPrompt.singleton.Init(String.Format(popupEnter, targetAnchor), OnClickConfirm);
                }
                else
                {
                    UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
                }
            }
            else
            {
                if (pc.IsCooldownElapsed)
                {
                    OnClickConfirm();
                }
                else
                {
                    UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
                }
            }
        }

        // -------------------------------------------------------------------------------
        // OnClickConfirm
        //@CLIENT
        public override void OnClickConfirm()
        {

            GameObject player = PlayerAccount.localPlayer;

            if (player == null) return;

            PlayerAccount pc = player.GetComponent<PlayerAccount>();

            if (player != null && targetAnchor != null && pc.IsCooldownElapsed)
            {
                pc.Cmd_WarpLocal(targetAnchor);
            }

            base.OnClickConfirm();
        }
    }
}
