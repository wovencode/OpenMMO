
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.DebugManager;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace OpenMMO.Database
{
	
	// ===================================================================================
	// DatabaseLayerSQLite
	// ===================================================================================
	[System.Serializable]
	public partial class DatabaseLayerSQLite : DatabaseAbstractionLayer
	{
		
		[Header("Options")]
		public string databaseName 	= "Database.sqlite";
				
		protected 			SQLiteConnection 	connection = null;
		protected static 	string 				_dbPath = "";
		
		// ================================ API METHODS ==================================
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		public override void Init()
		{
		}
		
		// -------------------------------------------------------------------------------
		// OpenConnection
		// -------------------------------------------------------------------------------
		public override void OpenConnection()
		{
			if (connection != null) return;
			_dbPath = Tools.GetPath(databaseName);
			connection = new SQLiteConnection(_dbPath);
		}
		
		// -------------------------------------------------------------------------------
		// CloseConnection
		// -------------------------------------------------------------------------------
		public override void CloseConnection()
		{
			connection?.Close();
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