using UnityEngine;

namespace OpenMMO
{

    [CreateAssetMenu(menuName = "OpenMMO/Config/Player Controls")]
    public class PlayerControlConfig : ScriptableObject
    {
        [Header("Input")]
        public KeyCode runKey = KeyCode.LeftShift;
        public string moveAxisHorizontal = "Horizontal";
        public string moveAxisVertical = "Vertical";

        [Header("Movement Factors")]
        public float rotationSpeed = 100;
        [Range(0, 100)] public float walkFactor = 1.0f;
        [Range(0, 100)] public float runFactor = 1.5f;
        [Range(0, 100)] public float backwardFactor = 0.5f;
    }
}