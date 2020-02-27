
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{
	
	// ===================================================================================
	// Database
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class DatabaseManager : BaseDatabaseManager, IAbstractableDatabase
	{
		
		[Header("Settings")]
		public DatabaseAbstractionLayer databaseLayer;
		[Tooltip("Player data save interval in seconds (0 to disable).")]
		public float saveInterval = 60f;
		[Tooltip("Deleted user prune interval in seconds (0 to disable).")]
		public float deleteInterval = 240f;
		[Tooltip("Allow relogin after client inactivity in seconds (should be saveInterval or more).")]
		public float logoutInterval = 90f;
		
		public static DatabaseManager singleton;
		
		// -------------------------------------------------------------------------------
		// OnEnable
		// updates the define to set the database layer depending of chosen database type
		// -------------------------------------------------------------------------------
		void OnEnable()
		{
			OnValidate();
		}
		
		// -------------------------------------------------------------------------------
		// OnValidate
		// updates the define to set the database layer depending of chosen database type
		// -------------------------------------------------------------------------------
		void OnValidate()
		{
			if (databaseLayer)
				databaseLayer.OnValidate();
				
			this.InvokeInstanceDevExtMethods(nameof(OnValidate)); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// DeleteUsers
		// hard deletes all users that have been soft deleted before
		// -------------------------------------------------------------------------------
		void DeleteUsers()
		{
			this.InvokeInstanceDevExtMethods(nameof(DeleteUsers)); //HOOK
			debug.Log("["+name+"] Invoking: DeleteUsers"); //DEBUG
		}
		
		// -------------------------------------------------------------------------------
		// SavePlayers
		// -------------------------------------------------------------------------------
		public void SavePlayers()
    	{
			this.InvokeInstanceDevExtMethods(nameof(SavePlayers)); //HOOK
    		debug.Log("["+name+"] Invoking: SavePlayers"); //DEBUG
    	}
    	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================