//BY DX4D
using UnityEngine;

namespace OpenMMO
{
	/// <summary>A snapshot of a current moving state</summary>
    //[CreateAssetMenu(menuName = "OpenMMO/Controls/Movement State")]
    [System.Serializable]
    public struct MovementState// : ScriptableObject
    {
        [Header("Movement Modes")]
        public bool running;
        public bool sneaking;

        [Header("Maneuvers")]
        public bool jumping;

        [Header("Movement Direction")]
        public float movingHorizontal;
        public float movingVertical;

        internal void Reset()
        {
            running = false;
            sneaking = false;
            jumping = false;
            movingHorizontal = 0f;
            movingVertical = 0f;
        }
    }
}