
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
    public partial class NetworkManager
    {

    	// ======================= PUBLIC METHODS - USER =================================

        // -------------------------------------------------------------------------------
        // RequestUserLogin
        // @Client
		// -------------------------------------------------------------------------------
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
		protected override bool RequestUserLogin(NetworkConnection conn, string userName, string password)
		{
			if (!base.RequestUserLogin(conn, userName, password))
				return false;

			ClientMessageRequestUserLogin message = new ClientMessageRequestUserLogin
			{
				username = userName,
				password = Tools.GenerateHash(name, password)
			};

			conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserLogin), conn.Id(), userName); //DEBUG
			
			return true;

		}
		
		// -------------------------------------------------------------------------------
        // RequestUserLogout
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestUserLogout(NetworkConnection conn)
		{
		
			ClientMessageRequestUserLogout message = new ClientMessageRequestUserLogout
			{

			};

			conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserLogout), conn.Id(), userName); //DEBUG
			
			return true;

		}
		
        // -------------------------------------------------------------------------------
        // RequestUserRegister
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Protected override function <c>RequestUserRegister</c> that returns a boolean.
        /// Sends a user registration request to the server.
        /// Checks whether the user register request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="usermail"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        protected override bool RequestUserRegister(NetworkConnection conn, string userName, string password, string usermail)
		{
			if (!base.RequestUserRegister(conn, userName, password, usermail))
				return false;

			ClientMessageRequestUserRegister message = new ClientMessageRequestUserRegister
			{
				username 	= userName,
				password 	= Tools.GenerateHash(name, password),
				email 		= usermail,
				deviceid	= Tools.GetDeviceId
			};

			conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserRegister), conn.Id(), userName); //DEBUG
						
			return true;

		}

        // -------------------------------------------------------------------------------
        // RequestUserDelete
        // @Client
        // -------------------------------------------------------------------------------
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
        protected override bool RequestUserDelete(NetworkConnection conn, string userName, string password, int action=1)
		{
			if (!base.RequestUserDelete(conn, userName, password))
				return false;

			ClientMessageRequestUserDelete message = new ClientMessageRequestUserDelete
			{
				username = userName,
				password = Tools.GenerateHash(name, password)
			};

			conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserDelete), conn.Id(), userName); //DEBUG
						
			return true;

		}

        // -------------------------------------------------------------------------------
        // RequestUserChangePassword
        // @Client
        // -------------------------------------------------------------------------------
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
        protected override bool RequestUserChangePassword(NetworkConnection conn, string userName, string oldpassword, string newpassword)
		{
			if (!base.RequestUserChangePassword(conn, userName, oldpassword, newpassword))
				return false;

			ClientMessageRequestUserChangePassword message = new ClientMessageRequestUserChangePassword
			{
				username = name,
				oldPassword = Tools.GenerateHash(userName, oldpassword),
				newPassword = Tools.GenerateHash(userName, newpassword)
			};

			// reset player prefs on password change
			PlayerPrefs.SetString(Constants.PlayerPrefsPassword, "");

			conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserChangePassword), conn.Id(), userName); //DEBUG
			
			return true;

		}

        // -------------------------------------------------------------------------------
        // RequestUserConfirm
        // @Client
        // -------------------------------------------------------------------------------
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
        protected override bool RequestUserConfirm(NetworkConnection conn, string userName, string password, int action=1)
		{
			if (!base.RequestUserConfirm(conn, userName, password))
				return false;

			ClientMessageRequestUserConfirm message = new ClientMessageRequestUserConfirm
			{
				username = userName,
				password = Tools.GenerateHash(userName, password)
			};

			conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserConfirm), conn.Id(), userName); //DEBUG
			
			return true;

		}

        // ======================= PUBLIC METHODS - PLAYER ================================

        // -------------------------------------------------------------------------------
        // RequestPlayerLogin
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Protected override function <c>RequestPlayerLogin</c> that returns a boolean.
        /// Sends a player login request to the server.
        /// Checks whether the player login request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playername"></param>
        /// <param name="username"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        protected override bool RequestPlayerLogin(NetworkConnection conn, string playername, string username)
		{
				
			if (!base.RequestPlayerLogin(conn, playername, username))
				return false;

			ClientMessageRequestPlayerLogin message = new ClientMessageRequestPlayerLogin
			{
				playername = playername,
				username = username
			};
			
			// must be readied here, not in the response - otherwise it generates a warning
			ClientScene.Ready(conn);

			conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestPlayerLogin), conn.Id(), username, playername); //DEBUG
			
			return true;

		}

        // -------------------------------------------------------------------------------
        // RequestPlayerRegister
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Protected override function <c>RequestPlayerRegister</c> that returns a boolean.
        /// Sends a player register request to the server.
        /// Checks whether the player register request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playerName"></param>
        /// <param name="userName"></param>
        /// <param name="prefabName"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        protected override bool RequestPlayerRegister(NetworkConnection conn, string playerName, string userName, string prefabName)
		{
			if (!base.RequestPlayerRegister(conn, playerName, userName, prefabName))
				return false;

			ClientMessageRequestPlayerRegister message = new ClientMessageRequestPlayerRegister
			{
				playername 	= playerName,
				username 	= userName,
				prefabname 	= prefabName
			};

			conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestPlayerRegister), conn.Id(), playerName, prefabName); //DEBUG
			
			return true;

		}

        // -------------------------------------------------------------------------------
        // RequestPlayerDelete
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Protected override function <c>RequestPlayerDelete</c> that returns a boolean.
        /// Sends a player deletion request to the server.
        /// Checks whether the player deletion request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playerName"></param>
        /// <param name="userName"></param>
        /// <param name="action"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        protected override bool RequestPlayerDelete(NetworkConnection conn, string playerName, string userName, int action=1)
		{
			if (!base.RequestPlayerDelete(conn, playerName, userName))
				return false;

			ClientMessageRequestPlayerDelete message = new ClientMessageRequestPlayerDelete
			{
				playername = playerName,
				username = userName
			};

			conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestPlayerDelete), conn.Id(), userName); //DEBUG
			
			return true;

		}

        // -------------------------------------------------------------------------------

    }
}

// =======================================================================================
