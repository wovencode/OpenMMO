
//using OpenMMO;
//using OpenMMO.Database;
using UnityEngine;
//using System;
//using System.IO;
//using System.Collections.Generic;
//using Mirror;

namespace OpenMMO.Database
{
	
	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// ============================= PUBLIC METHODS ==================================
		
    	// -------------------------------------------------------------------------------
		// CreateDefaultDataPlayer
		// called when a new player is registered, the hook is executed on all modules and
		// used to parse default data onto the player (like starting Equipment etc.).
		// -------------------------------------------------------------------------------
		public void CreateDefaultDataPlayer(GameObject player)
		{
			this.InvokeInstanceDevExtMethods(nameof(CreateDefaultDataPlayer), player); //HOOK
		}

        // -------------------------------------------------------------------------------
        // LoadDataPlayerPriority
        // Called at the start of LoadDataPlayer, before the rest of the method is called
        // -------------------------------------------------------------------------------
        public virtual void LoadDataPlayerPriority(GameObject prefab, GameObject player)
        {
			this.InvokeInstanceDevExtMethods(nameof(LoadDataPlayerPriority), player); //HOOK
        }
		
		// -------------------------------------------------------------------------------
		// LoadDataPlayer
		// called when a player is loaded from the database, the hooks are executed on
		// all modules and used to load additional player data.
		// -------------------------------------------------------------------------------
		public GameObject LoadDataPlayer(GameObject prefab, string _name)
		{
			GameObject player = Instantiate(prefab);
			player.name = _name;

            LoadDataPlayerPriority(prefab, player);

			this.InvokeInstanceDevExtMethods(nameof(LoadDataPlayer), player); //HOOK
			return player;
		}
		
		// -------------------------------------------------------------------------------
		// SaveUserAccount
		// called when a user is saved to the database, the hook is executed on all
		// modules and used to save additional data.
		//
		// isNew = Is it a new user account? (Saving might be different)
		// useTransaction = When saved individually we can use a transaction
		// -------------------------------------------------------------------------------
		public void SaveUserAccount(string username, bool isNew = false, bool useTransaction = true)
		{
			if (useTransaction)
				databaseLayer.BeginTransaction();
			
			this.InvokeInstanceDevExtMethods(nameof(SaveUserAccount), username, isNew); //HOOK
			
			if (useTransaction)
				databaseLayer.Commit();
		}
		
		// -------------------------------------------------------------------------------
		// SaveDataPlayer
		// called when a player is saved to the database, the hook is executed on all
		// modules and used to save additional data.
		//
		// isNew = Is it a new player character? (Saving might be different)
		// useTransaction = When saved individually we can use a transaction
		// -------------------------------------------------------------------------------
		public void SaveDataPlayer(GameObject player, bool isNew = false, bool useTransaction = true)
		{
			if (useTransaction)
				databaseLayer.BeginTransaction();
			
			this.InvokeInstanceDevExtMethods(nameof(SaveDataPlayer), player, isNew); //HOOK
			
			if (useTransaction)
				databaseLayer.Commit();
		}
		
		// ============================= NETWORK MANAGER EVENTS ==========================
		
		// -------------------------------------------------------------------------------
		// LoginUserAccount
		// From: @NetworkManager
		// -------------------------------------------------------------------------------
		public void LoginUserAccount(string name)
		{
			this.InvokeInstanceDevExtMethods(nameof(LoginUserAccount), name); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// LogoutUserAccount
		// From : @NetworkManager
		// -------------------------------------------------------------------------------
		public void LogoutUserAccount(string name)
		{
			SaveUserAccount(name, false);
			this.InvokeInstanceDevExtMethods(nameof(LogoutUserAccount), name); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// LoginPlayer
		// From: @NetworkManager
		// -------------------------------------------------------------------------------
		public void LoginPlayer(string username, string playername)
		{
			this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), playername, username); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// LogoutPlayer
		// From: @NetworkManager
		// -------------------------------------------------------------------------------
		public void LogoutPlayer(GameObject player)
		{
			SaveDataPlayer(player, false);
			this.InvokeInstanceDevExtMethods(nameof(LogoutPlayer), player); //HOOK
        }
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================