//BY FHIZ
//MODIFIED BY DX4D

// =======================================================================================
// NetworkManager
// by Weaver (Fhiz)
// MIT licensed
//
// This part of the NetworkManager contains all public functions. That comprises all
// methods that are called on the NetworkManager from UI elements in order to check for
// an action or perform an action (like "Can we register an account with password X and
// name Y?" or "Now register an account with password X and name Y").
//
// =======================================================================================

using Mirror;
using OpenMMO;
using OpenMMO.Network;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// NetworkManager
	// ===================================================================================
	public partial class NetworkManager
	{
        // ======================= PUBLIC METHODS - USER =================================

        // -------------------------------------------------------------------------------
        // TryLoginUser
        // @Client
        // we try to login into an existing user using name and password provided
        // -------------------------------------------------------------------------------
        /// <summary>
        ///  Public function <c>TryLoginUser</c>.
        ///  Tries to login the user.
        ///  Runs on the client.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void TryLoginUser(string username, string password)
		{
			
			userName 		= username;
			userPassword 	= password;
			
			//RequestUserLogin(NetworkClient.connection, username, password); //REMOVED - DX4D
			RequestUserLogin(username, password); //ADDED - DX4D
			
		}
		
		// -------------------------------------------------------------------------------
        // TryLogoutUser
        // @Client
        // we try to logout into an existing user on the current connection
        // -------------------------------------------------------------------------------
        public void TryLogoutUser()
		{
			//RequestUserLogout(NetworkClient.connection); //REMOVED - DX4D
            RequestUserLogout(); //ADDED - DX4D
        }
		
        // -------------------------------------------------------------------------------
        // TryRegisterUser
        // @Client
        // we try to register a new User with name and password provided
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryRegisterUser</c>.
        /// Tries to register the user.
        /// Runs on the client.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        public void TryRegisterUser(string username, string password, string email)
		{
			
			userName 		= username;
			userPassword 	= password;
			
			
			//RequestUserRegister(NetworkClient.connection, username, password, email); //REMOVED - DX4D
			RequestUserRegister(username, password, email); //ADDED - DX4D
		}

        // -------------------------------------------------------------------------------
        // TryDeleteUser
        // @Client
        // we try to delete an existing User according to its name and password
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryDeleteUser</c>.
        /// Tries to delete the user according to its username and password.
        /// Runs on the client.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void TryDeleteUser(string username, string password)
		{
			
			userName 		= username;
			userPassword 	= password;

            //RequestUserDelete(NetworkClient.connection, username, password); //REMOVED - DX4D
            RequestUserDelete(username, password); //ADDED - DX4D

        }

        // -------------------------------------------------------------------------------
        // TryChangePasswordUser
        // @Client
        // we try to delete an existing User according to its name and password
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryDeleteUser</c>.
        /// Tries to delete an existing User according to its name and password.
        /// Runs on the client.  
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        public void TryChangePasswordUser(string username, string oldpassword, string newpassword)
		{
			
			userName 		= username;
			userPassword 	= oldpassword;
			newPassword 	= newpassword;

            //RequestUserChangePassword(NetworkClient.connection, username, oldpassword, newpassword); //REMOVED - DX4D
            RequestUserChangePassword(username, oldpassword, newpassword); //ADDED - DX4D

		}

        // -------------------------------------------------------------------------------
        // TryConfirmUser
        // @Client
        // we try to confirm an existing User according to its name and password
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryConfirmUser</c>.
        /// Tries to confirm an existing User according to its name and password.
        /// Runs on the client. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void TryConfirmUser(string username, string password)
		{
            //RequestUserConfirm(NetworkClient.connection, username, password); //REMOVED - DX4D
            RequestUserConfirm(username, password); //ADDED - DX4D
        }

        // ======================= PUBLIC METHODS - PLAYER ===============================

        // -------------------------------------------------------------------------------
        // TryLoginPlayer
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryLoginPlayer</c>.
        /// Tries to login the player.
        /// Runs on the client. 
        /// </summary>
        /// <param name="username"></param>
        public void TryLoginPlayer(string playername)
		{
			//RequestPlayerLogin(NetworkClient.connection, username, userName); //REMOVED - DX4D
            RequestPlayerLogin(playername, userName); //ADDED - DX4D
        }

        // -------------------------------------------------------------------------------
        // TryRegisterPlayer
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryRegisterPlayer</c>.
        /// Tries to register the player.
        /// Runs on the client.
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="prefabName"></param>
        public void TryRegisterPlayer(string playerName, string prefabName)
		{
			//RequestPlayerRegister(NetworkClient.connection, playerName, userName, prefabName); //REMOVED - DX4D
			RequestPlayerRegister(playerName, userName, prefabName); //ADDED - DX4D
		}

        // -------------------------------------------------------------------------------
        // TryDeletePlayer
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryDeletePlayer</c>.
        /// Tries to delete the player.
        /// Runs on the client.
        /// </summary>
        /// <param name="playerName"></param>
        public void TryDeletePlayer(string playerName)
		{			
			//if (RequestPlayerDelete(NetworkClient.connection, playerName, userName)) //REMOVED - DX4D
            if (RequestPlayerDelete(playerName, userName)) //ADDED - DX4D
			{
				for (int i = 0; i < playerPreviews.Count; i++)
					if (playerPreviews[i].name == playerName)
						playerPreviews.RemoveAt(i);
			}
		}

        // ======================== PUBLIC METHODS - OTHER ===============================

        // -------------------------------------------------------------------------------
        // TryStartServer
        // @Client
        // we try to start a server (host only) if possible
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryStartServer</c>.
        /// Tries to start the server.
        /// Occurs only when running a host and play topology.
        /// Runs on the client.
        /// </summary>
        public void TryStartServer()
		{
			if (!CanStartServer()) return;
			StartServer();
		}

        // -------------------------------------------------------------------------------
        // TryCancel
        // @Client
        // we try to cancel whatever we are doing right now
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>TryCancel</c>.
        /// Tries to cancel what they are doing right now.
        /// Runs on the client.
        /// </summary>
        public void TryCancel()
		{
			StopClient();
		}
		
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================