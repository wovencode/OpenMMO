//BY DX4D
using UnityEngine;

namespace OpenMMO.Targeting
{
    public abstract class SpawnInteraction : Interaction
    {
        [Tooltip("This object is spawned OnInteraction")]
        [SerializeField] GameObject _objectToSpawn = null;
        public GameObject objectToSpawn { get { return _objectToSpawn; } }
        
        public override void ClientAction(GameObject user) { }
        public override void ServerAction(GameObject user) { }
        //public override void ClientResponse() { }
    }
}