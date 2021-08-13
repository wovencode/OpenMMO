//BY FHIZ

using System;
using OpenMMO;

using UnityEngine;
using Mirror;

namespace OpenMMO
{

    /// <summary>
    /// This partial section of the player component takes care of the global cooldown (used to limit 'Risky Actions')
    /// </summary>
    public partial class PlayerAccount
    {

        [Header("RISKY ACTION COOLDOWN")]
        [Tooltip("Base cooldown for 'risky actions' (in seconds)")]
        [Range(1, 999)] public double baseCooldown;

        // a global player cooldown to handle "risky actions"
        [SyncVar] double cooldown = 0;


        /// <summary>Returns the remaining cooldown (until another Risky Action is allowed).</summary>
        public float GetCooldownTimeRemaining()
        {
            return IsCooldownElapsed ? 0 
                : (float)(cooldown - NetworkTime.time);
        }
        /// <summary></summary>
        public bool IsCooldownElapsed { get { return NetworkTime.time >= cooldown; } }

        /// <summary></summary>
        /// <remarks>Sent from CLIENT to SERVER.</remarks>
        [Command]
        public void Cmd_UpdateCooldown(float extraCooldown)
        {
            UpdateCooldown(extraCooldown);
        }

        /// <summary>Updates the cooldown timer on the server.</summary>
        /// <remarks>Sent from CLIENT to SERVER.</remarks>
        [Server]
        protected void UpdateCooldown(float extraCooldown)
        {
            SetCooldown(extraCooldown);
        }
        [Server]
        void SetCooldown(float extraCooldown)
        {
#if _SERVER
            cooldown = NetworkTime.time + (float)(baseCooldown + Mathf.Abs(extraCooldown));
            _tablePlayer.cooldown = GetCooldownTimeRemaining();
#endif
        }
    }
}
