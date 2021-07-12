//BY DX4D

using UnityEngine;

namespace OpenMMO
{
    /// <summary>Player movement is determined by controller and keyboard inputs.</summary>
    [CreateAssetMenu(menuName = "OpenMMO/Controls/Movement Profile - Player", order = 1)]
    public partial class PlayerMovementProfile : MovementProfile
    {
        //CONTROLS
        [Header("Character Movement Keys")]
        [Tooltip("The Keys and Buttons used to control this object.")]
        public MovementKeys keys;

#if UNITY_EDITOR
        // LOAD PLUGINS
        /// <summary>Loads Default Components</summary>
        private void OnValidate()
        {
            if (!movement) movement = Resources.Load<MovementModifiers>("Player/Movement/_default/DefaultMovementModifiers"); //LOAD MOVEMENT MODS
            if (!keys) keys = Resources.Load<MovementKeys>("Player/Movement/_default/DefaultMovementKeys"); //LOAD DEFAULT MOVE KEYS
            if (!motor) motor = Resources.Load<CharacterMotor>("Player/Movement/_default/DefaultCharacterMotor"); //LOAD DEFAULT MOTOR
        }
#endif
        /// <summary>Updates the Movement Profile</summary>
        internal override void UpdateProfile()
        {
            //MOVE
            horizontalMovement = Input.GetAxis(keys.moveAxisHorizontal.ToString());
            verticalMovement = Input.GetAxis(keys.moveAxisVertical.ToString());
            //STRAFE
            strafeLeft = Input.GetKey(keys.strafeLeftKey);
            strafeRight = Input.GetKey(keys.strafeRightKey);
            //RUN
            running = Input.GetKey(keys.runKey);
            //SNEAK
            sneaking = Input.GetKey(keys.sneakKey);
            //JUMP
            jumping = Input.GetKey(keys.jumpKey);
        }
    }
}