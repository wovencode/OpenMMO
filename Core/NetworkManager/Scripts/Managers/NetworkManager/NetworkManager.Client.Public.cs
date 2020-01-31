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

using Wovencode;
using Wovencode.Network;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Wovencode.Network
{
	
	// ===================================================================================
	// NetworkManager
	// ===================================================================================
	public partial class NetworkManager
	{
   
   		// ======================= PUBLIC METHODS - USER =================================
   		
   		// -------------------------------------------------------------------------------
   		// CanClick
   		// @Client
   		// can any network related button be clicked at the moment?
   		// -------------------------------------------------------------------------------
		public bool CanClick()
		{
			return (isNetworkActive && IsConnecting());
		}
		
   		// -------------------------------------------------------------------------------
   		// CanCancel
   		// @Client
   		// can we cancel what we are currently doing?
   		// -------------------------------------------------------------------------------
		public bool CanCancel()
		{
			return IsConnecting();
		}
   		
		// -------------------------------------------------------------------------------
   		// CanLoginUser
   		// @Client
   		// can we login into an existing user with the provided name and password?
   		// -------------------------------------------------------------------------------
		public bool CanLoginUser(string name, string password)
		{
			return isNetworkActive && 
				Tools.IsAllowedName(name) && 
				Tools.IsAllowedPassword(password) &&
				IsConnecting();
		}

		// -------------------------------------------------------------------------------
   		// CanRegisterUser
   		// @Client
   		// can we register a new user with the provided name and password?
   		// -------------------------------------------------------------------------------
		public bool CanRegisterUser(string name, string password)
		{
			return isNetworkActive &&
				Tools.IsAllowedName(name) && 
				Tools.IsAllowedPassword(password) &&
				IsConnecting();
		}

		// -------------------------------------------------------------------------------
   		// CanDeleteUser
   		// @Client
   		// can we delete an user with the provided name and password?
   		// -------------------------------------------------------------------------------
		public bool CanDeleteUser(string name, string password)
		{
			return isNetworkActive &&
				Tools.IsAllowedName(name) && 
				Tools.IsAllowedPassword(password) &&
				IsConnecting();
		}
		
		// -------------------------------------------------------------------------------
   		// CanChangePasswordUser
   		// @Client
   		// can we change the provided users password?
   		// -------------------------------------------------------------------------------
		public bool CanChangePasswordUser(string name, string oldpassword, string newpassword)
		{
			return isNetworkActive &&
				Tools.IsAllowedName(name) && 
				Tools.IsAllowedPassword(oldpassword) &&
				Tools.IsAllowedPassword(newpassword) &&
				IsConnecting();
		}
		
		// -------------------------------------------------------------------------------
   		// CanStartServer
   		// @Client
   		// can we start a server (host only) right now?
   		// -------------------------------------------------------------------------------
		public bool CanStartServer()
		{
			return (Application.platform != RuntimePlatform.WebGLPlayer && 
					!isNetworkActive &&
					!IsConnecting());
		}

		// ======================= PUBLIC METHODS - USER =================================

		// -------------------------------------------------------------------------------
   		// TryLoginUser
   		// @Client
   		// we try to login into an existing user using name and password provided
   		// -------------------------------------------------------------------------------
		public void TryLoginUser(string name, string password)
		{
			
			userName 		= name;
			userPassword 	= password;
			
			RequestUserLogin(NetworkClient.connection, name, password);
			
		}
		
		// -------------------------------------------------------------------------------
   		// TryRegisterUser
   		// @Client
   		// we try to register a new User with name and password provided
   		// -------------------------------------------------------------------------------
		public void TryRegisterUser(string name, string password, string email)
		{
			
			userName 		= name;
			userPassword 	= password;
			
			
			RequestUserRegister(NetworkClient.connection, name, password, email);
		}
		
		// -------------------------------------------------------------------------------
   		// TryDeleteUser
   		// @Client
   		// we try to delete an existing User according to its name and password
   		// -------------------------------------------------------------------------------
		public void TryDeleteUser(string name, string password)
		{
			
			userName 		= name;
			userPassword 	= password;
			
			RequestUserDelete(NetworkClient.connection, name, password);

		}
		
		// -------------------------------------------------------------------------------
   		// TryChangePasswordUser
   		// @Client
   		// we try to delete an existing User according to its name and password
   		// -------------------------------------------------------------------------------
		public void TryChangePasswordUser(string name, string oldpassword, string newpassword)
		{
			
			userName 		= name;
			userPassword 	= oldpassword;
			newPassword 	= newpassword;
			
			RequestUserChangePassword(NetworkClient.connection, name, oldpassword, newpassword);

		}
		
		// -------------------------------------------------------------------------------
   		// TryConfirmUser
   		// @Client
   		// we try to confirm an existing User according to its name and password
   		// -------------------------------------------------------------------------------
		public void TryConfirmUser(string name, string password)
		{
			RequestUserConfirm(NetworkClient.connection, name, password);
		}
		
		// ======================= PUBLIC METHODS - PLAYER ===============================

		// -------------------------------------------------------------------------------
   		// TryLoginPlayer
   		// @Client
   		// -------------------------------------------------------------------------------
		public void TryLoginPlayer(string name)
		{
			RequestPlayerLogin(NetworkClient.connection, name, userName);
		}
		
		// -------------------------------------------------------------------------------
   		// TryRegisterPlayer
   		// @Client
   		// -------------------------------------------------------------------------------
		public void TryRegisterPlayer(string playerName, string prefabName)
		{
			if (RequestPlayerRegister(NetworkClient.connection, playerName, userName, prefabName))
			{
				playerPreviews.Add(new PlayerPreview{name=playerName});
			}
		}
		
		// -------------------------------------------------------------------------------
   		// TryDeletePlayer
   		// @Client
   		// -------------------------------------------------------------------------------
		public void TryDeletePlayer(string playerName)
		{			
			if (RequestPlayerDelete(NetworkClient.connection, playerName, userName))
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
		public void TryCancel()
		{
			StopClient();
		}
		
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================