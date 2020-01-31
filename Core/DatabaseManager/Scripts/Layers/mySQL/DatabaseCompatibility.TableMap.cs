using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data;
using MySql.Data.MySqlClient;
using Wovencode;
using Wovencode.Database;
using Wovencode.DebugManager;

namespace Wovencode.Database {
	
	// ===================================================================================
	// PrimaryKeyAttribute
	// ===================================================================================
	[AttributeUsage (AttributeTargets.Property)]
	public class PrimaryKeyAttribute : Attribute
	{
	}
	
	// ===================================================================================
	// AutoIncrementAttribute
	// ===================================================================================
	[AttributeUsage (AttributeTargets.Property)]
	public class AutoIncrementAttribute : Attribute
	{
	}
	
	// ===================================================================================
	// TableMap
	// ===================================================================================
	public class TableMap
	{
		
		public Type type;
		public string name;
		public TableRow[] rows;
		
		// Caches
		protected string mySQLString			= "";
		protected string mySQLString_Prefixed 	= "";
		
		// -------------------------------------------------------------------------------
		// TableMap (Constructor)
		// -------------------------------------------------------------------------------
		public TableMap(Type _type, string _name, int rowCount)
		{
			type = _type;
			name = _name;
			rows = new TableRow[rowCount];
		}
		
		// -------------------------------------------------------------------------------
		// RowsToMySQLInsertString
		// -------------------------------------------------------------------------------
		public string RowsToMySQLInsertString
		{
			get
			{
				
				string tableParameters = "";
			
				foreach (TableRow row in rows)
				{
			
					tableParameters += row.ToMySQLString;
					tableParameters += " NOT NULL";
				
					if (row != rows.Last())
						tableParameters += ",";
					
				}
				
				return tableParameters;
			}
		}
		
		// -------------------------------------------------------------------------------
		// RowsToMySQLString
		// -------------------------------------------------------------------------------
		public string RowsToMySQLString(string prefix="")
		{
		
			string convertedString = "";
			
			if (!String.IsNullOrWhiteSpace(prefix))
				convertedString = mySQLString_Prefixed;
			else
				convertedString = mySQLString;
						
			if (String.IsNullOrWhiteSpace(convertedString))
			{
			
				foreach (TableRow row in rows)
				{
					
					if (!String.IsNullOrWhiteSpace(prefix))
						convertedString += prefix + row.name;
					else
						convertedString += "`" + row.name + "`";
			
					if (row != rows.Last())
						convertedString += ",";
				
				}
				
				if (!String.IsNullOrWhiteSpace(prefix))
					mySQLString_Prefixed = convertedString;
				else
					mySQLString = convertedString;
				
			}
			
			return convertedString;
			
		}
		
		// -------------------------------------------------------------------------------
		// RowsToMySQLParameters
		// -------------------------------------------------------------------------------
		public MySqlParameter[] RowsToMySQLParameters
		{
			get
			{
				
				MySqlParameter[] parameters = new MySqlParameter[rows.Length];
				
				for (int i = 0; i < rows.Length; i++)
					parameters[i] = new MySqlParameter("@"+rows[i].name, rows[i].value);
				
				return parameters;
				
			}
		}
		
		// -------------------------------------------------------------------------------
		// HasPrimaryKey
		// -------------------------------------------------------------------------------
		public bool HasPrimaryKey
		{
			get
			{
				foreach (TableRow row in rows)
					if (row.primary) return true;
				return false;
			}
		}
		
		// -------------------------------------------------------------------------------
		// GetPrimaryKey
		// -------------------------------------------------------------------------------
		public string GetPrimaryKey
		{
			get
			{
				foreach (TableRow row in rows)
					if (row.primary) return row.name;
				return "";
			}
		}
		
		// -------------------------------------------------------------------------------
		// UpdateValue
		// -------------------------------------------------------------------------------
		public void UpdateValue(string rowname, object obj)
		{
			foreach (TableRow row in rows)
				if (row.name == rowname)
					row.value = obj;
		}
		
		// -------------------------------------------------------------------------------
		// UpdateValues
		// -------------------------------------------------------------------------------
		public void UpdateValues(object obj)
		{
		
			PropertyInfo[] pInfo;
			pInfo = obj.GetType().GetProperties();
			
			for (int i = 0; i < pInfo.Length; i++)
				rows[i].value = pInfo[i].GetValue(obj);

		}
		
		// -------------------------------------------------------------------------------
		// ToType
		// -------------------------------------------------------------------------------
		public T ToType<T>()
		{
		
			T result = (T)Activator.CreateInstance(typeof(T));
		
			PropertyInfo[] pInfo0;
			pInfo0 = result.GetType().GetProperties();
			
			for (int i = 0; i < pInfo0.Length; i++)
				pInfo0[i].SetValue(result, rows[i].value);
				
			return result;
			
		}
		
		// -------------------------------------------------------------------------------
		
	}
	
	// ===================================================================================
	// TableRow
	// ===================================================================================
	public class TableRow
	{
		public string name;
		public Type type;
		public object value;
		public bool primary;
				
		const string typeInt 		= " INT";
		const string typeBool		= " BOOLEAN";
		const string typeLong 		= " BIGINT";
		const string typeString 	= " VARCHAR(64)";
		const string typeDateTime 	= " DATETIME";
		const string typeFloat		= " FLOAT";
		const string typeDouble		= " DOUBLE";
		
		// -------------------------------------------------------------------------------
		// ToMySQLString
		// -------------------------------------------------------------------------------
		public string ToMySQLString
		{
			get
			{
				if (type == typeof(int))
				{
					return  "`" + name + "`" + typeInt;
				}
				else if (type == typeof(bool))
				{
					return "`" + name + "`" + typeBool;
				}		
				else if (type == typeof(long))
				{
					return "`" + name + "`" + typeLong;
				}		
				else if (type == typeof(string))
				{
					return "`" + name + "`" + typeString;
				}		
				else if (type == typeof(DateTime))
				{
					return "`" + name + "`" + typeDateTime;
				}
				else if (type == typeof(float))
				{
					return "`" + name + "`" + typeFloat;
				}
				else if (type == typeof(double))
				{
					return "`" + name + "`" + typeDouble;
				}
				
				return "";
			}
		}
		
		// -------------------------------------------------------------------------------
		
	}
		
}

// =======================================================================================