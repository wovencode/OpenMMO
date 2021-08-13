//by  Fhiz
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{

	/// <summary>
	/// This partial section of DatabaseManager is responsible for player (= character) related actions.
	/// </summary>
	public partial class DatabaseManager
	{
		
		/// <summary>
		/// Sets the player (= character) to deleted (1) or undeletes it (0).
		/// </summary>
		protected void PlayerSetDeleted(string playername, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TablePlayer)+" SET deleted=? WHERE playername=?", action == DatabaseAction.Do ? 1 : 0, playername);
		}
		
		/// <summary>
		/// Bans (1) or unbans (0) the player (= character).
		/// </summary>
		protected void PlayerSetBanned(string playername, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TablePlayer)+" SET banned=? WHERE playername=?", action == DatabaseAction.Do ? 1 : 0, playername);
		}
		
		/// <summary>
		/// Permanently deletes the player and all of its data (hard delete)
		/// </summary>
		protected void DeleteDataPlayer(string playername)
		{			
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataPlayer), playername); //HOOK
		}
		
		/// <summary>
		/// Checks if a player exists using playername and username combination
		/// </summary>
		public bool PlayerValid(string playername, string username)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND username=? AND banned=0 AND deleted=0", playername, username) != null;
		}
		
		/// <summary>
		/// Checks if a player exists, using only the playername
		/// </summary>
		public bool PlayerExists(string playername)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=?", playername) != null;
		}
		
		/// <summary>
		/// Checks if a player (= character) exists on this user (= account).
		/// </summary>
		public bool PlayerExists(string playername, string username)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND username=?", playername, username) != null;
		}
		
		/// <summary>
		/// Returns the number of players registered on this user account
		/// </summary>
		protected int GetPlayerCount(string username)
		{
			List<TablePlayer> result =  Query<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE username=? AND deleted=0", username);
			
			if (result == null)
				return 0;
			else
				return result.Count;
		}
		
	}

}