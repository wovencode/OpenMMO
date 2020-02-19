
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.DebugManager;
using OpenMMO.Network;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{
	
	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// -------------------------------------------------------------------------------
		// Awake
		// Sets the singleton on awake, database can be accessed from anywhere by using it
		// also calls "Init" on database and the databaseLayer to create database and 
		// open connection if
		// that is required
		// -------------------------------------------------------------------------------
		public override void Awake()
		{
			base.Awake(); // required
			singleton = this;

#if _SERVER
			Init();
#endif

		}
		
		// -------------------------------------------------------------------------------
		// Init
		// creates/connects to the database and creates all tables
		// for a multiplayer server based game, this should only be called on the server
		// -------------------------------------------------------------------------------
		public void Init()
		{
			
			OpenConnection();
			
			databaseLayer.Init();
			
			this.InvokeInstanceDevExtMethods(nameof(Init)); //HOOK
			
			if (saveInterval > 0)
				InvokeRepeating(nameof(SavePlayers), saveInterval, saveInterval);
			
			if (deleteInterval > 0)
				InvokeRepeating(nameof(DeleteUsers), deleteInterval, deleteInterval);
			
			this.InvokeInstanceDevExtMethods(nameof(Init)); //HOOK
			
		}
		
		// -------------------------------------------------------------------------------
		// Destruct
		// closes the connection, cancels saving 
		// -------------------------------------------------------------------------------
		public void Destruct()
		{
			CancelInvoke();
			CloseConnection();
		}
		
		// -------------------------------------------------------------------------------
		// OpenConnection
		// -------------------------------------------------------------------------------
		public void OpenConnection()
		{
			databaseLayer.OpenConnection();
			debug.Log("[DatabaseManager] OpenConnection");
		}
		
		// -------------------------------------------------------------------------------
		// CloseConnection
		// -------------------------------------------------------------------------------
		public void CloseConnection()
		{
			databaseLayer.CloseConnection();
			debug.Log("[DatabaseManager] CloseConnection");
		}
		
		// -------------------------------------------------------------------------------
		// CreateTable
		// -------------------------------------------------------------------------------
		public void CreateTable<T>()
		{
			databaseLayer.CreateTable<T>();
			debug.Log("[DatabaseManager] CreateTable: "+typeof(T));
		}
		
		// -------------------------------------------------------------------------------
		// CreateIndex
		// -------------------------------------------------------------------------------
		public void CreateIndex(string tableName, string[] columnNames, bool unique = false)
		{
			databaseLayer.CreateIndex(tableName, columnNames, unique);
			debug.Log("[DatabaseManager] CreateIndex: "+tableName + " ("+string.Join ("_", columnNames)+")");
		}
		
		// -------------------------------------------------------------------------------
		// Query
		// -------------------------------------------------------------------------------
		public List<T> Query<T>(string query, params object[] args) where T : new()
		{
			debug.Log("[DatabaseManager] Query: "+typeof(T)+ "("+query+")");
			return databaseLayer.Query<T>(query, args);
		}
		
		// -------------------------------------------------------------------------------
		// Execute
		// -------------------------------------------------------------------------------
		public void Execute(string query, params object[] args)
		{
			databaseLayer.Execute(query, args);
			debug.Log("[DatabaseManager] Execute: "+query);
		}
		
		// -------------------------------------------------------------------------------
		// FindWithQuery
		// -------------------------------------------------------------------------------
		public T FindWithQuery<T>(string query, params object[] args) where T : new()
		{
			debug.Log("[DatabaseManager] FindWithQuery: "+typeof(T)+" ("+query+")");
			return databaseLayer.FindWithQuery<T>(query, args);
		}
		
		// -------------------------------------------------------------------------------
		// Insert
		// -------------------------------------------------------------------------------
		public void Insert(object obj)
		{
			databaseLayer.Insert(obj);
			debug.Log("[DatabaseManager] Insert: "+obj);
		}
		
		// -------------------------------------------------------------------------------
		// InsertOrReplace
		// -------------------------------------------------------------------------------
		public void InsertOrReplace(object obj)
		{
			databaseLayer.InsertOrReplace(obj);
			debug.Log("[DatabaseManager] InsertOrReplace: "+obj);
		}
		
		// -------------------------------------------------------------------------------
		// BeginTransaction
		// -------------------------------------------------------------------------------
		public void BeginTransaction()
		{
			databaseLayer.BeginTransaction();
			debug.Log("[DatabaseManager] BeginTransaction");
		}
		
		// -------------------------------------------------------------------------------
		// Commit
		// -------------------------------------------------------------------------------
		public void Commit()
		{
			databaseLayer.Commit();
			debug.Log("[DatabaseManager] Commit");
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================