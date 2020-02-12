
using OpenMMO;
using OpenMMO.Network;
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

        // ======================= PUBLIC METHODS - PLAYER ================================

        // -------------------------------------------------------------------------------
        // RequestPlayerAutoLogin
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        protected bool RequestPlayerAutoLogin(NetworkConnection conn, string playerName, string userName)
		{
		
			Debug.Log("RequestPlayerAutoLogin:"+playerName+"/"+userName);
		
			if (!base.RequestPlayerLogin(conn, playerName, userName))
				return false;

			ClientMessageRequestPlayerAutoLogin message = new ClientMessageRequestPlayerAutoLogin
			{
				playername = playerName,
				username = userName
			};

			ClientScene.Ready(conn);

			conn.Send(message);

			return true;

		}

        // -------------------------------------------------------------------------------

    }
}

// =======================================================================================
