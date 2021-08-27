//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // D E L E T E  U S E R

        /// <summary>Try to delete an existing User according to its name and password
        /// Public function <c>TryDeleteUser</c>.
        /// Tries to delete the user according to its username and password.
        /// Runs on the client.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        // @Client
        public void TryDeleteUser(string username, string password)
        {

            userName = username;
            userPassword = password;

            //RequestUserDelete(NetworkClient.connection, username, password); //REMOVED - DX4D
            RequestUserDelete(username, password); //ADDED - DX4D

        }
        /// <summary>
        /// Protected override function <c>RequestUserDelete</c> that returns a boolean.
        /// Sends a user deletion request to the server.
        /// Checks whether the user deletion request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="userName"></param><param name="password"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        // @Client
        //protected override bool RequestUserDelete(NetworkConnection conn, string userName, string password, int action=1) //REMOVED - DX4D
        protected override bool RequestUserDelete(string userName, string password) //ADDED - DX4D
        {
            //if (!base.RequestUserDelete(conn, userName, password)) //REMOVED - DX4D
            if (!CanUserDelete(userName, password)) return false; //ADDED - DX4D

            Request.UserDeleteRequest message = new Request.UserDeleteRequest
            {
                username = userName,
                password = Tools.GenerateHash(name, password),
                success = true
			};

            //NetworkConnection conn = NetworkClient.connection; //REMOVED - DX4D
            //conn.Send(message); //REMOVED - DX4D

            NetworkClient.connection.Send(message); //ADDED DX4D

            //debug.LogFormat(this.name, nameof(RequestUserDelete), conn.Id(), userName); //DEBUG //REMOVED - DX4D

            return true;
		}
        /// <summary>Can we delete an user with the provided name and password?
        /// Public function <c>CanDeleteUser</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the user can be deleted checks the username and password
        /// Called by <see cref="UI.UIWindowDeleteUser"/>
        /// </summary>
        /// <param name="username"></param><param name="password"></param>
        /// <returns> Returns a boolean value detailing whether the user can be deleted </returns>
        // @Client
        public bool CanDeleteUser(string username, string password)
        {
            return isNetworkActive &&
                Tools.IsAllowedName(username) &&
                Tools.IsAllowedPassword(password) &&
                IsConnecting();
        }
    }
}
