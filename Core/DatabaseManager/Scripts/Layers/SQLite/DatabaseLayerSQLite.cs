
using Wovencode;
using Wovencode.Database;
using Wovencode.DebugManager;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace Wovencode.Database
{
	
	// ===================================================================================
	// DatabaseLayerSQLite
	// ===================================================================================
	[System.Serializable]
	public partial class DatabaseLayerSQLite : DatabaseAbstractionLayer
	{
		
		[Header("Options")]
		public string databaseName 	= "Database.sqlite";
		[Tooltip("Launch automatically when the game is started (recommended for Single-Player games).")]
		public bool initOnAwake;
		[Tooltip("Compares the hash of the database with player prefs to prevent cheating.")]
		public bool checkIntegrity;
		
		protected 			SQLiteConnection 	connection = null;
		protected static 	string 				_dbPath = "";
		
		// ================================ API METHODS ==================================
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		public override void Init()
		{
			if (!initOnAwake) return;
			OpenConnection();
		}
		
		// -------------------------------------------------------------------------------
		// OpenConnection
		// -------------------------------------------------------------------------------
		public override void OpenConnection()
		{
			
			if (connection != null) return;
			
			_dbPath = Tools.GetPath(databaseName);
			
			// checks if the database file has been manipulated outside of the game
			// recommended for single-player games only, not recommended on very large files
			if (File.Exists(_dbPath) &&
				checkIntegrity &&
				Tools.GetChecksum(_dbPath) == false)
			{
				debug.LogWarning("[DatabaseManager] Database file is corrupted!");
				// deletes the file, a fresh database file is re-created thereafter
				File.Delete(_dbPath);
			}

			connection = new SQLiteConnection(_dbPath);
		
		}
		
		// -------------------------------------------------------------------------------
		// CloseConnection
		// -------------------------------------------------------------------------------
		public override void CloseConnection()
		{
			
			connection?.Close();
		
			if (checkIntegrity)
				Tools.SetChecksum(_dbPath);
			
		}
				
		// -------------------------------------------------------------------------------
		// CreateTable
		// -------------------------------------------------------------------------------
		public override void CreateTable<T>()
		{
			connection.CreateTable<T>();
		}
		
		// -------------------------------------------------------------------------------
		// CreateIndex
		// -------------------------------------------------------------------------------
		public override void CreateIndex(string tableName, string[] columnNames, bool unique = false)
		{
			connection.CreateIndex(tableName, columnNames, unique);
		}
		
		// -------------------------------------------------------------------------------
		// Query
		// -------------------------------------------------------------------------------
		public override List<T> Query<T>(string query, params object[] args)
		{
			return connection.Query<T>(query, args);
		}
		
		// -------------------------------------------------------------------------------
		// Execute
		// -------------------------------------------------------------------------------
		public override void Execute(string query, params object[] args)
		{
			connection.Execute(query, args);
		}
		
		// -------------------------------------------------------------------------------
		// FindWithQuery
		// -------------------------------------------------------------------------------
		public override T FindWithQuery<T>(string query, params object[] args)
		{
			return connection.FindWithQuery<T>(query, args);
		}
		
		// -------------------------------------------------------------------------------
		// Insert
		// -------------------------------------------------------------------------------
		public override void Insert(object obj)
		{
			connection.Insert(obj);		
		}
		
		// -------------------------------------------------------------------------------
		// InsertOrReplace
		// -------------------------------------------------------------------------------
		public override void InsertOrReplace(object obj)
		{
			connection.InsertOrReplace(obj);		
		}
		
		// -------------------------------------------------------------------------------
		// BeginTransaction
		// -------------------------------------------------------------------------------
		public override void BeginTransaction()
		{
			connection.BeginTransaction();		
		}
		
		// -------------------------------------------------------------------------------
		// Commit
		// -------------------------------------------------------------------------------
		public override void Commit()
		{
			connection.Commit();
		}
		
		// -------------------------------------------------------------------------------
		// OnValidate
		// -------------------------------------------------------------------------------
		public override void OnValidate()
		{
			
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================