
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Network;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
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
		// 
		// -------------------------------------------------------------------------------
		public bool GetPlayerOnline(string playerName)
		{
			TablePlayer tablePlayer = FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND banned=0 AND deleted=0", playerName);
			
			if (tablePlayer == null)
				return false;
			else
			{
				DateTime dateTime = tablePlayer.lastonline.AddSeconds(saveInterval * 2.0f);
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
		
			if (!base.TryPlayerRegister(name, username, prefabname) || PlayerExists(name, username))
				return false;
			
			// -- check if maximum amount of characters per account reached

			if (GetPlayerCount(username) >= GameRulesTemplate.singleton.maxPlayersPerUser)
				return false;

			return true;
			
		}
		
		// -------------------------------------------------------------------------------
		// TryPlayerDeleteSoft
		// -------------------------------------------------------------------------------
		public override bool TryPlayerDeleteSoft(string name, string username, int _action=1)
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
		public override bool TryPlayerBan(string name, string username, int _action=1)
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