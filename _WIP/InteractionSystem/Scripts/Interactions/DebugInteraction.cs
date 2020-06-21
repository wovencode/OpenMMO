//BY DX4D
using UnityEngine;

namespace OpenMMO.Targeting
{
    public class DebugInteraction : Interaction
    {
        public override void ServerAction(GameObject user) { Debug.Log("INTERACTION TEST - ServerAction\nUser: " + user
            + (isClient ? ("\n<color=green>" + "server" + "</color>") : ("\n<color=red>" + "not server" + "</color>")));
        }
        public override void ClientResponse() { Debug.Log("INTERACTION TEST - ClientResponse"
            + (isClient ? ("\n<color=green>" + "client" + "</color>") : ("\n<color=red>" + "not client" + "</color>")));
        }
    }
}