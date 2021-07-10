//BY FHIZ
//MODIFIED BY DX4D

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
			NetworkClient.RegisterHandler<ServerResponsePlayerSwitchServer>(GetComponent<ZoneManager>().OnServerMessageResponsePlayerSwitchServer);
			NetworkClient.RegisterHandler<ServerResponsePlayerAutoLogin>(GetComponent<ZoneManager>().OnServerMessageResponsePlayerAutoLogin);
		}

        // ======================= PUBLIC METHODS - PLAYER ================================

        // -------------------------------------------------------------------------------
        // RequestPlayerAutoLogin
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        //protected bool RequestPlayerAutoLogin(NetworkConnection conn, string playerName, string userName, int _token) //REMOVED - DX4D
        protected bool RequestPlayerAutoLogin(string playerName, string userName, int _token) //ADDED - DX4D
        {
		
			//if (!base.RequestPlayerLogin(conn, playerName, userName)) //REMOVED - DX4D
			if (!base.RequestPlayerLogin(playerName, userName)) //ADDED - DX4D
                return false;

			ClientRequestPlayerAutoLogin message = new ClientRequestPlayerAutoLogin
			{
				playername 	= playerName,
				username 	= userName,
				token 		= _token
			};

            //ClientScene.Ready(conn);

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			return true;

		}
		
        // -------------------------------------------------------------------------------

    }
}

// =======================================================================================
