//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client
        /// <summary>
        /// Protected override function <c>RequestUserConfirm</c> that returns a boolean.
        /// Sends a user confirmation request to the server.
        /// Checks whether the user confirmation request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="action"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        //protected override bool RequestUserConfirm(NetworkConnection conn, string userName, string password, int action=1) //REMOVED - DX4D
        protected override bool RequestUserConfirm(string userName, string password, int action=1) //ADDED - DX4D
        {
			//if (!base.RequestUserConfirm(conn, userName, password)) //REMOVED - DX4D
			if (!base.RequestUserConfirm(userName, password)) //ADDED - DX4D
				return false;

			ClientRequestUserConfirm message = new ClientRequestUserConfirm
			{
				username = userName,
				password = Tools.GenerateHash(userName, password)
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserConfirm), conn.Id(), userName); //DEBUG
			
			return true;
		}
    }
}