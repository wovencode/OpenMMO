//by  Fhiz
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.Collections.Generic;
using SQLite;
using Mirror;

namespace OpenMMO.Database
{

	/// <summary>
	/// This partial section of DatabaseManager is responsible for player related actions.
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	public partial class DatabaseManager
	{
	
		/// <summary>
		/// Hooks into Init and creates the TablePlayer on the database if it does not exist yet.
		/// </summary>
		[DevExtMethods(nameof(Init))]
		void Init_Player()
		{
	   		CreateTable<TablePlayer>();
		}
		
		/// <summary>
		/// Hooks into CreateDefaultDataPlayer. Not used yet.
		/// </summary>
	   	[DevExtMethods(nameof(CreateDefaultDataPlayer))]
		void CreateDefaultDataPlayer_Player(GameObject player)
		{
			
		}
		
		/// <summary>
		/// Hooks into LoadDataPlayerPriority. Not used yet.
		/// </summary>
		[DevExtMethods(nameof(LoadDataPlayerPriority))]
		void LoadDataPlayerPriority_Player(GameObject player)
		{
			
		}
		
	   	/// <summary>
		/// Fetches the table that is present on the local player object instead of copy-pasting all the individual properties, update it and forward it to the database.
		/// </summary>
		[DevExtMethods(nameof(LoadDataPlayer))]
		void LoadDataPlayer_Player(GameObject player)
		{
	   		TablePlayer tablePlayer = FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND deleted=0", player.name);
	   		player.GetComponent<PlayerAccount>()._tablePlayer = tablePlayer;
	   		
	   		player.transform.position = new Vector3(tablePlayer.x, tablePlayer.y, tablePlayer.z);
	   		player.transform.rotation = Quaternion.Euler(0, tablePlayer.roty, 0);
		}
		
	   	/// <summary>
		/// Fetches the table that is present on the local player object instead of copy-pasting individual properties, update it and forward it to the database.
		/// </summary>
		[DevExtMethods(nameof(SaveDataPlayer))]
		void SaveDataPlayer_Player(GameObject player, bool isNew)
		{
			// -- delete all data of this player first, to prevent duplicates
	   		DeleteDataPlayer_Player(player.name);
	   		
	   		// -- times are updated in the tablePlayer.Update function
			player.GetComponent<PlayerAccount>()._tablePlayer.Update(player);
			
			InsertOrReplace(player.GetComponent<PlayerAccount>()._tablePlayer);
		}
		
		/// <summary>
		/// Hooks into LoginPlayer and updates last login time when a player logs in to prevent multi-login
		/// </summary>
	   	[DevExtMethods(nameof(LoginPlayer))]
	   	void LoginPlayer_Player(NetworkConnection conn, GameObject player, string playerName, string userName)
	   	{
	   		Execute("UPDATE "+nameof(TablePlayer)+" SET lastonline=? WHERE playername=?", DateTime.UtcNow, playerName);
	   	}
		
		/// <summary>
		/// Hooks into LogoutPlayer and updates the lastonline time on TablePlayer when the player (= character) logs out.
		/// </summary>
	   	[DevExtMethods(nameof(LogoutPlayer))]
	   	void LogoutPlayer_Player(GameObject player)
	   	{
	   		// -- lastlogin is UtcNow minus 'logoutInterval' to allow immediate login
			DateTime dateTime = DateTime.UtcNow.AddSeconds(-logoutInterval);
	   		Execute("UPDATE "+nameof(TablePlayer)+" SET lastonline=? WHERE playername=?", dateTime, player.name);
	   	}
		
		/// <summary>
		/// Hooks into DeleteDataPlayer and removes all data from TablePlayer when the player (= character) is hard-deleted.
		/// </summary>
	   	[DevExtMethods(nameof(DeleteDataPlayer))]
	   	void DeleteDataPlayer_Player(string playername)
	   	{
	   		Execute("DELETE FROM "+nameof(TablePlayer)+" WHERE playername=?", playername);
	   	}
		
	}

}