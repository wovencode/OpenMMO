//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client
        /// <summary>
        /// Protected override <c>RequestUserLogin</c> function that returns a boolean.
        /// Sends a user login request to the server.
        /// Checks whether the user login request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        //protected override bool RequestUserLogin(NetworkConnection conn, string userName, string password) //REMOVED - DX4D
        protected override bool RequestUserLogin(string userName, string password) //ADDED - DX4D
        {
            //if (!base.RequestUserLogin(conn, userName, password)) //REMOVED - DX4D
            if (!base.RequestUserLogin(userName, password)) //ADDED - DX4D
                return false;

            ClientRequestUserLogin message = new ClientRequestUserLogin
            {
                username = userName,
                password = Tools.GenerateHash(name, password)
            };

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);

            debug.LogFormat(this.name, nameof(RequestUserLogin), conn.Id(), userName); //DEBUG

            return true;
        }
    }
}