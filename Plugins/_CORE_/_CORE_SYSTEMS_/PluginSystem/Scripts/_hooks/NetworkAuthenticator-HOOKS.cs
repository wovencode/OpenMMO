//BY DX4D

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        //@CLIENT
        public override void OnStartClient()
        {
            this.InvokeInstanceDevExtMethods(nameof(OnStartClient)); //HOOK
        }

        //@SERVER
        public override void OnStartServer()
        {
            this.InvokeInstanceDevExtMethods(nameof(OnStartServer)); //HOOK
        }
    }
}
