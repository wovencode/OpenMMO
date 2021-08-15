//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // L O G I N  U S E R

        /// <summary>Try to login into an existing user using name and password provided
        ///  Public function <c>TryLoginUser</c>.
        ///  Tries to login the user.
        ///  Runs on the client.
        /// </summary>
        /// <param name="username"></param><param name="password"></param>
        // @Client
        public void TryLoginUser(string username, string password)
        {

            userName = username;
            userPassword = password;

            //RequestUserLogin(NetworkClient.connection, username, password); //REMOVED - DX4D
            RequestUserLogin(username, password); //ADDED - DX4D

        }
        /// <summary>
        /// Protected override <c>RequestUserLogin</c> function that returns a boolean.
        /// Sends a user login request to the server.
        /// Checks whether the user login request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param><param name="password"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        // @Client
		//protected override bool RequestUserLogin(NetworkConnection conn, string userName, string password) //REMOVED - DX4D
		protected override bool RequestUserLogin(string userName, string password) //ADDED - DX4D
		{
			//if (!base.RequestUserLogin(conn, userName, password)) //REMOVED - DX4D
            if (!CanUserLogin(userName, password)) return false; //ADDED - DX4D

            Request.UserLoginRequest message = new Request.UserLoginRequest
			{
				username = userName,
				password = Tools.GenerateHash(name, password)
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserLogin), conn.Id(), userName); //DEBUG
			
			return true;

		}
        /// <summary>Can we login into an existing user with the provided name and password?
        /// Public function <c>CanLoginUser</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the user can login checks the username and password
        /// Called by <see cref="UI.UILoginUserPrompt"/>
        /// </summary>
        /// <param name="username"></param><param name="password"></param>
        /// <returns> Returns a boolean value detailing whether the user can login </returns>
        // @Client
        public bool CanLoginUser(string username, string password)
        {
            return isNetworkActive &&
                Tools.IsAllowedName(username) &&
                Tools.IsAllowedPassword(password) &&
                IsConnecting();
        }
    }
}
