
using Wovencode;
using Wovencode.Database;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Wovencode.Database
{
	
	// ===================================================================================
	// DatabaseCompatibility
	// ===================================================================================
	public partial class DatabaseCompatibility
	{
		
		// Caches
		protected Dictionary<int, TableMap> tableMaps = new Dictionary<int, TableMap>();
		protected Dictionary<int, MySqlParameter[]> mySQLParameters = new Dictionary<int, MySqlParameter[]>();
		protected Dictionary<int, string> mySQLQueries = new Dictionary<int, string>();
		
		// -------------------------------------------------------------------------------
		// GetTableMap
		// -------------------------------------------------------------------------------
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
		
		// -------------------------------------------------------------------------------
		// GetTableMap
		// -------------------------------------------------------------------------------
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
		
		// -------------------------------------------------------------------------------
		// GetConvertedParameters
		// converts parameters or returns cached ones
		// -------------------------------------------------------------------------------
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
		
		// -------------------------------------------------------------------------------
		// GetConvertedQuery
		// converts the query or returns a cached one
		// -------------------------------------------------------------------------------
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
		
		// -------------------------------------------------------------------------------
		// ConvertReader
		// -------------------------------------------------------------------------------
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
				
		// -------------------------------------------------------------------------------
		// BuildTableMapFromType
		// -------------------------------------------------------------------------------
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
		
		// ============================= PROTECTED =======================================
		
		// -------------------------------------------------------------------------------
		// BuildTableMapFromObject
		// -------------------------------------------------------------------------------
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
		
		// -------------------------------------------------------------------------------
		// BuildConvertedParameters
		// -------------------------------------------------------------------------------
		protected MySqlParameter[] BuildConvertedParameters(object[] args)
		{
			
			MySqlParameter[] parameters = new MySqlParameter[args.Length];
			
			for (int i = 0; i < args.Length; i++)
				parameters[i] = new MySqlParameter("@"+i.ToString(), args.GetValue(i));
				
			return parameters;
		
		}
		
		// -------------------------------------------------------------------------------
		// BuildConvertedQuery
		// -------------------------------------------------------------------------------
		protected string BuildConvertedQuery(string query)
		{
		
			int count = query.Split('?').Length -1;
			
			for (int i = 0; i < count; i++)
				query = query.ReplaceFirstInstance("?", "@"+i.ToString());
			
			return query;
			
		}
		
		// -------------------------------------------------------------------------------
		// IsPK
		// -------------------------------------------------------------------------------
        protected bool IsPK (MemberInfo p)
		{
			return p.CustomAttributes.Any (x => x.AttributeType == typeof (PrimaryKeyAttribute));
		}
		
        // -------------------------------------------------------------------------------
		// IsAutoInc
		// -------------------------------------------------------------------------------
        protected  bool IsAutoInc (MemberInfo p)
		{
			return p.CustomAttributes.Any (x => x.AttributeType == typeof (AutoIncrementAttribute));
		}
		
		// TODO: No Collate
        
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================