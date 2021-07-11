//BY FHIZ
//BY DX4D

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
        [Header("Movement Profile")]
        [SerializeField] MovementProfile input;

        private void OnValidate()
        {
            if (!input) input = Resources.Load<PlayerMovementProfile>("Player/Movement/_default/DefaultMovementProfile-Player"); //LOAD MOVEMENT PROFILE
        }
        //- - - - - - - -
        //MOVEMENT STATES
        //- - - - - - - -
        // MovementStates represent the various movement related states of a character.
        // Note that certain states like Stealth can be active at the same time as other states like Running.

        //IDLE?
        /// <summary>Checks if the character is standing still</summary>
        public bool Idle { get { return !Moving; } }
        //MOVING?
        /// <summary>Checks if the character is moving</summary>
        public bool Moving { get { return agent.velocity != Vector3.zero; } }
        //WALKING?
        /// <summary>Checks if the character is walking</summary>
        public bool Walking { get { return Moving && !input.running; } }
        //RUNNING?
        /// <summary>Checks if the character is running</summary>
        public bool Running { get { return Moving && input.running; } }
        //SNEAKING?
        /// <summary>Checks if the character is moving stealthily</summary>
        public bool Sneaking { get { return input.sneaking; } }


        ///// <summary> Start called client and server-side to initialize properties. </summary>
        //protected override void Start() { agent.updateRotation = false; base.Start(); }

        //CLIENT
        /// <summary> [Client] start. </summary>
        [Client] protected override void StartClient() {agent.updateRotation = false; base.StartClient(); }
        //SERVER
        /// <summary> [Server] start. </summary>
        [Server] protected override void StartServer() {agent.updateRotation = false; base.StartServer(); }
        
        /// <summary> OnStartLocalPlayer. Not used yet. </summary>
        public override void OnStartLocalPlayer()
        {
        }

        /// <summary>
    	/// OnDestroy client and side-server. Not used yet.
    	/// </summary>
        void OnDestroy()
        {
        }

        /// <summary> [Server] throttled update. </summary>
        [Server] protected override void UpdateServer()
        {
            base.UpdateServer();
            this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK
        }

        /// <summary> [Client] throttled update. </summary>
        [Client] protected override void UpdateClient()
        {

            if (!isLocalPlayer) return;
            if (Tools.AnyInputFocused) return;

            //UPDATE INPUTS - Checks current inputs and updates the attached Movement Profile
            input.UpdateProfile();

            //UPDATE VELOCITY
            agent.velocity = input.motor.GetVelocity(
                new MovementInput(transform.position, transform.rotation, input), input.movement, agent
                //data.verticalMovement, data.horizontalMovement,
                //data.running, data.sneaking,
                //data.strafeLeft, data.strafeRight),
                //data.movement, agent
                );

            base.UpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK

        }

        /// <summary>
    	/// Client only, late update.
    	/// </summary>
        [Client] protected override void LateUpdateClient()
        {
            base.LateUpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient)); //HOOK
        }
    }
}
