//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Zones;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
    public partial class NetworkManager
    {

		// -----------------------------------------------------------------------------------
		// OnStartClient_NetworkPortals
		// @Client
		// -----------------------------------------------------------------------------------
		[DevExtMethods(nameof(OnStartClient))]
		void OnStartClient_NetworkPortals()
		{
			NetworkClient.RegisterHandler<ServerResponsePlayerSwitchServer>(GetComponent<ZoneManager>().OnServerMessageResponsePlayerSwitchServer);
			NetworkClient.RegisterHandler<ServerResponsePlayerAutoLogin>(GetComponent<ZoneManager>().OnServerMessageResponsePlayerAutoLogin);
		}
    }
}
