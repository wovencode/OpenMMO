
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
		
		// ============================ PROTECTED METHODS ================================
		
		// -------------------------------------------------------------------------------
		// PlayerSetDeleted
		// Sets the player to deleted (1) or undeletes it (0)
		// -------------------------------------------------------------------------------
		protected void PlayerSetDeleted(string playername, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TablePlayer)+" SET deleted=? WHERE playername=?", action == DatabaseAction.Do ? 1 : 0, playername);
		}
		
		// -------------------------------------------------------------------------------
		// PlayerSetBanned
		// Bans (1) or unbans (0) the user
		// -------------------------------------------------------------------------------
		protected void PlayerSetBanned(string playername, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TablePlayer)+" SET banned=? WHERE playername=?", action == DatabaseAction.Do ? 1 : 0, playername);
		}
		
		// -------------------------------------------------------------------------------
		// DeleteDataPlayer
		// Permanently deletes the player and all of its data (hard delete)
		// -------------------------------------------------------------------------------
		protected void DeleteDataPlayer(string playername)
		{			
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataPlayer), playername); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// PlayerValid
		// Checks if a player exists using playername and username combination
		// -------------------------------------------------------------------------------
		public bool PlayerValid(string playername, string username)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND username=? AND banned=0 AND deleted=0", playername, username) != null;
		}
		
		// -------------------------------------------------------------------------------
		// PlayerExists
		// Checks if a player exists, using only the playername
		// -------------------------------------------------------------------------------
		public bool PlayerExists(string playername)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=?", playername) != null;
		}
		
		// -------------------------------------------------------------------------------
		// PlayerExists
		// -------------------------------------------------------------------------------
		public bool PlayerExists(string playername, string username)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND username=?", playername, username) != null;
		}
		
		// -------------------------------------------------------------------------------
		// GetPlayerCount
		// returns the number of players registered on this user account
		// -------------------------------------------------------------------------------
		protected int GetPlayerCount(string username)
		{
			List<TablePlayer> result =  Query<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE username=? AND deleted=0", username);
			
			if (result == null)
				return 0;
			else
				return result.Count;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================