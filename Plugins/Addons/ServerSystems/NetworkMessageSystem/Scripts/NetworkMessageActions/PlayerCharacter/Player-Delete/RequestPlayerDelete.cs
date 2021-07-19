//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client
        /// <summary>
        /// Protected override function <c>RequestPlayerDelete</c> that returns a boolean.
        /// Sends a player deletion request to the server.
        /// Checks whether the player deletion request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playerName"></param>
        /// <param name="userName"></param>
        /// <param name="action"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        //protected override bool RequestPlayerDelete(NetworkConnection conn, string playerName, string userName, int action=1) //REMOVED - DX4D
        protected override bool RequestPlayerDelete(string playerName, string userName, int action=1) //ADDED - DX4D
        {
            //if (!base.RequestPlayerDelete(conn, playerName, userName)) //REMOVED - DX4D
            if (!base.RequestPlayerDelete(playerName, userName)) //ADDED - DX4D
                return false;

			ClientRequestPlayerDelete message = new ClientRequestPlayerDelete
			{
				playername = playerName,
				username = userName
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestPlayerDelete), conn.Id(), userName); //DEBUG
			
			return true;
		}
    }
}