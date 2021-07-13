//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;
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
		
		// -------------------------------------------------------------------------------
        // RequestUserLogout
        // @Client
		// -------------------------------------------------------------------------------

        //protected override bool RequestUserLogout(NetworkConnection conn) //REMOVED - DX4D
        protected override bool RequestUserLogout() //ADDED - DX4D
		{
		
			ClientRequestUserLogout message = new ClientRequestUserLogout
			{

			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

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
        //protected override bool RequestUserRegister(NetworkConnection conn, string userName, string password, string usermail) //REMOVED - DX4D
        protected override bool RequestUserRegister(string userName, string password, string usermail) //ADDED - DX4D
        {
            //if (!base.RequestUserRegister(conn, userName, password, usermail)) //REMOVED - DX4D
            if (!base.RequestUserRegister(userName, password, usermail)) //ADDED - DX4D
                return false;

			ClientRequestUserRegister message = new ClientRequestUserRegister
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
        //protected override bool RequestUserDelete(NetworkConnection conn, string userName, string password, int action=1) //REMOVED - DX4D
        protected override bool RequestUserDelete(string userName, string password, int action=1) //ADDED - DX4D
        {
            //if (!base.RequestUserDelete(conn, userName, password)) //REMOVED - DX4D
            if (!base.RequestUserDelete(userName, password)) //ADDED - DX4D
                return false;

			ClientRequestUserDelete message = new ClientRequestUserDelete
			{
				username = userName,
				password = Tools.GenerateHash(name, password)
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

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
        //protected override bool RequestUserConfirm(NetworkConnection conn, string userName, string password, int action=1) //REMOVED - DX4D
        protected override bool RequestUserConfirm(string userName, string password, int action=1) //ADDED - DX4D
        {
			//if (!base.RequestUserConfirm(conn, userName, password)) //REMOVED - DX4D
			if (!base.RequestUserConfirm(userName, password)) //ADDED - DX4D
				return false;

			ClientRequestUserConfirm message = new ClientRequestUserConfirm
			{
				username = userName,
				password = Tools.GenerateHash(userName, password)
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

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
        //protected override bool RequestPlayerLogin(NetworkConnection conn, string playername, string username) //REMOVED - DX4D
        protected override bool RequestPlayerLogin(string playername, string username) //ADDED - DX4D
        {

            //if (!base.RequestPlayerLogin(conn, playername, username)) //REMOVED - DX4D
            if (!base.RequestPlayerLogin(playername, username)) //ADDED - DX4D
                return false;

			ClientRequestPlayerLogin message = new ClientRequestPlayerLogin
			{
				playername = playername,
				username = username
			};
			
			// must be readied here, not in the response - otherwise it generates a warning
			//ClientScene.Ready(conn); //REMOVED - DX4D
			NetworkClient.Ready(); //ADDED - DX4D

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

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
        //protected override bool RequestPlayerRegister(NetworkConnection conn, string playerName, string userName, string prefabName) //REMOVED - DX4D
        protected override bool RequestPlayerRegister(string playerName, string userName, string prefabName) //ADDED - DX4D
        {
            //if (!base.RequestPlayerRegister(conn, playerName, userName, prefabName)) //REMOVED - DX4D
            if (!base.RequestPlayerRegister(playerName, userName, prefabName)) //ADDED - DX4D
                return false;

			ClientRequestPlayerRegister message = new ClientRequestPlayerRegister
			{
				playername 	= playerName,
				username 	= userName,
				prefabname 	= prefabName
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

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
        //protected override bool RequestPlayerDelete(NetworkConnection conn, string playerName, string userName, int action=1) //REMOVED - DX4D
        protected override bool RequestPlayerDelete(string playerName, string userName, int action=1) //ADDED - DX4D
        {
            //if (!base.RequestPlayerDelete(conn, playerName, userName)) //REMOVED - DX4D
            if (!base.RequestPlayerDelete(playerName, userName)) //ADDED - DX4D
                return false;

			ClientRequestPlayerDelete message = new ClientRequestPlayerDelete
			{
				playername = playerName,
				username = userName
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestPlayerDelete), conn.Id(), userName); //DEBUG
			
			return true;

		}

        // -------------------------------------------------------------------------------

    }
}

// =======================================================================================
