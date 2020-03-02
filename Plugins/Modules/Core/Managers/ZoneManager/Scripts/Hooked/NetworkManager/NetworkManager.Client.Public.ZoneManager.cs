
using OpenMMO;
using OpenMMO.Network;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// NetworkManager
	// ===================================================================================
	public partial class NetworkManager
	{

        // ======================= PUBLIC METHODS - PLAYER ===============================

        // -------------------------------------------------------------------------------
        // TryLoginPlayer
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryLoginPlayer</c>.
        /// Tries to automatically login the player.
        /// Runs on the client. 
        /// </summary>
        /// <param name="username"></param>
        public void TryAutoLoginPlayer(string playername, int token)
		{
			RequestPlayerAutoLogin(NetworkClient.connection, playername, userName, token);
		}

		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================