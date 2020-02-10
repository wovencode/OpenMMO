
using OpenMMO;
using OpenMMO.Network;
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
        /// Protected override function <c>RequestUserLogin</c> function that returns a boolean.
        /// Sends a user login request to the server.
        /// Checks whether the login request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns> Returns a boolean detailing whetherthe request was sent to the server. </returns>
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

			return true;

		}

		// -------------------------------------------------------------------------------
        // RequestUserRegister
        // @Client
		// -------------------------------------------------------------------------------
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

			return true;

		}

		// -------------------------------------------------------------------------------
        // RequestUserDelete
        // @Client
		// -------------------------------------------------------------------------------
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

			return true;

		}

		// -------------------------------------------------------------------------------
        // RequestUserChangePassword
        // @Client
		// -------------------------------------------------------------------------------
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

			return true;

		}

		// -------------------------------------------------------------------------------
        // RequestUserConfirm
        // @Client
		// -------------------------------------------------------------------------------
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

			return true;

		}

		// ======================= PUBLIC METHODS - PLAYER ================================

        // -------------------------------------------------------------------------------
        // RequestPlayerLogin
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestPlayerLogin(NetworkConnection conn, string playername, string username)
		{
			if (!base.RequestPlayerLogin(conn, playername, username))
				return false;

			ClientMessageRequestPlayerLogin message = new ClientMessageRequestPlayerLogin
			{
				playername = playername,
				username = username
			};

			ClientScene.Ready(conn);

			conn.Send(message);

			return true;

		}

		// -------------------------------------------------------------------------------
        // RequestPlayerRegister
        // @Client
		// -------------------------------------------------------------------------------
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

			return true;

		}

		// -------------------------------------------------------------------------------
        // RequestPlayerDelete
        // @Client
		// -------------------------------------------------------------------------------
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

			return true;

		}

		// -------------------------------------------------------------------------------
        // RequestPlayerSwitchServer
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestPlayerSwitchServer(NetworkConnection conn, string playerName, string anchorName, string zoneName)
		{

			if (!base.RequestPlayerSwitchServer(conn, playerName, anchorName, zoneName))
				return false;

			ClientMessageRequestPlayerSwitchServer message = new ClientMessageRequestPlayerSwitchServer
			{
				playername = playerName,
				zonename = zoneName
			};

			conn.Send(message);

			return true;

		}

        // -------------------------------------------------------------------------------

    }
}

// =======================================================================================
