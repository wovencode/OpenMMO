//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;
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
            Debug.Log("[REGISTER NETWORK MESSAGES] - [CHARACTER CLIENT] - [AUTOJOINPLAYER] - "
                + "Registering Message Handlers to Client...");
			NetworkClient.RegisterHandler<ServerResponsePlayerAutoLogin>(GetComponent<ZoneManager>().OnServerMessageResponsePlayerAutoLogin);

            Debug.Log("[REGISTER NETWORK MESSAGES] - [ZONE CLIENT] - [SWITCHSERVER] - "
                + "Registering Message Handlers to Client...");
            NetworkClient.RegisterHandler<ServerResponsePlayerSwitchServer>(GetComponent<ZoneManager>().OnServerMessageResponsePlayerSwitchServer);
		}
    }
}
