//by  Fhiz
using System;

namespace OpenMMO.Database
{

	/// <summary>
	/// This partial section of DatabaseManager is exposed to the public and used from other components.
	/// </summary>
	public partial class DatabaseManager
	{
		
		/// <summary>
		/// Returns a existing player's prefab from the database, according to the players name.
		/// </summary>
		public string GetPlayerPrefabName(string playername)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=?", playername).prefab;
		}
		/// <summary>
		/// Checks if a player (= character) is online anywhere using 'lastonline' time frame
		/// </summary>
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
		
		/// <summary>
		/// Tries to login a existing player (= character) using the provided playername (= charactername) and username (= accountname).
		/// </summary>
		public override bool TryPlayerLogin(string playername, string username)
		{
			
			if (!base.TryPlayerLogin(playername, username) || !PlayerValid(playername, username))
				return false;
			
			return true;
			
		}
        /// <summary>
        /// Tries to register a new player (= character) on the provided account, using the provided prefab.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="playername"></param>
        /// <param name="prefabname"></param>
        /// <returns></returns>
        public override bool TryPlayerRegister(string playername, string username, string prefabname)
		{
		
			if (!base.TryPlayerRegister(playername, username, prefabname) || PlayerExists(playername))
				return false;
			
			// -- check if maximum amount of characters per account reached

			if (GetPlayerCount(username) >= GameRulesTemplate.singleton.maxPlayersPerUser)
				return false;

			return true;
			
		}
		
		/// <summary>
		/// Tries do soft-delete a player (= character). Soft-deletion does not delete any data, only marks the player as 'deleted'.
		/// </summary>
		public override bool TryPlayerDeleteSoft(string name, string username, DatabaseAction _action = DatabaseAction.Do)
		{
		
			if (!base.TryPlayerDeleteSoft(name, username) || !PlayerValid(name, username))
				return false;
				
			PlayerSetDeleted(name, _action);
			return true;	
			
		}
		
		/// <summary>
		/// Tries to permanently delete the player (= character) and all of its data.
		/// </summary>
		public override bool TryPlayerDeleteHard(string name, string username)
		{
		
			if (!base.TryPlayerDeleteHard(name, username) || !PlayerExists(name, username))
				return false;
			
			DeleteDataPlayer(name);
			return true;	
				
		}
		
		/// <summary>
		/// Tries to ban a player (= character), so it cannot be used to play anymore.
		/// </summary>
		public override bool TryPlayerBan(string name, string username, DatabaseAction _action = DatabaseAction.Do)
		{
			
			if (!base.TryPlayerBan(name, username) || !PlayerValid(name, username))
				return false;
				
			PlayerSetBanned(name, _action);
			return true;	
			
		}
		
	}

}