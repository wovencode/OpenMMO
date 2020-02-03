
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// ============================= PRIVATE METHODS =================================
		
		// -------------------------------------------------------------------------------
		// Init_User
		// -------------------------------------------------------------------------------
		[DevExtMethods("Init")]
		void Init_User()
		{
	   		CreateTable<TableUser>();
		}
		
	   	// -------------------------------------------------------------------------------
	   	// CreateDefaultDataUser_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("CreateDefaultDataUser")]
		void CreateDefaultDataUser_User(GameObject player)
		{
			/*
				users have no default data, feel free to add your own
				
				instead, user data is saved/loaded as part of the register/login process
			*/
		}
		
		// -------------------------------------------------------------------------------
		// LoadDataWithPriority_User
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadDataPlayerPriority")]
		void LoadDataPlayerPriority_User(GameObject player)
		{
			/*
				users do not load priority data, feel free to add your own
				
				instead, user data is saved/loaded as part of the register/login process
			*/
		}
		
	   	// -------------------------------------------------------------------------------
	   	// LoadDataPlayer_User
	   	// -------------------------------------------------------------------------------
		[DevExtMethods("LoadDataPlayer")]
		void LoadDataPlayer_User(GameObject player)
		{
	   		/*
				users do not load any data, feel free to add your own
				
				instead, user data is saved/loaded as part of the register/login process
			*/
		}
		
		// -------------------------------------------------------------------------------
	   	// LoadDataUser_User
	   	// we simply fetch the table that is present on the local player object instead
	   	// of copy-pasting all the individual properties, update it and forward it to the db
	   	// -------------------------------------------------------------------------------
		[DevExtMethods("LoadDataUser")]
		void LoadDataUser_User(GameObject player)
		{
	   	/*
				users do not load any data, feel free to add your own
				
				instead, user data is saved/loaded as part of the register/login process
			*/
		}
		
		// -------------------------------------------------------------------------------
	   	// SaveDataPlayer_User
	   	// -------------------------------------------------------------------------------
		[DevExtMethods("SaveDataPlayer")]
		void SaveDataPlayer_User(GameObject player, bool isOnline)
		{
	   		/*
				users do not save any player data, feel free to add your own
			*/
		}
		
	   	// -------------------------------------------------------------------------------
	   	// SaveDataUser_User
	   	// we simply fetch the table that is present on the local player object instead
	   	// of copy-pasting all the individual properties, update it and forward it to the db
	   	// -------------------------------------------------------------------------------
		[DevExtMethods("SaveDataUser")]
		void SaveDataUser_User(string username, bool isOnline)
		{
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastsaved=?, online=? WHERE username=?", DateTime.UtcNow, isOnline.ToInt(), username);
		}
		
		// -------------------------------------------------------------------------------
	   	// LoginUser_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("LoginUser")]
	   	void LoginUser_User(string username)
	   	{
	   		UserSetOnline(username, 1);
	   	}
		
		// -------------------------------------------------------------------------------
	   	// LogoutUser_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("LogoutUser")]
	   	void LogoutUser_User(string username)
	   	{
	   		UserSetOnline(username, 0);
	   	}
		
		// -------------------------------------------------------------------------------
	   	// DeleteDataUser_User
	   	// Note: This one is not called "DeleteDataPlayer" because its the user, not a player
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("DeleteDataUser")]
	   	void DeleteDataUser_User(string name)
	   	{
	   		Execute("DELETE FROM "+nameof(TableUser)+" WHERE username=?", name);
	   	}
	   	
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================