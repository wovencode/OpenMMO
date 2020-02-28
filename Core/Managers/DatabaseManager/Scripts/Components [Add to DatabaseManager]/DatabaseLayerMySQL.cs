// by Fhiz, insthync
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Debugging;
using UnityEngine;
using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using SQLite;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO.Database
{
	
	/// <summary>
	/// The DatabaseLayerMySQL overrides all methods of the DatabaseAbstractionLayer and translates the queries to match the database type.
	/// </summary>
	[System.Serializable]
	public partial class DatabaseLayerMySQL : DatabaseAbstractionLayer
	{
	
		[Header("Settings")]
		[Tooltip("Default: 127.0.0.1")]
        public string address;
        [Tooltip("Default: 3306")]
        public uint port;
        [Tooltip("Default: root")]
        public string username;
        [Tooltip("Default: password")]
        public string password;
        [Tooltip("Default: mysqldatabase")]
        public string dbName;
        [Tooltip("Default: utf8mb4")]
		public string charset;
		
		protected MySqlConnection connection = null;
		protected string connectionString = null;
		
		protected DatabaseCompatibility dbCompat = new DatabaseCompatibility();
		
		/// <summary>
		/// Overridable Init that might be required for certain database layer implementations.
		/// <summary>
		public override void Init() {}
		
		/// <summary>
		/// Opens a new connection or re-establishes a existing connection.
		/// <summary>
		public override void OpenConnection()
		{
			
			if (connection == null)
				connection = NewConnection();
				
			connection.Open();
		}
		
		/// <summary>
		/// Closes a connection if it exists and is open.
		/// <summary>
		public override void CloseConnection()
		{
			if (connection != null)
				connection.Close();
		}
		
		/// <summary>
		/// Creates a table of type T. Where T is a TableClass.
		/// <summary>
		public override void CreateTable<T>()
		{
			TableMap tableMap = dbCompat.GetTableMap<T>();
			
			string primaryKeyString = "";
			
			if (tableMap.HasPrimaryKey)
				primaryKeyString = ", PRIMARY KEY (`"+tableMap.GetPrimaryKey+"`)";
			
			string queryString = "CREATE TABLE IF NOT EXISTS "+tableMap.name+"("+tableMap.RowsToMySQLInsertString+primaryKeyString+") CHARACTER SET="+charset;

			ExecuteNonQuery(connection, null, queryString);
			
		}
		
		/// <summary>
		/// Creates an index on a existing table. Where columnNames is a string array of column names on the table.
		/// <summary>
		public override void CreateIndex(string tableName, string[] columnNames, bool unique = false)
		{
			
			string indexName = tableName + "_" + string.Join ("_", columnNames);
			
			if (ExecuteScalar(connection, null, "SELECT COUNT(1) "+indexName+" FROM "+tableName) == null)
			{
				string isUnique = unique ? "UNIQUE" : "";
				string queryString = "CREATE INDEX "+indexName+" "+isUnique+" ON "+tableName+" ("+string.Join (",", columnNames)+")";
				ExecuteNonQuery(connection, null, queryString);
			}
			
		}
		
		/// <summary>
		/// Queries the given table and returns a list of matching TableClasses.
		/// <summary>
		public override void Execute(string query, params object[] args)
		{
			ExecuteNonQuery(connection, null, dbCompat.GetConvertedQuery(query), dbCompat.GetConvertedParameters(args));
		}
		
		/// <summary>
		/// Executes a query that does not return anything. Where arguments is a list of parameters.
		/// <summary>
		public override List<T> Query<T>(string query, params object[] args)
		{
			MySQLRowsReader reader = ExecuteReader(connection, null, dbCompat.GetConvertedQuery(query), dbCompat.GetConvertedParameters(args));
			return dbCompat.ConvertReader<T>(reader);
		}
		
		/// <summary>
		/// Finds a matching object using a query. Where arguments is a list of properties. Returns a TableClass.
		/// <summary>
		public override T FindWithQuery<T>(string query, params object[] args)
		{
			List<T> list = Query<T>(query, args);
			
			if (list == null)
				return default(T);
			
			return list.FirstOrDefault();
			
		}
		
		/// <summary>
		/// Executes a Insert. Where object is a TableClass.
		/// <summary>
		public override void Insert(object obj)
		{

			if (obj == null)
				return;
			
			TableMap tableMap = dbCompat.GetTableMap(obj);
			
			string queryString = "INSERT INTO "+tableMap.name+" ("+tableMap.RowsToMySQLString()+") VALUES("+tableMap.RowsToMySQLString("@")+")";

			ExecuteNonQuery(connection,null,  queryString, tableMap.RowsToMySQLParameters);
		
		}
		
		/// <summary>
		/// Executes a InsertOrReplace. Where objects is a TableClass.
		/// <summary>
		public override void InsertOrReplace(object obj)
		{
		
			if (obj == null)
				return;
			
			TableMap tableMap = dbCompat.GetTableMap(obj);
			
			string queryString = "REPLACE INTO "+tableMap.name+" ("+tableMap.RowsToMySQLString()+") VALUES("+tableMap.RowsToMySQLString("@")+")";

			ExecuteNonQuery(connection,null,  queryString, tableMap.RowsToMySQLParameters);
		
		}
		
		/// <summary>
		/// Begins a transaction.
		/// <summary>
		public override void BeginTransaction()
		{
			ExecuteNonQuery("START TRANSACTION");
		}
		
		/// <summary>
		/// Commits a transaction.
		/// <summary>
		public override void Commit()
		{
			ExecuteNonQuery("COMMIT");
		}
		
		
        /// <summary>
		/// Called when the inspector properties change due to user input.
		/// <summary>
		public override void OnValidate()
		{
#if UNITY_EDITOR
			address 	= EditorTools.EditorPrefsUpdateString(Constants.EditorPrefsMySQLAddress, address);
			port 		= (uint)EditorTools.EditorPrefsUpdateInt(Constants.EditorPrefsMySQLPort, (int)port);
			username 	= EditorTools.EditorPrefsUpdateString(Constants.EditorPrefsMySQLUsername, username);
			password 	= EditorTools.EditorPrefsUpdateString(Constants.EditorPrefsMySQLPassword, password);
			dbName 		= EditorTools.EditorPrefsUpdateString(Constants.EditorPrefsMySQLDatabase, dbName);
			charset		= EditorTools.EditorPrefsUpdateString(Constants.EditorPrefsMySQLCharset, charset);
#endif
		}
        
		// ============================== PRIVATE METHODS ================================
		
		/*
		
		ThirdPartyNotice:
		
		F. insthync/UnityMultiplayerARPG_MMO
		MIT License

		Copyright (c) 2018 Ittipon Teerapruettikulchai

		Permission is hereby granted, free of charge, to any person obtaining a copy
		of this software and associated documentation files (the "Software"), to deal
		in the Software without restriction, including without limitation the rights
		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
		copies of the Software, and to permit persons to whom the Software is
		furnished to do so, subject to the following conditions:

		The above copyright notice and this permission notice shall be included in all
		copies or substantial portions of the Software.

		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
		LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
		SOFTWARE.
		
		*/
		
		string GetConnectionString
		{
			get
			{
				if (connectionString == null)
				{
					MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder
					{
						Server 			= string.IsNullOrWhiteSpace(address) 	? "127.0.0.1" 	: address,
						Database 		= string.IsNullOrWhiteSpace(dbName) 	? "database" 	: dbName,
						UserID 			= string.IsNullOrWhiteSpace(username) 	? "root" 		: username,
						Password 		= string.IsNullOrWhiteSpace(password) 	? "password" 	: password,
						Port 			= port,
						CharacterSet 	= string.IsNullOrWhiteSpace(charset) 	? "utf8mb4" 	: charset
					};
					connectionString = connectionStringBuilder.ConnectionString;
				}
				return connectionString;
			}
		}
		
		MySqlConnection NewConnection()
        {
            return new MySqlConnection(GetConnectionString);
        }
		
        long ExecuteInsertData(string sql, params MySqlParameter[] args)
        {
            MySqlConnection connection = NewConnection();
            connection.Open();
            long result = ExecuteInsertData(connection, null, sql, args);
            connection.Close();
            return result;
        }
		
        long ExecuteInsertData(MySqlConnection connection, MySqlTransaction transaction, string sql, params MySqlParameter[] args)
        {
            bool createLocalConnection = false;
            if (connection == null)
            {
                connection = NewConnection();
                transaction = null;
                connection.Open();
                createLocalConnection = true;
            }
            long result = 0;
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                if (transaction != null)
                    cmd.Transaction = transaction;
                foreach (MySqlParameter arg in args)
                {
                    cmd.Parameters.Add(arg);
                }
                cmd.ExecuteNonQuery();
                result = cmd.LastInsertedId;
            }
            if (createLocalConnection)
                connection.Close();
            return result;
        }
		
        int ExecuteNonQuery(string sql, params MySqlParameter[] args)
        {
            MySqlConnection connection = NewConnection();
            connection.Open();
            int result = ExecuteNonQuery(connection, null, sql, args);
            connection.Close();
            return result;
        }
		
        int ExecuteNonQuery(MySqlConnection connection, MySqlTransaction transaction, string sql, params MySqlParameter[] args)
        {
            bool createLocalConnection = false;
            if (connection == null)
            {
                connection = NewConnection();
                transaction = null;
                connection.Open();
                createLocalConnection = true;
            }
            int numRows = 0;
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                if (transaction != null)
                    cmd.Transaction = transaction;
                foreach (MySqlParameter arg in args)
                {
                    cmd.Parameters.Add(arg);
                }
                numRows = cmd.ExecuteNonQuery();
            }
            if (createLocalConnection)
                connection.Close();
            return numRows;
        }
		
        object ExecuteScalar(string sql, params MySqlParameter[] args)
        {
            MySqlConnection connection = NewConnection();
            connection.Open();
            object result = ExecuteScalar(connection, null, sql, args);
            connection.Close();
            return result;
        }
		
        object ExecuteScalar(MySqlConnection connection, MySqlTransaction transaction, string sql, params MySqlParameter[] args)
        {
            bool createLocalConnection = false;
            if (connection == null)
            {
                connection = NewConnection();
                transaction = null;
                connection.Open();
                createLocalConnection = true;
            }
            object result;
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                if (transaction != null)
                    cmd.Transaction = transaction;
                foreach (MySqlParameter arg in args)
                {
                    cmd.Parameters.Add(arg);
                }
                result = cmd.ExecuteScalar();
            }
            if (createLocalConnection)
                connection.Close();
            return result;
        }
		
        MySQLRowsReader ExecuteReader(string sql, params MySqlParameter[] args)
        {
            MySqlConnection connection = NewConnection();
            connection.Open();
            MySQLRowsReader result = ExecuteReader(connection, null, sql, args);
            connection.Close();
            return result;
        }
		
        MySQLRowsReader ExecuteReader(MySqlConnection connection, MySqlTransaction transaction, string sql, params MySqlParameter[] args)
        {
            bool createLocalConnection = false;
            if (connection == null)
            {
                connection = NewConnection();
                transaction = null;
                connection.Open();
                createLocalConnection = true;
            }
            MySQLRowsReader result = new MySQLRowsReader();
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                if (transaction != null)
                    cmd.Transaction = transaction;
                foreach (MySqlParameter arg in args)
                {
                    cmd.Parameters.Add(arg);
                }
                MySqlDataReader dataReader = cmd.ExecuteReader();
                result.Init(dataReader);
                dataReader.Close();
            }
            if (createLocalConnection)
                connection.Close();
            return result;
        }
        
	}

}
