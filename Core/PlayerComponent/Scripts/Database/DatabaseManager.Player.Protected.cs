
using Wovencode;
using Wovencode.Database;
using UnityEngine;
using System;
using System.Collections.Generic;
using SQLite;

namespace Wovencode.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// ============================ PROTECTED METHODS ================================
		
		// -------------------------------------------------------------------------------
		// PlayerSetOnline
		// Sets the player online (1) or offline (0) and updates last login time
		// -------------------------------------------------------------------------------
		protected void PlayerSetOnline(string playername, int action=1)
		{
			Execute("UPDATE "+nameof(TablePlayer)+" SET online=?, lastlogin=? WHERE playername=?", action, DateTime.UtcNow, playername);
		}
		
		// -------------------------------------------------------------------------------
		// PlayerSetDeleted
		// Sets the player to deleted (1) or undeletes it (0)
		// -------------------------------------------------------------------------------
		protected void PlayerSetDeleted(string playername, int action=1)
		{
			Execute("UPDATE "+nameof(TablePlayer)+" SET deleted=? WHERE playername=?", action, playername);
		}
		
		// -------------------------------------------------------------------------------
		// PlayerSetBanned
		// Bans (1) or unbans (0) the user
		// -------------------------------------------------------------------------------
		protected void PlayerSetBanned(string playername, int action=1)
		{
			Execute("UPDATE "+nameof(TablePlayer)+" SET banned=? WHERE playername=?", action, playername);
		}
		
		// -------------------------------------------------------------------------------
		// DeleteDataPlayer
		// Permanently deletes the player and all of its data (hard delete)
		// -------------------------------------------------------------------------------
		protected void DeleteDataPlayer(string playername)
		{			
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataPlayer), playername);
		}
		
		// -------------------------------------------------------------------------------
		// PlayerValid
		// -------------------------------------------------------------------------------
		public bool PlayerValid(string playername, string username)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND username=? AND banned=0 AND deleted=0", playername, username) != null;
		}
		
		// -------------------------------------------------------------------------------
		// PlayerExists
		// -------------------------------------------------------------------------------
		public bool PlayerExists(string playername, string username)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND username=?", playername, username) != null;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================