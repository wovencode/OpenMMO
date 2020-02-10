
using OpenMMO;
using OpenMMO.Network;
using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// BaseNetworkManager
	// ===================================================================================
	public abstract partial class BaseNetworkManager
	{
		
		// ====================== PUBLIC METHODS - GENERAL ===============================
        /// <summary>
        /// Public abstract general Method <c>GetPlayerPrefab</c> that can be overwritten to load the player prefab using the player prefab name
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns> The player prefab gameobject </returns>        
		protected abstract GameObject GetPlayerPrefab(string playerName);
		
    	// ======================= PUBLIC METHODS - USER =================================
    	
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserLogin</c> that requests the login for a user and returns a boolean value. 
        /// The base version fo the method checks whether the name is allowed and the password is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns> A boolean value determining whether the user can login </returns>
		protected virtual bool RequestUserLogin(NetworkConnection conn, string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserRegister</c> requests whether the user can regsiter and returns a boolean value. 
        /// The base version fo the method checks whether the name is allowed and the password is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="usermail"></param>
        /// <returns> A boolean value determining whether the user can register </returns>
        protected virtual bool RequestUserRegister(NetworkConnection conn, string name, string password, string usermail)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserDelete</c> requests whether the user can be deleted. 
        /// The base version fo the method checks whether the name is allowed and the password is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="_action"></param>
        /// <returns> A boolean value determining whether the user can deleted their account </returns>
        protected virtual bool RequestUserDelete(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserChangePassword</c> requests whether the user can change their password. 
        /// The base version fo the method checks whether the name is allowed, the passwords (bot old and new) are allowed and if the old and new password are different.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        /// <returns> A boolean value determining whether the user can change their password </returns>
        protected virtual bool RequestUserChangePassword(NetworkConnection conn, string name, string oldpassword, string newpassword)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(oldpassword) && Tools.IsAllowedPassword(newpassword) && oldpassword != newpassword);
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual user method <c>RequestUserConfirm</c>. 
        /// The base version fo the method checks whether the name is allowed and the password is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="_action"></param>
        /// <returns> A boolean value determining whether the user request has been confirmed </returns>
        protected virtual bool RequestUserConfirm(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}

        // ======================= PUBLIC METHODS - PLAYER =================================

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual player method <c>RequestPlayerLogin</c> that request the player login.
        /// The base version fo the method checks whether the name is allowed and the username is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <returns> A boolean value determing whether the player can login </returns>
        protected virtual bool RequestPlayerLogin(NetworkConnection conn, string name, string username)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual player method <c>RequestPlayerRegister</c> that registers the player
        /// The base version fo the method checks whether the name is allowed, the username is allowed and if a player prefab name is passed
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <param name="prefabname"></param>
        /// <returns> A boolean value determining whether the player can register </returns>
        protected virtual bool RequestPlayerRegister(NetworkConnection conn, string name, string username, string prefabname)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username) && !String.IsNullOrWhiteSpace(prefabname));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        ///  Public virtual player method <c>RequestPlayerDelete</c> that deletes the player
        /// The base version fo the method checks whether the name is allowed and the username is allowed.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <param name="_action"></param>
        /// <returns> A boolean value determining whether the player has been deleted </returns>
        protected virtual bool RequestPlayerDelete(NetworkConnection conn, string name, string username, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username));
		}

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public virtual player method <c>RequestPlayerSwitchServer</c> that switches the players server
        /// The base version fo the method checks whether the player name is allowed, the anchor name is given and the zone name is given.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playerName"></param>
        /// <param name="anchorName"></param>
        /// <param name="zoneName"></param>
        /// <returns> A boolean value determining whehter the player can switch server </returns>
        protected virtual bool RequestPlayerSwitchServer(NetworkConnection conn, string playerName, string anchorName, string zoneName)
		{
			return (Tools.IsAllowedName(name) && !String.IsNullOrWhiteSpace(anchorName) && !String.IsNullOrWhiteSpace(zoneName));
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================