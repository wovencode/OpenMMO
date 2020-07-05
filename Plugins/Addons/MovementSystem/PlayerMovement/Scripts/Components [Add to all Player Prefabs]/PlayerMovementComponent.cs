// by Fhiz, DX4D
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
        //
        [Header("Character Movement Modifiers")]
        public MovementModifiers movement;

        //CONTROLS
        [Header("Character Movement Keys")]
        [Tooltip("The Keys and Buttons used to control this object.")]
        public MovementKeys keys;

        //MOTOR
        [Header("Character Motor")]
        [Tooltip("The Motor is the plugin responsible for updating the velocity of the attached agent.")]
        public CharacterMotor motor;

        //MOVE
        protected float verticalMovementInput;
        protected float horizontalMovementInput;

        //TURN
        protected bool strafeLeft;
        protected bool strafeRight;

        //RUN
        protected bool running;
		
#if UNITY_EDITOR
        // LOAD PLUGINS
		/// <summary>
    	/// Loads default OnValidate from PlayerControlConfig in Editor only.
    	/// </summary>
        private void OnValidate()
        {
            if (!movement) movement = Resources.Load<MovementModifiers>("Player/Movement/DefaultMovementModifiers"); //LOAD MOVEMENT MODS
            if (!keys) keys = Resources.Load<MovementKeys>("Player/Movement/DefaultMovementKeys"); //LOAD DEFAULT MOVE KEYS
            if (!motor) motor = Resources.Load<CharacterMotor>("Player/Movement/DefaultCharacterMotor"); //LOAD DEFAULT MOTOR
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
            horizontalMovementInput = Input.GetAxis(keys.moveAxisHorizontal.ToString());
            verticalMovementInput = Input.GetAxis(keys.moveAxisVertical.ToString());
            //STRAFE
            strafeLeft = Input.GetKey(keys.strafeLeftKey);
            strafeRight = Input.GetKey(keys.strafeRightKey);
            //RUN
            running = Input.GetKey(keys.runKey);

            //UPDATE VELOCITY
            agent.velocity = motor.GetVelocity(
                new MovementStateInfo(transform.position, transform.rotation, verticalMovementInput, horizontalMovementInput, running, strafeLeft, strafeRight) 
                , movement
                , agent
                );

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