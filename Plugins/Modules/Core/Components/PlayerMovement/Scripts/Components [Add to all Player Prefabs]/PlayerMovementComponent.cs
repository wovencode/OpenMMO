// by Fhiz, DXD4
using UnityEngine;
using Mirror;

namespace OpenMMO
{

    /// <summary>
    /// 
    /// </summary>
    [DisallowMultipleComponent]
    [System.Serializable]
    public partial class PlayerMovementComponent : EntityMovementComponent
    {
        [Header("Player Movement Config")]
        public PlayerControlConfig movementConfig;

        //MOVE
        protected float verticalMovementInput;
        protected float horizontalMovementInput;

        //TURN
        protected bool strafeLeft;
        protected bool strafeRight;

        //RUN
        protected bool running;
		
		/// <summary>
    	/// Lodas default OnValidate from PlayerControlConfig in Editor only.
    	/// </summary>
#if UNITY_EDITOR
        // LOAD DEFAULTS
        private void OnValidate()
        {
            if (!movementConfig) movementConfig = Resources.Load<PlayerControlConfig>("Player/Movement/DefaultPlayerControls"); //LOAD DEFAULT
        }
#endif

        /// <summary>
    	/// Start called client and server-side to initialize properties.
    	/// </summary>
        protected override void Start()
        {
            agent.updateRotation = false;
            base.Start();
        }

        /// <summary>
    	/// OnStartLocalPlayer. Not used yet.
    	/// </summary>
        public override void OnStartLocalPlayer()
        {
        }

        /// <summary>
    	/// OnDestroy client and side-server. Not used yet.
    	/// </summary>
        void OnDestroy()
        {
        }

        /// <summary>
    	/// Server-side throttled update.
    	/// </summary>
        [Server]
        protected override void UpdateServer()
        {
            base.UpdateServer();
            this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK
        }

        /// <summary>
    	/// Client-side, throttled update.
    	/// </summary>
        [Client]
        protected override void UpdateClient()
        {

            if (!isLocalPlayer) return;
            if (Tools.AnyInputFocused) return;

            //MOVE
            horizontalMovementInput = Input.GetAxis(movementConfig.moveAxisHorizontal.ToString());
            verticalMovementInput = Input.GetAxis(movementConfig.moveAxisVertical.ToString());
            //STRAFE
            strafeLeft = Input.GetKey(movementConfig.strafeLeftKey);
            strafeRight = Input.GetKey(movementConfig.strafeRightKey);
            //RUN
            running = Input.GetKey(movementConfig.runKey);

            UpdateVelocity(); //UPDATE VELOCITY

            base.UpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK

        }

        /// <summary>
    	/// Client only, late update.
    	/// </summary>
        protected override void LateUpdateClient()
        {
            base.LateUpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient)); //HOOK
        }

    }

}