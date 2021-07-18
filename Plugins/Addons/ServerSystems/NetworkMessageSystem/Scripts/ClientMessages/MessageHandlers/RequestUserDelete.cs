//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client
        /// <summary>
        /// Protected override function <c>RequestUserDelete</c> that returns a boolean.
        /// Sends a user deletion request to the server.
        /// Checks whether the user deletion request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="action"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        //protected override bool RequestUserDelete(NetworkConnection conn, string userName, string password, int action=1) //REMOVED - DX4D
        protected override bool RequestUserDelete(string userName, string password, int action=1) //ADDED - DX4D
        {
            //if (!base.RequestUserDelete(conn, userName, password)) //REMOVED - DX4D
            if (!base.RequestUserDelete(userName, password)) //ADDED - DX4D
                return false;

			ClientRequestUserDelete message = new ClientRequestUserDelete
			{
				username = userName,
				password = Tools.GenerateHash(name, password)
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserDelete), conn.Id(), userName); //DEBUG
						
			return true;
		}
    }
}