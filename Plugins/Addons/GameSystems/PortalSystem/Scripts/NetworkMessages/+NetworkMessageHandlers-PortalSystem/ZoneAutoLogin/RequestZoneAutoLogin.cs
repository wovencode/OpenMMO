//BY FHIZ
//MODIFIED BY DX4D
//Direction: @Client -> @Server
//Execution: @Server

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        //protected bool RequestZoneAutoLogin(NetworkConnection conn, string playerName, string userName, int _token) //REMOVED - DX4D
        protected bool RequestZoneAutoLogin(string playerName, string userName, int _token) //ADDED - DX4D
        {
		
			//if (!base.RequestPlayerLogin(conn, playerName, userName)) //REMOVED - DX4D
			if (!base.RequestPlayerLogin(playerName, userName)) //ADDED - DX4D
                return false;

			ClientRequestZoneAutoLogin message = new ClientRequestZoneAutoLogin
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
    }
}