
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Network;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Mirror;

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
			
			// -- place on navmesh in case it changed
			OpenMMO.Network.NetworkManager.singleton.ValidatePlayerPosition(player);
		
            LoadDataPlayerPriority(prefab, player);

			this.InvokeInstanceDevExtMethods(nameof(LoadDataPlayer), player); //HOOK
			return player;
		}
		
		// -------------------------------------------------------------------------------
		// SaveDataUser
		// called when a user is saved to the database, the hook is executed on all
		// modules and used to save additional data.
		//
		// isNew = Is it a new user account? (Saving might be different)
		// useTransaction = When saved individually we can use a transaction
		// -------------------------------------------------------------------------------
		public void SaveDataUser(string username, bool isNew = false, bool useTransaction = true)
		{
			if (useTransaction)
				databaseLayer.BeginTransaction();
			
			this.InvokeInstanceDevExtMethods(nameof(SaveDataUser), username, isNew); //HOOK
			
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
		// LoginUser
		// From: @NetworkManager
		// -------------------------------------------------------------------------------
		public void LoginUser(string name)
		{
			this.InvokeInstanceDevExtMethods(nameof(LoginUser), name); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// LogoutUser
		// From : @NetworkManager
		// -------------------------------------------------------------------------------
		public void LogoutUser(string name)
		{
debug.Log("DatabaseManager->LogoutUser");
			SaveDataUser(name, false);
			this.InvokeInstanceDevExtMethods(nameof(LogoutUser), name); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// LoginPlayer
		// From: @NetworkManager
		// -------------------------------------------------------------------------------
		public void LoginPlayer(NetworkConnection conn, GameObject player, string playerName, string userName)
		{
			this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), conn, player, playerName, userName); //HOOK
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