
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
		protected virtual bool RequestUserLogin(NetworkConnection conn, string username, string password)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserRegister(NetworkConnection conn, string username, string password, string usermail)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserDelete(NetworkConnection conn, string username, string password, int _action=1)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserChangePassword(NetworkConnection conn, string username, string oldpassword, string newpassword)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(oldpassword) && Tools.IsAllowedPassword(newpassword) && oldpassword != newpassword);
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserConfirm(NetworkConnection conn, string username, string password, int _action=1)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(password));
		}
		
		// ======================= PUBLIC METHODS - PLAYER =================================
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerLogin(NetworkConnection conn, string playername, string username)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerRegister(NetworkConnection conn, string playername, string username, string prefabname)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username) && !String.IsNullOrWhiteSpace(prefabname));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerDelete(NetworkConnection conn, string playername, string username, int _action=1)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username));
		}
				
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerSwitchServer(NetworkConnection conn, string playerName, string anchorName, string zoneName)
		{
			return (Tools.IsAllowedName(name) && !String.IsNullOrWhiteSpace(anchorName) && !String.IsNullOrWhiteSpace(zoneName));
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================