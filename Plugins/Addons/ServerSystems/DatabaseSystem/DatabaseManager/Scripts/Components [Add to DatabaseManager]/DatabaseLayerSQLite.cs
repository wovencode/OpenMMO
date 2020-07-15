// by Fhiz
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Debugging;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace OpenMMO.Database
{
	
	/// <summary>
	/// The DatabaseLayerSQLite overrides all methods of the DatabaseAbstractionLayer and translates the queries to match the database type.
	/// </summary>
	[System.Serializable]
	public partial class DatabaseLayerSQLite : DatabaseAbstractionLayer
	{
		
		[Header("Options")]
		public string databaseName 	= "DatabaseOpenMMO.sqlite";
		
		protected 			SQLiteConnection 	connection = null;
		protected static 	string 				_dbPath = "";
		
		/// <summary>
		/// Overridable Init that might be required for certain database layer implementations.
		/// <summary>
		public override void Init() {}
		
		/// <summary>
		/// Opens a new connection.
		/// <summary>
		public override void OpenConnection()
		{
			if (connection != null) return;
			_dbPath = Tools.GetPath(databaseName);
			connection = new SQLiteConnection(_dbPath);
		}
		
		/// <summary>
		/// Closes a connection if it exists and is open.
		/// <summary>
		public override void CloseConnection()
		{
			connection?.Close();
		}
				
		/// <summary>
		/// Creates a table of type T. Where T is a TableClass.
		/// <summary>
		public override void CreateTable<T>()
		{
			connection.CreateTable<T>();
		}
		
		/// <summary>
		/// Creates an index on a existing table. Where columnNames is a string array of column names on the table.
		/// <summary>
		public override void CreateIndex(string tableName, string[] columnNames, bool unique = false)
		{
			connection.CreateIndex(tableName, columnNames, unique);
		}
		
		/// <summary>
		/// Queries the given table and returns a list of matching TableClasses.
		/// <summary>
		public override List<T> Query<T>(string query, params object[] args)
		{
			return connection.Query<T>(query, args);
		}
		
		/// <summary>
		/// Executes a query that does not return anything. Where arguments is a list of parameters.
		/// <summary>
		public override void Execute(string query, params object[] args)
		{
			connection.Execute(query, args);
		}
		
		/// <summary>
		/// Finds a matching object using a query. Where arguments is a list of properties. Returns a TableClass.
		/// <summary>
		public override T FindWithQuery<T>(string query, params object[] args)
		{
			return connection.FindWithQuery<T>(query, args);
		}
		
		/// <summary>
		/// Executes a Insert. Where object is a TableClass.
		/// <summary>
		public override void Insert(object obj)
		{
			connection.Insert(obj);		
		}
		
		/// <summary>
		/// Executes a InsertOrReplace. Where object is a TableClass.
		/// <summary>
		public override void InsertOrReplace(object obj)
		{
			connection.InsertOrReplace(obj);		
		}
		
		/// <summary>
		/// Begins a transaction.
		/// <summary>
		public override void BeginTransaction()
		{
			connection.BeginTransaction();		
		}
		
		/// <summary>
		/// Commits a transaction.
		/// <summary>
		public override void Commit()
		{
			connection.Commit();
		}
		
		 /// <summary>
		/// Called when the inspector properties change due to user input.
		/// <summary>
		public override void OnValidate() {}

	}

}
