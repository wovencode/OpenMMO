//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // R E G I S T E R  U S E R

        /// <summary>Try to register a new User with name and password provided
        /// Public function <c>TryRegisterUser</c>.
        /// Tries to register the user.
        /// Runs on the client.
        /// Called by <see cref="UI.UIRegisterUserPrompt"/>
        /// </summary>
        /// <param name="username"></param><param name="password"></param>
        /// <param name="email"></param>
        // @Client
        public void TryRegisterUser(string username, string password, string email)
        {

            userName = username;
            userPassword = password;


            //RequestUserRegister(NetworkClient.connection, username, password, email); //REMOVED - DX4D
            RequestUserRegister(username, password, email); //ADDED - DX4D
        }
        /// <summary>
        /// Protected override function <c>RequestUserRegister</c> that returns a boolean.
        /// Sends a user registration request to the server.
        /// Checks whether the user register request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param><param name="userName"></param>
        /// <param name="password"></param><param name="usermail"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        // @Client
        //protected override bool RequestUserRegister(NetworkConnection conn, string userName, string password, string usermail) //REMOVED - DX4D
        protected override bool RequestUserRegister(string userName, string password, string usermail) //ADDED - DX4D
        {
            //if (!base.RequestUserRegister(conn, userName, password, usermail)) //REMOVED - DX4D
            if (!CanUserRegister(userName, password, usermail)) return false; //ADDED - DX4D

            Request.UserRegisterRequest message = new Request.UserRegisterRequest
			{
				username 	= userName,
				password 	= Tools.GenerateHash(name, password),
				email 		= usermail,
				deviceid	= Tools.GetDeviceId
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserRegister), conn.Id(), userName); //DEBUG
						
			return true;
		}
        /// <summary>Can we register a new user with the provided name and password?
        /// Public function <c>CanRegisterUser</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the user can register checks the username and password
        /// Called by <see cref="UI.UIRegisterUserPrompt"/>
        /// </summary>
        /// <param name="username"></param><param name="password"></param>
        /// <returns> Returns a boolean value detailing whether the user can register </returns>
        // @Client
        public bool CanRegisterUser(string username, string password)
        {
            return isNetworkActive &&
                Tools.IsAllowedName(username) &&
                Tools.IsAllowedPassword(password) &&
                IsConnecting();
        }
    }
}
