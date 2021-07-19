//BY Fhiz
//MODIFIED BY DX4D
//Execution: @Client

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        [DevExtMethods(nameof(OnStartClient))]
        void OnStartClient_NetworkPortals()
        {
            NetworkClient.RegisterHandler<ServerResponseZoneAutoConnect>(OnServerResponseAutoAuth, false);

            OnClientAuthenticated.AddListener(OnClientAuthenticated_NetworkPortals);
        }
    }
}