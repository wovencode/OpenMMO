//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client
        /// <summary>
        /// Protected override function <c>RequestPlayerRegister</c> that returns a boolean.
        /// Sends a player register request to the server.
        /// Checks whether the player register request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playerName"></param>
        /// <param name="userName"></param>
        /// <param name="prefabName"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        //protected override bool RequestPlayerRegister(NetworkConnection conn, string playerName, string userName, string prefabName) //REMOVED - DX4D
        protected override bool RequestPlayerRegister(string playerName, string userName, string prefabName) //ADDED - DX4D
        {
            //if (!base.RequestPlayerRegister(conn, playerName, userName, prefabName)) //REMOVED - DX4D
            if (!base.RequestPlayerRegister(playerName, userName, prefabName)) //ADDED - DX4D
                return false;

			ClientRequestPlayerRegister message = new ClientRequestPlayerRegister
			{
				playername 	= playerName,
				username 	= userName,
				prefabname 	= prefabName
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestPlayerRegister), conn.Id(), playerName, prefabName); //DEBUG
			
			return true;
		}
    }
}