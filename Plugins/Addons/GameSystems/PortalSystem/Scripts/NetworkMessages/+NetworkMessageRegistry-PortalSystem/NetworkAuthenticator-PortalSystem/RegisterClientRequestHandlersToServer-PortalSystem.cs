//BY FHIZ
//MODIFIED BY DX4D
//Execution: @Server

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        [DevExtMethods(nameof(OnStartServer))]
        void OnStartServer_NetworkPortals()
        {
            NetworkServer.RegisterHandler<ClientRequestZoneAutoConnect>(OnClientRequestZoneAutoConnect, false);
        }
    }
}