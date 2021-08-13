//BY FHIZ
//MODIFIED BY DX4D

using System;
using Mirror;

namespace OpenMMO.Network
{

	// ===================================================================================
	// BaseNetworkManager
	// ===================================================================================
	public abstract partial class BaseNetworkManager
	{
        // ======================= PUBLIC METHODS - USER =================================

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserLogin</c> that requests the login for a user and returns a boolean value.
        /// The base version fo the method checks whether the username is allowed and the password is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> A boolean value determining whether the user can login </returns>
        //protected virtual bool RequestUserLogin(NetworkConnection conn, string username, string password) //REMOVED - DX4D
        protected abstract bool RequestUserLogin(string username, string password); //ADDED - DX4D
        protected virtual bool CanUserLogin(string username, string password) //ADDED - DX4D
        {
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(password));
		}

        // -------------------------------------------------------------------------------
        //protected virtual bool RequestUserLogout(NetworkConnection conn) //REMOVED - DX4D
        protected abstract bool RequestUserLogout(); //ADDED - DX4D
		protected virtual bool CanUserLogout() //ADDED - DX4D
        {
			//return (conn != null); //REMOVED - DX4D
			return (NetworkClient.connection != null); //ADDED - DX4D
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserRegister</c> requests whether the user can regsiter and returns a boolean value.
        /// The base version fo the method checks whether the username is allowed and the password is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="usermail"></param>
        /// <returns> A boolean value determining whether the user can register </returns>
        //protected virtual bool RequestUserRegister(NetworkConnection conn, string username, string password, string usermail) //REMOVED - DX4D
        protected abstract bool RequestUserRegister(string username, string password, string usermail); //ADDED - DX4D
        protected virtual bool CanUserRegister(string username, string password, string usermail) //ADDED - DX4D
        {
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(password));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserDelete</c> requests whether the user can be deleted.
        /// The base version fo the method checks whether the username is allowed and the password is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="_action"></param>
        /// <returns> A boolean value determining whether the user can deleted their account </returns>
        //protected virtual bool RequestUserDelete(NetworkConnection conn, string username, string password, int _action=1) //REMOVED - DX4D
        protected abstract bool RequestUserDelete(string username, string password, int _action = 1); //ADDED - DX4D
        protected virtual bool CanUserDelete(string username, string password, int _action=1) //ADDED - DX4D
        {
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(password));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserChangePassword</c> requests whether the user can change their password.
        /// The base version fo the method checks whether the username is allowed, the passwords (bot old and new) are allowed and if the old and new password are different.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        /// <returns> A boolean value determining whether the user can change their password </returns>
        //protected virtual bool RequestUserChangePassword(NetworkConnection conn, string username, string oldpassword, string newpassword) //REMOVED - DX4D
        protected abstract bool RequestUserChangePassword(string username, string oldpassword, string newpassword); //ADDED - DX4D
        protected virtual bool CanUserChangePassword(string username, string oldpassword, string newpassword) //ADDED - DX4D
        {
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(oldpassword) && Tools.IsAllowedPassword(newpassword) && oldpassword != newpassword);
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserConfirm</c>.
        /// The base version fo the method checks whether the name is allowed and the password is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="_action"></param>
        /// <returns> A boolean value determining whether the user request has been confirmed </returns>
        //protected virtual bool RequestUserConfirm(NetworkConnection conn, string username, string password, int _action=1) //REMOVED - DX4D
        protected abstract bool RequestUserConfirm(string username, string password, int _action = 1); //ADDED - DX4D
        protected virtual bool CanUserConfirm(string username, string password, int _action=1) //ADDED - DX4D
        {
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(password));
		}

        // ======================= PUBLIC METHODS - PLAYER =================================

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual player method <c>RequestPlayerLogin</c> that request the player login.
        /// The base version fo the method checks whether the name is allowed and the username is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playername"></param>
        /// <param name="username"></param>
        /// <returns> A boolean value determing whether the player can login </returns>
        //protected virtual bool RequestPlayerLogin(NetworkConnection conn, string playername, string username) //REMOVED - DX4D
        protected abstract bool RequestPlayerLogin(string playername, string username); //ADDED - DX4D
        //CAN PLAYER LOGIN
        protected virtual bool CanPlayerLogin(string playername, string username) //ADDED - DX4D
        {
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual player method <c>RequestPlayerRegister</c> that registers the player
        /// The base version fo the method checks whether the name is allowed, the username is allowed and if a player prefab name is passed
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playername"></param>
        /// <param name="username"></param>
        /// <param name="prefabname"></param>
        /// <returns> A boolean value determining whether the player can register </returns>
        //protected virtual bool RequestPlayerRegister(NetworkConnection conn, string playername, string username, string prefabname) //REMOVED - DX4D
        protected abstract bool RequestPlayerRegister(string playername, string username, string prefabname); //ADDED - DX4D
        //CAN PLAYER REGISTER
        protected virtual bool CanPlayerRegister(string playername, string username, string prefabname) //ADDED - DX4D
        {
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username) && !String.IsNullOrWhiteSpace(prefabname));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        ///  Public virtual player method <c>RequestPlayerDelete</c> that deletes the player
        /// The base version fo the method checks whether the name is allowed and the username is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playername"></param>
        /// <param name="username"></param>
        /// <param name="_action"></param>
        /// <returns> A boolean value determining whether the player has been deleted </returns>
        //protected virtual bool RequestPlayerDelete(NetworkConnection conn, string playername, string username, int _action=1) //REMOVED - DX4D
        protected abstract bool RequestPlayerDelete(string playername, string username, int _action=1); //ADDED - DX4D
        protected virtual bool CanPlayerDelete(string playername, string username, int _action = 1) //ADDED - DX4D
        {
            return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username));
		}
	}
}
