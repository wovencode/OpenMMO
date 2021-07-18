//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client
        /// <summary>
        /// Protected override function <c>RequestUserChangePassword</c> that returns a boolean.
        /// Sends a user change password request to the server.
        /// Checks whether the user change password request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        //protected override bool RequestUserChangePassword(NetworkConnection conn, string userName, string oldpassword, string newpassword) //REMOVED - DX4D
        protected override bool RequestUserChangePassword(string userName, string oldpassword, string newpassword) //ADDED - DX4D
        {
            //if (!base.RequestUserChangePassword(conn, userName, oldpassword, newpassword)) //REMOVED - DX4D
            if (!base.RequestUserChangePassword(userName, oldpassword, newpassword)) //ADDED - DX4D
                return false;

			ClientRequestUserChangePassword message = new ClientRequestUserChangePassword
			{
				username = name,
				oldPassword = Tools.GenerateHash(userName, oldpassword),
				newPassword = Tools.GenerateHash(userName, newpassword)
			};

			// reset player prefs on password change
			PlayerPrefs.SetString(Constants.PlayerPrefsPassword, "");

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserChangePassword), conn.Id(), userName); //DEBUG
			
			return true;
		}
    }
}