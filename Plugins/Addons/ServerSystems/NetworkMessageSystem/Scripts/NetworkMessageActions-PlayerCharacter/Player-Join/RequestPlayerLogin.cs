//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client
        /// <summary>
        /// Protected override function <c>RequestPlayerLogin</c> that returns a boolean.
        /// Sends a player login request to the server.
        /// Checks whether the player login request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playername"></param>
        /// <param name="username"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        //protected override bool RequestPlayerLogin(NetworkConnection conn, string playername, string username) //REMOVED - DX4D
        protected override bool RequestPlayerLogin(string playername, string username) //ADDED - DX4D
        {

            //if (!base.RequestPlayerLogin(conn, playername, username)) //REMOVED - DX4D
            if (!base.RequestPlayerLogin(playername, username)) //ADDED - DX4D
                return false;

			ClientRequestPlayerLogin message = new ClientRequestPlayerLogin
			{
				playername = playername,
				username = username
			};
			
			// must be readied here, not in the response - otherwise it generates a warning
			//ClientScene.Ready(conn); //REMOVED - DX4D
			NetworkClient.Ready(); //ADDED - DX4D

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestPlayerLogin), conn.Id(), username, playername); //DEBUG
			
			return true;
		}
    }
}