//BY DX4D

using UnityEngine;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        [Header("EVENT HANDLERS")]
        public NetworkManager_Events eventListeners;

        //@CLIENT
        public override void OnStartClient()
        {
            this.InvokeInstanceDevExtMethods(nameof(OnStartClient)); //HOOK
            eventListeners.OnStartClient.Invoke(); //EVENT
        }
        //@SERVER
        public override void OnStartServer()
        {
            this.InvokeInstanceDevExtMethods(nameof(OnStartServer)); //HOOK
            eventListeners.OnStartServer.Invoke(); //EVENT
        }
    }
}
