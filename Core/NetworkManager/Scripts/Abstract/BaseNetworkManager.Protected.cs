
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
		
		protected abstract GameObject GetPlayerPrefab(string playerName);
		
    	// ======================= PUBLIC METHODS - USER =================================
    	
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserLogin(NetworkConnection conn, string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserRegister(NetworkConnection conn, string name, string password, string usermail)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserDelete(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserChangePassword(NetworkConnection conn, string name, string oldpassword, string newpassword)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(oldpassword) && Tools.IsAllowedPassword(newpassword) && oldpassword != newpassword);
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserConfirm(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// ======================= PUBLIC METHODS - PLAYER =================================
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerLogin(NetworkConnection conn, string name, string username)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerRegister(NetworkConnection conn, string name, string username, string prefabname)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username) && !String.IsNullOrWhiteSpace(prefabname));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerDelete(NetworkConnection conn, string name, string username, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username));
		}
				
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerSwitchServer(NetworkConnection conn, string name, int _token)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================