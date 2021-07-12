//BY FHIZ
//BY DX4D

using UnityEngine;
using Mirror;

namespace OpenMMO
{
    public abstract partial class MovementProfile : ScriptableObject
    {
        //MODIFIERS
        [Header("Character Movement Modifiers")]
        public MovementModifiers movement;

        //MOTOR
        [Header("Character Motor")]
        [Tooltip("The Motor is the plugin responsible for updating the velocity of the attached agent.")]
        public CharacterMotor motor;

        //MOVE
        internal float verticalMovement;
        internal float horizontalMovement;

        //STRAFE
        internal bool strafeLeft;
        internal bool strafeRight;

        //RUN
        internal bool running;

        //SNEAK
        internal bool sneaking;

        //JUMP
        internal bool jumping;

#if UNITY_EDITOR
        // LOAD PLUGINS
        /// <summary>Loads Default Components</summary>
        private void OnValidate()
        {
            if (!movement) movement = Resources.Load<MovementModifiers>("Player/Movement/_default/DefaultMovementModifiers"); //LOAD MOVEMENT MODS
            //if (!keys) keys = Resources.Load<MovementKeys>("Player/Movement/_default/DefaultMovementKeys"); //LOAD DEFAULT MOVE KEYS
            if (!motor) motor = Resources.Load<CharacterMotor>("Player/Movement/_default/DefaultCharacterMotor"); //LOAD DEFAULT MOTOR
        }
#endif

        internal abstract void UpdateProfile();
    }
}