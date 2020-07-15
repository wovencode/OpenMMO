using UnityEngine;

namespace OpenMMO
{
    /// <summary>Player movement is determined by controller and keyboard inputs.</summary>
    [CreateAssetMenu(menuName = "OpenMMO/Controls/Movement Profile - AI", order = 0)]
    public partial class AIMovementProfile : MovementProfile
    {
        //MOVEMENT
        [Header("Character Movement State")]
        [Tooltip("The movement state of this object.")]
        [SerializeField] MovementState move = new MovementState();

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
        /// <summary>Updates the Movement Profile</summary>
        internal override void UpdateProfile()
        {
            //MOVE DIRECTION
            horizontalMovement = move.movingHorizontal;// Input.GetAxis(keys.moveAxisHorizontal.ToString());
            verticalMovement = move.movingVertical;// Input.GetAxis(keys.moveAxisVertical.ToString());

            //MOVE SPEED
            //run
            running = move.running;// Input.GetKey(keys.runKey);
            //sneak
            sneaking = move.sneaking;// Input.GetKey(keys.sneakKey);
        }
    }
}