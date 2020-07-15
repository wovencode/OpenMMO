// by Fhiz
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{
	
	/// <summary>
	/// The actual DatabaseManager class is almost empty and extended via partial. Derived from BaseDatabaseManager and implements the interface.
	/// </summary>
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
		
		/// <summary>
		/// Singletons are OK for all manager type classes
		/// </summary>
		public static DatabaseManager singleton;
		
		/// <summary>
		/// Updates the define to set the database layer according to chosen database type
		/// </summary>
		void OnEnable()
		{
			OnValidate();
		}
		
		/// <summary>
		/// Updates the define to set the database layer depending of chosen database type
		/// </summary>
		void OnValidate()
		{
			if (databaseLayer)
				databaseLayer.OnValidate();
				
			this.InvokeInstanceDevExtMethods(nameof(OnValidate)); //HOOK
		}
		
		/// <summary>
		/// Hard deletes all users that have been soft deleted before
		/// </summary>
		void DeleteUsers()
		{
			this.InvokeInstanceDevExtMethods(nameof(DeleteUsers)); //HOOK
			debug.Log("["+name+"] Invoking: DeleteUsers"); //DEBUG
		}
		
		/// <summary>
		/// Saves all online players. The actual saving is done via the hooked methods.
		/// </summary>
		public void SavePlayers()
    	{
			this.InvokeInstanceDevExtMethods(nameof(SavePlayers)); //HOOK
    		debug.Log("["+name+"] Invoking: SavePlayers"); //DEBUG
    	}
    	
	}

}