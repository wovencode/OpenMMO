
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
		[DevExtMethods(nameof(Init))]
		void Init_User()
		{
	   		CreateTable<TableUser>();
		}
		
		// -------------------------------------------------------------------------------
		// LoadDataWithPriority_User
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoadDataPlayerPriority))]
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
		[DevExtMethods(nameof(LoadDataPlayer))]
		void LoadDataPlayer_User(GameObject player)
		{
	   		/*
				users do not load any data, feel free to add your own
				
				instead, user data is saved/loaded as part of the register/login process
			*/
		}

		// -------------------------------------------------------------------------------
	   	// SaveDataPlayer_User
	   	// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(SaveDataPlayer))]
		void SaveDataPlayer_User(GameObject player)
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
		[DevExtMethods(nameof(SaveDataUser))]
		void SaveDataUser_User(string username)
		{
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastsaved=? WHERE username=?", DateTime.UtcNow, username);
		}
		
		// -------------------------------------------------------------------------------
	   	// LoginUser_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LoginUser))]
	   	void LoginUser_User(string username)
	   	{
	   		
	   	}
		
		// -------------------------------------------------------------------------------
	   	// LogoutUser_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LogoutUser))]
	   	void LogoutUser_User(string username)
	   	{
	   		
	   	}
		
		// -------------------------------------------------------------------------------
	   	// DeleteDataUser_User
	   	// Note: This one is not called "DeleteDataPlayer" because its the user, not a player
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(DeleteDataUser))]
	   	void DeleteDataUser_User(string name)
	   	{
	   		Execute("DELETE FROM "+nameof(TableUser)+" WHERE username=?", name);
	   	}
	   	
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================