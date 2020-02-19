
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;
using Mirror;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{

		// -------------------------------------------------------------------------------
		// Init_User
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(Init))]
		void Init_User()
		{
	   		CreateTable<TableUser>();
		}
		
		// ================================ PLAYER =======================================
		
		// -------------------------------------------------------------------------------
		// LoadDataPlayerPriority_User
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoadDataPlayerPriority))]
		void LoadDataPlayerPriority_User(GameObject player)
		{
		
		}
				
	   	// -------------------------------------------------------------------------------
	   	// LoadDataPlayer_User
	   	// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoadDataPlayer))]
		void LoadDataPlayer_User(GameObject player)
		{
	   	
		}

		// -------------------------------------------------------------------------------
	   	// SaveDataPlayer_User
	   	// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(SaveDataPlayer))]
		void SaveDataPlayer_User(GameObject player, bool isNew)
		{
			// dont update the time on a new player or we log ourselves out of login
			if (isNew) return; 
			
			string userName = player.GetComponent<PlayerComponent>().tablePlayer.username;
			Execute("UPDATE "+nameof(TableUser)+" SET lastonline=?, lastsaved=? WHERE username=?", DateTime.UtcNow, DateTime.UtcNow, userName);

		}
		
		// -------------------------------------------------------------------------------
	   	// LoginPlayer_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LoginPlayer))]
	   	void LoginPlayer_User(NetworkConnection conn, GameObject player, string playerName, string userName)
	   	{
	   		// -- we update lastlogin of user only when a player character logs in (otherwise we lock ourselves out)
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastonline=? WHERE username=?", DateTime.UtcNow, userName);
	   	}
	   	
	   	// -------------------------------------------------------------------------------
	   	// LogoutPlayer_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LogoutPlayer))]
	   	void LogoutPlayer_User(GameObject player)
	   	{
	   		// -- this resets lastlogin to allow immediate re-login
	   		string userName = player.GetComponent<PlayerComponent>().tablePlayer.username;
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastonline=? WHERE username=?", DateTime.MinValue, userName);
	   	}
	   	
	   	// ============================= USER HOOKS ======================================
	   	
		// -------------------------------------------------------------------------------
	   	// SaveDataUser_User
	   	// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(SaveDataUser))]
		void SaveDataUser_User(string username, bool isNew)
		{
		
			// dont update the time on a new player or we log ourselves out of login
			if (isNew) return; 
			
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastonline=?, lastsaved=? WHERE username=?", DateTime.UtcNow, DateTime.UtcNow, username);
		}
		
		// -------------------------------------------------------------------------------
	   	// LoginUser_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LoginUser))]
	   	void LoginUser_User(string username)
	   	{
	   		// -- Note: We do NOT set the lastonline time here as it would lock us out!
	   	}
		
		// -------------------------------------------------------------------------------
	   	// LogoutUser_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LogoutUser))]
	   	void LogoutUser_User(string username)
	   	{
	   		// -- this resets lastlogin to allow immediate re-login
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastonline=? WHERE username=?", DateTime.MinValue, username);
	   	}
		
		// -------------------------------------------------------------------------------
	   	// DeleteDataUser_User
	   	// Note: This one is not called "DeleteDataPlayer" because its the user, not a player
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(DeleteDataUser))]
	   	void DeleteDataUser_User(string username)
	   	{
	   		Execute("DELETE FROM "+nameof(TableUser)+" WHERE username=?", username);
	   	}
	   	
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================