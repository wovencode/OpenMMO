
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Zones;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
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
			NetworkClient.RegisterHandler<ServerMessageResponsePlayerSwitchServer>(GetComponent<ZoneManager>().OnServerMessageResponsePlayerSwitchServer);
			NetworkClient.RegisterHandler<ServerMessageResponsePlayerAutoLogin>(GetComponent<ZoneManager>().OnServerMessageResponsePlayerAutoLogin);
		}

        // ======================= PUBLIC METHODS - PLAYER ================================

        // -------------------------------------------------------------------------------
        // RequestPlayerAutoLogin
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        protected bool RequestPlayerAutoLogin(NetworkConnection conn, string playerName, string userName, int _token)
		{
		
			if (!base.RequestPlayerLogin(conn, playerName, userName))
				return false;

			ClientMessageRequestPlayerAutoLogin message = new ClientMessageRequestPlayerAutoLogin
			{
				playername 	= playerName,
				username 	= userName,
				token 		= _token
			};
			
			//ClientScene.Ready(conn);
			
			conn.Send(message);
			
			return true;

		}
		
        // -------------------------------------------------------------------------------

    }
}

// =======================================================================================
