// by Fhiz
using System;
using System.Collections.Generic;
using OpenMMO;
using OpenMMO.Database;

namespace OpenMMO.Database
{

	/// <summary>
    /// This interface contains methods required for the database and database layer to be operational, independent of the database type.
    /// </summary>
	public partial interface IAbstractableDatabase
	{
		
		/// <summary>
        /// Initializes the database or database layer. Called in Awake on server only.
        /// </summary>
		void Init();
		
		/// <summary>
        /// Used to open the connection to the database.
        /// </summary>
		void OpenConnection();
		
		/// <summary>
        /// Used to close the connection to the database.
        /// </summary>
		void CloseConnection();
		
		/// <summary>
        /// Used to begin a transaction.
        /// </summary>
		void BeginTransaction();
		
		/// <summary>
        /// Used to commit a transaction.
        /// </summary>
		void Commit();
		
		/// <summary>
        /// Used to create a table of type T. Where T is a TableClass.
        /// </summary>
		void CreateTable<T>();
		
		/// <summary>
        /// Used to create an index on the table of the given names.
        /// </summary>
		void CreateIndex(string tableName, string[] columnNames, bool unique = false);
		
		/// <summary>
        /// Used to query the table to return one matching result.
        /// </summary>
		T FindWithQuery<T>(string query, params object[] args) where T : new();
		
		/// <summary>
        /// Used to insert an object obj into the table. Where obj is a TableClass object.
        /// </summary>
		void Insert(object obj);
		
		/// <summary>
        /// Used to query the table to return a list of matching results.
        /// </summary>
		List<T> Query<T>(string query, params object[] args) where T : new();
		
		/// <summary>
        /// Used to execute a query that does not return anything.
        /// </summary>
		void Execute(string query, params object[] args);
		
		/// <summary>
        /// Used to insert or replace an object obj into the table. Where obj is a TableClass object.
        /// </summary>
		void InsertOrReplace(object obj);
		
	}
		
}