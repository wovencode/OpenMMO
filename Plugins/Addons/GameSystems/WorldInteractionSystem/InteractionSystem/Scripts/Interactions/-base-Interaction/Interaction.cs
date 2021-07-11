//BY DX4D

using UnityEngine;
using Mirror;

namespace OpenMMO
{
    [RequireComponent(typeof(Targetable))]
    public abstract class Interaction : NetworkBehaviour
    {
        //INTERACTION RANGE
        [SerializeField] private int _interactionRange = 5;
        public int interactionRange { get { return _interactionRange; } }

        public bool CanInteract(Transform location)
        {
            if (Vector3.Distance(location.position, gameObject.transform.position) <= interactionRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //INTERACT
        //[Server][Client]
        public void Interact(InteractionSystem interactionSystem)
        {
            if (CanInteract(interactionSystem.gameObject.transform))
            {
                if (isServer) OnInteractServer(interactionSystem.gameObject);
                if (isClient) OnInteractClient(interactionSystem.gameObject);
            }
#if UNITY_EDITOR && DEBUG
            else
            {
                if (!isLocalPlayer) return; //LOCAL PLAYER ONLY
                Debug.Log(name.ToUpper() + " " + "CANNOT INTERACT");
            }
#endif
        }

        //SERVER ACTION
        ///<summary>NOTE: Be sure to use [Server] when implementing this method!</summary>
        public abstract void ServerAction(GameObject user);
        [Server] public void OnInteractServer(GameObject user) { ServerAction(user); }// RpcRespondToClient(); //TargetRespondToClient();

        //CLIENT ACTION
        ///<summary>NOTE: Be sure to use [Client] when implementing this method!</summary>
        public abstract void ClientAction(GameObject user);
        [Client] public void OnInteractClient(GameObject user) { ClientAction(user); } //TargetRespondToClient(); 

        ////SINGLE CLIENT RESPONSE
        //[TargetRpc] public void TargetRpcRespondToClient() { ClientResponse(); }
        //[Client] public abstract void ClientResponse();
        //MULTI CLIENT RESPONSE
        //[ClientRpc] public void RpcRespondToClient() { ClientResponse(); }
        //[Client] public abstract void ClientResponse();

    }
}