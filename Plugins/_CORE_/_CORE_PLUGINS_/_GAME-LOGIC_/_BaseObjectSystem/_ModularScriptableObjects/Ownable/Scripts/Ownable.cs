//BY DX4D
using UnityEngine;
using Mirror;

namespace OpenMMO
{
    public abstract class Ownable : GameItem
    {
        /// <summary>The unique id assigned to this object.</summary>
        [Tooltip("The unique identification number of this object.")]
        [SerializeField, ReadOnly] double _id = 0;
        public double id
        {
            get
            {
                if (_id == 0) _id = name.GetStableHashCode();
                return _id; }
        }

        private void Awake()
        {
            if (_id == 0) _id = name.GetStableHashCode();
        }
    }
}