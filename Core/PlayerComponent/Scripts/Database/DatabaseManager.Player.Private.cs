
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
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
		// Init_Player
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(Init))]
		void Init_Player()
		{
	   		CreateTable<TablePlayer>();
		}
		
	   	// -------------------------------------------------------------------------------
	   	// CreateDefaultDataPlayer_Player
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(CreateDefaultDataPlayer))]
		void CreateDefaultDataPlayer_Player(GameObject player)
		{
			
		}
		
		// -------------------------------------------------------------------------------
		// LoadDataPlayerPriority_Player
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoadDataPlayerPriority))]
		void LoadDataPlayerPriority_Player(GameObject player)
		{
			
		}
		
	   	// -------------------------------------------------------------------------------
	   	// LoadDataPlayer_Player
	   	// we simply fetch the table that is present on the local player object instead
	   	// of copy-pasting all the individual properties, update it and forward it to the db
	   	// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoadDataPlayer))]
		void LoadDataPlayer_Player(GameObject player)
		{
	   		TablePlayer tablePlayer = FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND deleted=0", player.name);
	   		player.GetComponent<PlayerComponent>().tablePlayer = tablePlayer;
	   		player.transform.position = new Vector3(tablePlayer.x, tablePlayer.y, tablePlayer.z);
		}
		
	   	// -------------------------------------------------------------------------------
	   	// SaveDataPlayer_Player
	   	// we simply fetch the table that is present on the local player object instead
	   	// of copy-pasting all the individual properties, update it and forward it to the db
	   	// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(SaveDataPlayer))]
		void SaveDataPlayer_Player(GameObject player, bool isNew)
		{
			// you should delete all data of this player first, to prevent duplicates
	   		DeleteDataPlayer_Player(player.name);
	   		
	   		// -- times are updated in the tablePlayer.Update function
			player.GetComponent<PlayerComponent>().tablePlayer.Update(player);
			
			InsertOrReplace(player.GetComponent<PlayerComponent>().tablePlayer);
		}
		
		// -------------------------------------------------------------------------------
	   	// LoginPlayer_Player
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LoginPlayer))]
	   	void LoginPlayer_Player(string playername, string username)
	   	{
	   		Execute("UPDATE "+nameof(TablePlayer)+" SET lastonline=? WHERE playername=?", DateTime.UtcNow, playername);
	   	}
		
		// -------------------------------------------------------------------------------
	   	// LogoutPlayer_Player
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LogoutPlayer))]
	   	void LogoutPlayer_Player(string playername)
	   	{
	   		// -- this resets lastlogin to allow immediate re-login
	   		Execute("UPDATE "+nameof(TablePlayer)+" SET lastonline=? WHERE playername=?", DateTime.UtcNow, playername);
	   	}
		
		// -------------------------------------------------------------------------------
	   	// DeleteDataPlayer_Player
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(DeleteDataPlayer))]
	   	void DeleteDataPlayer_Player(string playername)
	   	{
	   		Execute("DELETE FROM "+nameof(TablePlayer)+" WHERE playername=?", playername);
	   	}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================