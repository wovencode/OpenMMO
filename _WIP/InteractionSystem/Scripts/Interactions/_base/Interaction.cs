//BY DX4D
using UnityEngine;
using Mirror;

namespace OpenMMO.Targeting
{
    [RequireComponent(typeof(Targetable))]
    public abstract class Interaction : NetworkBehaviour
    {
        
        //INTERACTION RANGE
        [SerializeField] private int _interactionRange;
        public int interactionRange { get { return _interactionRange; } }

        public bool CanInteract(Transform location)
        {
            if (Vector3.Distance( location.position, gameObject.transform.position) <= interactionRange)
            {
                return true;
            }

            return false;
        }

        //INTERACT
        [Server]
        public void Interact(InteractionSystem interactionSystem)
        {
            if (CanInteract(interactionSystem.gameObject.transform))
            {
                OnInteractServer(interactionSystem.gameObject);
            }
        }

        [Server] public abstract void ServerAction(GameObject user);
        
        [Server] public void OnInteractServer(GameObject user) { RpcRespondToClient(); ServerAction(user); } //TargetRespondToClient(); 

        //SINGLE CLIENT RESPONSE
        //[TargetRpc] public void TargetRespondToClient() { ClientResponse(); }
        //[Client] public abstract void ClientResponse();
        //MULTI CLIENT RESPONSE
        [ClientRpc] public void RpcRespondToClient() { ClientResponse(); }
        [Client] public abstract void ClientResponse();

    }
}