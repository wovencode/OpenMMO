//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Network;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

using OpenMMO.Zones;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
    public partial class NetworkManager
    {
        // ======================= PUBLIC METHODS - PLAYER ================================

        // -------------------------------------------------------------------------------
        // RequestPlayerAutoLogin
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        //protected bool RequestPlayerAutoLogin(NetworkConnection conn, string playerName, string userName, int _token) //REMOVED - DX4D
        protected bool RequestPlayerAutoLogin(NetworkConnection conn, string playerName, string userName, int _token) //ADDED - DX4D
        {
		
			//if (!base.RequestPlayerLogin(conn, playerName, userName)) //REMOVED - DX4D
			if (!base.RequestPlayerLogin(playerName, userName)) return false; //ADDED - DX4D

			ClientRequestPlayerAutoLogin message = new ClientRequestPlayerAutoLogin
			{
				playername 	= playerName,
				username 	= userName,
				token 		= _token
			};

            //ClientScene.Ready(conn); //REMOVED - DX4D
            if (!NetworkClient.ready) NetworkClient.Ready(); //ADDED - DX4D

            //NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			return true;

		}
		
        // -------------------------------------------------------------------------------

    }
}

// =======================================================================================
