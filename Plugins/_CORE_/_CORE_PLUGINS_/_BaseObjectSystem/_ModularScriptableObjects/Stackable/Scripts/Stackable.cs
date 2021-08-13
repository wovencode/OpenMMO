//BY DX4D
using UnityEngine;

namespace OpenMMO
{
    /// <summary>[Ownable] [Stackable]</summary>
    public abstract class Stackable : Ownable
    {
        /// <summary>The max number of items this stack can contain.</summary>
        [SerializeField] ushort _capacity = 1;
        public ushort capacity { get { return _capacity; } }
    }
}