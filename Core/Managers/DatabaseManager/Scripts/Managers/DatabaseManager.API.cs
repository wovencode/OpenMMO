// by Fhiz
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Debugging;
using OpenMMO.Network;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{
	
	/// <summary>
	/// This partial section of the DatabaseManager accepts all kinds of queries and forwards them to the DatabaseLayer currently in use.
	/// </summary>
	public partial class DatabaseManager
	{
	
		/// <summary>
		/// Sets the singleton on awake, database can be accessed from anywhere by using it. Calls Init only when we are the server.
		/// </summary>
		public override void Awake()
		{
			base.Awake(); // required
			singleton = this;

#if _SERVER
			Init();
#endif

		}
		
		/// <summary>
		/// creates/connects to the database and creates all (non-existant) tables plus indices
		/// </summary>
		public void Init()
		{
			
			OpenConnection();
			
			databaseLayer.Init();
			
			this.InvokeInstanceDevExtMethods(nameof(Init)); //HOOK
			
			if (saveInterval > 0)
			{
				InvokeRepeating(nameof(SavePlayers), saveInterval, saveInterval);
				debug.Log("["+name+"] InvokeRepeating: SavePlayers every "+saveInterval+" seconds"); //DEBUG
			}
			
			if (deleteInterval > 0)
			{
				InvokeRepeating(nameof(DeleteUsers), deleteInterval, deleteInterval);
				debug.Log("["+name+"] InvokeRepeating: DeleteUsers every "+deleteInterval+" seconds"); //DEBUG
			}
			
			this.InvokeInstanceDevExtMethods(nameof(Init)); //HOOK
			
		}
		
		/// <summary>
		/// Closes the connection and cancels player saving. Usually called from OnDestroy
		/// </summary>
		public void Destruct()
		{
			CancelInvoke();
			CloseConnection();
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to open the connection to the database
		/// </summary>
		public void OpenConnection()
		{
			databaseLayer.OpenConnection();
			debug.Log("["+name+"] OpenConnection"); //DEBUG
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to close the connection to the database
		/// </summary>
		public void CloseConnection()
		{
			databaseLayer.CloseConnection();
			debug.Log("["+name+"] CloseConnection"); //DEBUG
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to create a new table of type T
		/// </summary>
		public void CreateTable<T>()
		{
			databaseLayer.CreateTable<T>();
			debug.Log("["+name+"] CreateTable: "+typeof(T)); //DEBUG
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to create a new index for the given table
		/// </summary>
		public void CreateIndex(string tableName, string[] columnNames, bool unique = false)
		{
			databaseLayer.CreateIndex(tableName, columnNames, unique);
			debug.Log("["+name+"] CreateIndex: "+tableName + " ("+string.Join ("_", columnNames)+")"); //DEBUG
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to query a table and return a list of matching objects
		/// </summary>
		public List<T> Query<T>(string query, params object[] args) where T : new()
		{
			debug.Log("["+name+"] Query: "+typeof(T)+ "("+query+")"); //DEBUG
			return databaseLayer.Query<T>(query, args);
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to execute a query that does not return anything
		/// </summary>
		public void Execute(string query, params object[] args)
		{
			databaseLayer.Execute(query, args);
			debug.Log("["+name+"] Execute: "+query); //DEBUG
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to return one object of type T that matches the query
		/// </summary>
		public T FindWithQuery<T>(string query, params object[] args) where T : new()
		{
			debug.Log("["+name+"] FindWithQuery: "+typeof(T)+" ("+query+")"); //DEBUG
			return databaseLayer.FindWithQuery<T>(query, args);
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to insert a new object into the database. Where object is a TableClass.
		/// </summary>
		public void Insert(object obj)
		{
			databaseLayer.Insert(obj);
			debug.Log("["+name+"] Insert: "+obj); //DEBUG
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to insert or replace a object into the database. Where object is a TableClass.
		/// </summary>
		public void InsertOrReplace(object obj)
		{
			databaseLayer.InsertOrReplace(obj);
			debug.Log("["+name+"] InsertOrReplace: "+obj); //DEBUG
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to begin a transaction.
		/// </summary>
		public void BeginTransaction()
		{
			databaseLayer.BeginTransaction();
			debug.Log("["+name+"] BeginTransaction"); //DEBUG
		}
		
		/// <summary>
		/// Tells the DatabaseLayer to commit a transaction.
		/// </summary>
		public void Commit()
		{
			databaseLayer.Commit();
			debug.Log("["+name+"] Commit"); //DEBUG
		}
		
	}

}