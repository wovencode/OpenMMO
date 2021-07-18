//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Zones;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
		// -----------------------------------------------------------------------------------
		// @Client
		// -----------------------------------------------------------------------------------
		[DevExtMethods(nameof(OnStartClient))]
		void OnStartClient_NetworkPortals()
		{
			NetworkClient.RegisterHandler<ServerResponseZoneSwitchServer>(GetComponent<ZoneManager>().OnServerResponseZoneSwitchServer);
			NetworkClient.RegisterHandler<ServerResponseZoneAutoLogin>(GetComponent<ZoneManager>().OnServerResponseZoneAutoLogin);
		}
    }
}