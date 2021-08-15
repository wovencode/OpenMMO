//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // C O N F I R M  U S E R

        /// <summary>Try to confirm an existing User according to its name and password
        /// Public function <c>TryConfirmUser</c>.
        /// Tries to confirm an existing User according to its name and password.
        /// Runs on the client. 
        /// </summary>
        /// <param name="username"></param><param name="password"></param>
        // @Client
        public void TryConfirmUser(string username, string password)
        {
            //RequestUserConfirm(NetworkClient.connection, username, password); //REMOVED - DX4D
            RequestUserConfirm(username, password); //ADDED - DX4D
        }
        /// <summary>
        /// Protected override function <c>RequestUserConfirm</c> that returns a boolean.
        /// Sends a user confirmation request to the server.
        /// Checks whether the user confirmation request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param><param name="userName"></param>
        /// <param name="password"></param><param name="action"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        // @Client
        //protected override bool RequestUserConfirm(NetworkConnection conn, string userName, string password, int action=1) //REMOVED - DX4D
        protected override bool RequestUserConfirm(string userName, string password, int action=1) //ADDED - DX4D
        {
			//if (!base.RequestUserConfirm(conn, userName, password)) //REMOVED - DX4D
			if (!CanUserConfirm(userName, password)) return false; //ADDED - DX4D

            Request.UserConfirmRequest message = new Request.UserConfirmRequest
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
