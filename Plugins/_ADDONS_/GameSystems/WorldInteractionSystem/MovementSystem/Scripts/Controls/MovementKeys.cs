//BY DX4D

using UnityEngine;

namespace OpenMMO
{
	/// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "OpenMMO/Controls/Movement Keys")]
    public class MovementKeys : ScriptableObject
    {
        [Header("Input Keys")]
        public KeyCode runKey = KeyCode.LeftShift;
        public KeyCode sneakKey = KeyCode.LeftControl;

        //JUMP
        public KeyCode jumpKey = KeyCode.Space;

        //MOVE
        public InputAxis moveAxisHorizontal = InputAxis.Horizontal;
        public InputAxis moveAxisVertical = InputAxis.Vertical;

        //STRAFE
        public KeyCode strafeLeftKey = KeyCode.Q;
        public KeyCode strafeRightKey = KeyCode.E;
    }
}