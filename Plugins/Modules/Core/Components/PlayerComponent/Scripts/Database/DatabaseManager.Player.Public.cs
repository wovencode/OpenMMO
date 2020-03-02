
using System;

namespace OpenMMO.Database
{

	/// <summary>
	/// 
	/// </summary>
	public partial class DatabaseManager
	{
		
		// -------------------------------------------------------------------------------
		// GetPlayerPrefabName
		// -------------------------------------------------------------------------------
		public string GetPlayerPrefabName(string playername)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=?", playername).prefab;
		}
		
		// -------------------------------------------------------------------------------
		// GetPlayerOnline
		// Checks if a player is online anywhere using 'lastonline' time frame
		// -------------------------------------------------------------------------------
		public bool GetPlayerOnline(string playerName)
		{
			TablePlayer tablePlayer = FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND banned=0 AND deleted=0", playerName);
			
			if (tablePlayer == null)
				return false;
			else
			{
				DateTime dateTime = tablePlayer.lastonline.AddSeconds(logoutInterval);
                return DateTime.Compare(DateTime.UtcNow, dateTime) <= 0;
			}
			
		}
		
		// ============================== PUBLIC METHODS =================================
		
		// -------------------------------------------------------------------------------
		// TryPlayerLogin
		// -------------------------------------------------------------------------------
		public override bool TryPlayerLogin(string playername, string username)
		{
			
			if (!base.TryPlayerLogin(playername, username) || !PlayerValid(playername, username))
				return false;
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
		// TryPlayerRegister
		// -------------------------------------------------------------------------------
		public override bool TryPlayerRegister(string name, string username, string prefabname)
		{
		
			if (!base.TryPlayerRegister(name, username, prefabname) || PlayerExists(name))
				return false;
			
			// -- check if maximum amount of characters per account reached

			if (GetPlayerCount(username) >= GameRulesTemplate.singleton.maxPlayersPerUser)
				return false;

			return true;
			
		}
		
		// -------------------------------------------------------------------------------
		// TryPlayerDeleteSoft
		// -------------------------------------------------------------------------------
		public override bool TryPlayerDeleteSoft(string name, string username, DatabaseAction _action = DatabaseAction.Do)
		{
		
			if (!base.TryPlayerDeleteSoft(name, username) || !PlayerValid(name, username))
				return false;
				
			PlayerSetDeleted(name, _action);
			return true;	
			
		}
		
		// -------------------------------------------------------------------------------
		// TryPlayerDeleteHard
		// Permanently deletes the player and all of its data
		// -------------------------------------------------------------------------------
		public override bool TryPlayerDeleteHard(string name, string username)
		{
		
			if (!base.TryPlayerDeleteHard(name, username) || !PlayerExists(name, username))
				return false;
			
			DeleteDataPlayer(name);
			return true;	
				
		}
		
		// -------------------------------------------------------------------------------
		// TryPlayerBan
		// -------------------------------------------------------------------------------
		public override bool TryPlayerBan(string name, string username, DatabaseAction _action = DatabaseAction.Do)
		{
			
			if (!base.TryPlayerBan(name, username) || !PlayerValid(name, username))
				return false;
				
			PlayerSetBanned(name, _action);
			return true;	
			
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================