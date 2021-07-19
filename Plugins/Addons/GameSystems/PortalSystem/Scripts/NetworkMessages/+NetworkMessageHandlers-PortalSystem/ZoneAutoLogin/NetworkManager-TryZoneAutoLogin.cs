//BY FHIZ
//MODIFIED BY DX4D
//Execution: @Client

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// NetworkManager
	// ===================================================================================
	public partial class NetworkManager
	{
        
        /// <summary>
        /// Public function <c>TryLoginPlayer</c>.
        /// Tries to automatically login the player.
        /// Runs on the client. 
        /// </summary>
        /// <param name="username"></param>
        public void TryAutoLoginPlayer(string playername, int token)
		{
			//RequestPlayerAutoLogin(NetworkClient.connection, playername, userName, token); //REMOVED - DX4D
            RequestZoneAutoLogin(playername, userName, token); //ADDED - DX4D
        }

		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================