//BY DX4D
using UnityEngine;

namespace OpenMMO.Targeting
{
    public class DebugInteraction : Interaction
    {
        public override void ServerAction(GameObject user) { Debug.Log("INTERACTION TEST - ServerAction from user: " + user + "\n"
            + (isClient ? ("<color=green>" + " called on server" + "</color>") : ("\n<color=red>" + " not called on server" + "</color>")));
        }
        public override void ClientResponse() { Debug.Log("INTERACTION TEST - ClientResponse" + "\n"
            + (isClient ? ("<color=green>" + " called on client" + "</color>") : ("\n<color=red>" + " not called on client" + "</color>")));
        }
    }
}