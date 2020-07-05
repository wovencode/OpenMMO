//BY DX4D
using UnityEngine;

namespace OpenMMO
{
	/// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "OpenMMO/Controls/Movement Modifiers")]
    public class MovementModifiers : ScriptableObject
    {
        [Header("Orientation")]
        [Tooltip("NOTE: This setting will respond in strange ways with certain cameras. Turn it off if you get strange movement results.")]
        public bool faceCameraDirection = false;
        [Tooltip("This basically overrides strafing. Turns strafe buttons into Turn buttons")]
        public bool turnWhileStrafing = false;

        /* //DEPRECIATED: MOVED TO MOVEMENT KEYS
        [Header("Input Keys")]
        public KeyCode runKey = KeyCode.LeftShift;

        //MOVE
        public InputAxis moveAxisHorizontal = InputAxis.Horizontal;
        public InputAxis moveAxisVertical = InputAxis.Vertical;

        //STRAFE
        public KeyCode strafeLeftKey = KeyCode.Q;
        public KeyCode strafeRightKey = KeyCode.E;
        */
        [Header("Move Speed")]
        [Tooltip("How fast you can move.")]
        [Range(0, 10)] public float moveSpeedMultiplier = 1.0f;
        [Tooltip("How fast you can turn.")]
        [Range(0, 10)] public float turnSpeedMultiplier = 0.8f;

        [Header("Move Speed Scale")]
        //WALK
        [Tooltip("Scales speed while walking. 1.0f = normal speed")]
        [Range(0, 10)] public float walkSpeedScale = 1.0f;
        //RUN
        [Tooltip("Scales speed while running. 1.0f = normal speed")]
        [Range(0, 10)] public float runSpeedScale = 1.5f;
        //STRAFE
        [Tooltip("Scales speed while strafing. 1.0f = normal speed")]
        [Range(0, 10)] public float strafeSpeedScale = 0.75f;
        //BACKPEDAL
        [Tooltip("Scales speed while backpedaling. 1.0f = normal speed")]
        [Range(0, 10)] public float backpedalSpeedScale = 0.5f;
    }
}