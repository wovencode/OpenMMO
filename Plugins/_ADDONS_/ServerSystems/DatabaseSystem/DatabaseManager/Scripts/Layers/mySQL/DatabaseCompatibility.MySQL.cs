// by Fhiz
using OpenMMO;
using OpenMMO.Database;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace OpenMMO.Database
{
	
	/// <summary>
	/// Translates queries from SQLite to mySQL using System.Reflection. This is possible because the syntax of both languages is very similar.
	/// <remarks>This solution is a bit slower and more cumbersome, but not used too frequently (not in update etc.). It allows to drive all database queries off the same codebase.</remarks>
	/// <remarks>Several caches store common queries, parameter lists and table maps to increase performance. Those caches are built the first time they are used and then simply fetched.</remarks>
	/// </summary>
	public partial class DatabaseCompatibility
	{
		
		protected Dictionary<int, TableMap> tableMaps = new Dictionary<int, TableMap>();
		protected Dictionary<int, MySqlParameter[]> mySQLParameters = new Dictionary<int, MySqlParameter[]>();
		protected Dictionary<int, string> mySQLQueries = new Dictionary<int, string>();
		
		/// <summary>
		/// Creates a TableMap class instance from the provided object T. Where T is a TableClass.
		/// <remarks>If the TableMap is found in the cache already, it is returned. Which is much faster than rebuilding it every time.</remarks>
		/// </summary>
		public TableMap GetTableMap<T>()
		{
			
			string tableName = typeof(T).Name;
			
			tableMaps.TryGetValue(tableName.GetDeterministicHashCode(), out TableMap tableMap);
			
			if (tableMap != null)
				return tableMap;
			
			TableMap newTableMap = BuildTableMapFromType<T>();
			
			tableMaps.Add(tableName.GetDeterministicHashCode(), newTableMap);
			
			return newTableMap;
		
		}
		
		/// <summary>
		/// Returns a TableMap class instance or builds a new one from the provided object. Where the object is a TableClass.
		/// <remarks>If the TableMap is found in the cache already, it is returned. Which is much faster than rebuilding it every time.</remarks>
		/// </summary>
		public TableMap GetTableMap(object obj)
		{
			
			string tableName = obj.GetType().Name;
			
			tableMaps.TryGetValue(tableName.GetDeterministicHashCode(), out TableMap tableMap);
			
			if (tableMap != null)
			{
				tableMap.UpdateValues(obj);
				return tableMap;
			}
			
			TableMap newTableMap = BuildTableMapFromObject(obj);
			
			tableMaps.Add(tableName.GetDeterministicHashCode(), newTableMap);
			
			newTableMap.UpdateValues(obj);
			
			return newTableMap;
			
		}
		
		/// <summary>
		/// Converts provided SQLite parameters to mySQL or returns cached ones.
		/// <remarks>If the exact parameter list is found in the cache already, it returns that instead. Which is much faster than rebuilding it every time.</remarks>
		/// </summary>
		public MySqlParameter[] GetConvertedParameters(object[] args)
		{
		
			int hash = Tools.GetArrayHashCode(args);
			
			mySQLParameters.TryGetValue(hash, out MySqlParameter[] cachedParameters);
			
			if (cachedParameters != null)
				return cachedParameters;
			
			MySqlParameter[] newParameters = BuildConvertedParameters(args);
			
			mySQLParameters.Add(hash, newParameters);
			
			return newParameters;
			
		}
		
		/// <summary>
		/// Converts the provided SQLite query to mySQL or returns a cached one.
		/// <remarks>If the exact query (without parameters) is found in the cache already, it returns that instead. Which is much faster than rebuilding it every time.</remarks>
		/// </summary>
		public string GetConvertedQuery(string query)
		{
			
			int hash = query.GetDeterministicHashCode();
			
			mySQLQueries.TryGetValue(hash, out string cachedQuery);
			
			if (cachedQuery != null && !String.IsNullOrWhiteSpace(cachedQuery))
				return cachedQuery;
				
			string newQuery = BuildConvertedQuery(query);
			
			mySQLQueries.Add(hash, newQuery);
		
			return newQuery;
			
		}
		
		/// <summary>
		/// Converts a mySQLRowsReader into a list of results of type T. Where T is a TableClass.
		/// </summary>
		public List<T> ConvertReader<T>(MySQLRowsReader reader)
		{

			if (reader.RowCount == 0)
				return null;
			
			List<T> results = new List<T>();
			
			while (reader.Read())
			{
				
				TableMap map = GetTableMap<T>();
				
				for (int i = 0; i < map.rows.Length; i++)
				{
					
					object obj = null;
					
					if (map.rows[i].type == typeof(int))
					{
						obj = reader.GetInt32(map.rows[i].name);
					}
					else if (map.rows[i].type == typeof(bool))
					{
						obj = reader.GetBoolean(map.rows[i].name);
					}		
					else if (map.rows[i].type == typeof(long))
					{
						obj = reader.GetInt64(map.rows[i].name);
					}		
					else if (map.rows[i].type == typeof(string))
					{
						obj = reader.GetString(map.rows[i].name);
					}		
					else if (map.rows[i].type == typeof(DateTime))
					{
						obj = reader.GetDateTime(map.rows[i].name);
					}
					else if (map.rows[i].type == typeof(float))
					{
						obj = reader.GetFloat(map.rows[i].name);
					}
					else if (map.rows[i].type == typeof(double))
					{
						obj = reader.GetDouble(map.rows[i].name);
					}
					
					map.UpdateValue(map.rows[i].name, obj);
					
				}

				results.Add(map.ToType<T>());
			}
			
			return results;
			
		}
				
		/// <summary>
		/// Builds a TableMap instance from the provided type T. Where T is a TableClass.
		/// </summary>
		protected TableMap BuildTableMapFromType<T>()
		{
			
			bool hasPrimary = false;
			PropertyInfo[] pInfo;
			Type t = typeof(T);
			pInfo = t.GetProperties();
			
			TableMap tableMap = new TableMap(t, typeof(T).Name, pInfo.Length);

			for (int i = 0; i < pInfo.Length; i++)
			{
				tableMap.rows[i] = new TableRow();
				tableMap.rows[i].name = pInfo[i].Name;
				tableMap.rows[i].type = pInfo[i].PropertyType;
				
				if (IsPK(pInfo[i]) && !hasPrimary)
				{
					tableMap.rows[i].primary = true;
					hasPrimary = true;
				}
				
			}
			
			return tableMap;
		
		}
		
		/// <summary>
		/// Builds a TableMap instance from the provided object obj. Where obj is a TableClass.
		/// </summary>
		protected TableMap BuildTableMapFromObject(object obj)
		{
			
			bool hasPrimary = false;			
			PropertyInfo[] pInfo;
			Type t = obj.GetType();
			pInfo = t.GetProperties();
			
			TableMap tableMap = new TableMap(t, obj.GetType().Name, pInfo.Length);
			
			for (int i = 0; i < pInfo.Length; i++)
			{
				tableMap.rows[i] = new TableRow();
				tableMap.rows[i].name = pInfo[i].Name;
				tableMap.rows[i].type = pInfo[i].PropertyType;
				
				if (IsPK(pInfo[i]) && !hasPrimary)
				{
					tableMap.rows[i].primary = true;
					hasPrimary = true;
				}
				
			}
			
			return tableMap;
		
		}
		
		/// <summary>
		/// Takes a SQLite parameter list and converts it to mySQL.
		/// </summary>
		protected MySqlParameter[] BuildConvertedParameters(object[] args)
		{
			
			MySqlParameter[] parameters = new MySqlParameter[args.Length];
			
			for (int i = 0; i < args.Length; i++)
				parameters[i] = new MySqlParameter("@"+i.ToString(), args.GetValue(i));
				
			return parameters;
		
		}
		
		/// <summary>
		/// Takes a SQLite query in form of a string and converts it to mySQL.
		/// </summary>
		protected string BuildConvertedQuery(string query)
		{
		
			int count = query.Split('?').Length -1;
			
			for (int i = 0; i < count; i++)
				query = query.ReplaceFirstInstance("?", "@"+i.ToString());
			
			return query;
			
		}
		
		/// <summary>
		/// Checks if the provided column is of the type "Primary Key"
		/// <remarks>This function requires the SQLite.net open-source code that is present anyway in this project.</remarks>
		/// </summary>
        protected bool IsPK (MemberInfo p)
		{
			return p.CustomAttributes.Any (x => x.AttributeType == typeof (PrimaryKeyAttribute));
		}
		
        /// <summary>
		/// Checks if the provided column is of the type "Auto Increment"
		/// <remarks>This function requires the SQLite.net open-source code that is present anyway in this project.</remarks>
		/// </summary>
        protected bool IsAutoInc (MemberInfo p)
		{
			return p.CustomAttributes.Any (x => x.AttributeType == typeof (AutoIncrementAttribute));
		}
		
		// TODO: No Collate
        
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================