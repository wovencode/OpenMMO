//BY DX4D
using UnityEngine;

namespace OpenMMO.Targeting
{
    public class Targetable : MonoBehaviour
    {
        [Header("VISUAL REPRESENTATION")]
        [Tooltip("An icon representing this targetable when it is targeted.")]
        [SerializeField] Sprite _icon;
        public Sprite icon { get { return _icon; } }

        [Header("LOCATION")]
        [Tooltip("The layer(s) that this object interacts with.")]
        [SerializeField] LayerMask _layer = new LayerMask() { value = 1 }; //Default Layer
        public LayerMask layer { get { return _layer; } }

        public Vector3 position
        {
            get { return transform.position; }
        }

        public bool IsAlive
        {
            get { return true; } //TODO
        }
    }
}