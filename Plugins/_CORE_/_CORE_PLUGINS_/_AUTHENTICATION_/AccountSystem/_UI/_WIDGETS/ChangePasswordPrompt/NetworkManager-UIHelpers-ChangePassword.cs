//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // C H A N G E  P A S S W O R D

        /// <summary>Try to delete an existing User according to its name and password
        /// Public function <c>TryDeleteUser</c>.
        /// Tries to delete an existing User according to its name and password.
        /// Runs on the client.  
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldpassword"></param><param name="newpassword"></param>
        // @Client
        public void TryChangePasswordUser(string username, string oldpassword, string newpassword)
        {

            userName = username;
            userPassword = oldpassword;
            newPassword = newpassword;

            //RequestUserChangePassword(NetworkClient.connection, username, oldpassword, newpassword); //REMOVED - DX4D
            RequestUserChangePassword(username, oldpassword, newpassword); //ADDED - DX4D

        }
        /// <summary>
        /// Protected override function <c>RequestUserChangePassword</c> that returns a boolean.
        /// Sends a user change password request to the server.
        /// Checks whether the user change password request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param><param name="userName"></param>
        /// <param name="oldpassword"></param><param name="newpassword"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        // @Client
        //protected override bool RequestUserChangePassword(NetworkConnection conn, string userName, string oldpassword, string newpassword) //REMOVED - DX4D
        protected override bool RequestUserChangePassword(string userName, string oldpassword, string newpassword) //ADDED - DX4D
        {
            //if (!base.RequestUserChangePassword(conn, userName, oldpassword, newpassword)) //REMOVED - DX4D
            if (!CanUserChangePassword(userName, oldpassword, newpassword)) return false; //ADDED - DX4D

            Request.UserChangePasswordRequest message = new Request.UserChangePasswordRequest
			{
				username = name,
				oldPassword = Tools.GenerateHash(userName, oldpassword),
				newPassword = Tools.GenerateHash(userName, newpassword)
			};

			// reset player prefs on password change
			PlayerPrefs.SetString(Defines.Login.Password, "");

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserChangePassword), conn.Id(), userName); //DEBUG
			
			return true;
		}
        /// <summary>Can we change the provided users password?
        /// Public function <c>CanChangePasswordUser</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the user's password can be changed
        /// //Called by <see cref="UI.UIWindowChangePasswordUser"/>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldpassword"></param><param name="newpassword"></param>
        /// <returns>  Returns a boolean value detailing whether the user can change their password </returns>
        // @Client
        public bool CanChangePasswordUser(string username, string oldpassword, string newpassword)
        {
            return isNetworkActive &&
                Tools.IsAllowedName(username) &&
                Tools.IsAllowedPassword(oldpassword) &&
                Tools.IsAllowedPassword(newpassword) &&
                IsConnecting();
        }
    }
}
