//BY DX4D
using UnityEngine;

namespace OpenMMO.Targeting
{
    public class GetItemInteraction : Interaction
    {
        [Tooltip("This item is added to inventory OnInteraction")]
        [SerializeField] Item _itemToAdd;
        public Item itemToAdd { get { return _itemToAdd; } }
        
        public override void ClientAction(GameObject user) { }
        public override void ServerAction(GameObject user) { }
        //public override void ClientResponse() { }
    }
}