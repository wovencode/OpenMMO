
using System;
using System.Text;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using Mirror;
//using OpenMMO;

namespace OpenMMO
{
    // ===================================================================================
    // PlayerMovement
    // ===================================================================================
    [DisallowMultipleComponent]
    [System.Serializable]
    public partial class PlayerMovementComponent : EntityMovementComponent
    {
        [Header("Player Control Config")]
        public PlayerControlConfig config;

        protected float verticalMovementInput;
        protected float horizontalMovementInput;
        protected bool running;

#if UNITY_EDITOR
        // -------------------------------------------------------------------------------
        // OnValidate
        // -------------------------------------------------------------------------------
        private void OnValidate()
        {
            if (!config) config = Resources.Load<PlayerControlConfig>("Config/DefaultPlayerControls"); //LOAD DEFAULT
        }
#endif
        // -------------------------------------------------------------------------------
        // Start
        // -------------------------------------------------------------------------------
        protected override void Start()
        {
            agent.updateRotation = false;
            base.Start();
        }

        // -------------------------------------------------------------------------------
        // 
        // -------------------------------------------------------------------------------
        public override void OnStartLocalPlayer()
        {
        }

        // -------------------------------------------------------------------------------
        // 
        // -------------------------------------------------------------------------------
        void OnDestroy()
        {
        }

        // -------------------------------------------------------------------------------
        // UpdateServer
        // @Server
        // -------------------------------------------------------------------------------
        [Server]
        protected override void UpdateServer()
        {
            base.UpdateServer();
            this.InvokeInstanceDevExtMethods(nameof(UpdateServer));
        }

        // -------------------------------------------------------------------------------
        // UpdateClient
        // @Client
        // -------------------------------------------------------------------------------
        [Client]
        protected override void UpdateClient()
        {

            if (!isLocalPlayer) return;
            if (Tools.AnyInputFocused) return;

            horizontalMovementInput = Input.GetAxis(config.moveAxisHorizontal.ToString());
            verticalMovementInput = Input.GetAxis(config.moveAxisVertical.ToString());
            running = Input.GetKey(config.runKey);

            UpdateVelocity();

            base.UpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(UpdateClient));

        }

        // -------------------------------------------------------------------------------
        // LateUpdateClient
        // @Client
        // -------------------------------------------------------------------------------
        protected override void LateUpdateClient()
        {
            base.LateUpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient));
        }

        // -------------------------------------------------------------------------------

    }

}

// =======================================================================================