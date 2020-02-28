// by Fhiz
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data.MySqlClient;
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Debugging;

namespace OpenMMO.Database {
	
	/// <summary>
	/// [PrimaryKey] This property attribute is used to mark a field as "Primary Key"
	/// </summary>
	[AttributeUsage (AttributeTargets.Property)]
	public class PrimaryKeyAttribute : Attribute {}
	
	/// <summary>
	/// [AutoIncrement] This property attribute is used to mark a field as "Auto Increment"
	/// </summary>
	[AttributeUsage (AttributeTargets.Property)]
	public class AutoIncrementAttribute : Attribute {}
	
	/// <summary>
	/// 
	/// </summary>
	public class TableMap
	{
		
		public Type type;
		public string name;
		public TableRow[] rows;
		
		protected string mySQLString			= "";
		protected string mySQLString_Prefixed 	= "";
		
		/// <summary>
		/// Constructor sets type, name and creates a set of rows.
		/// </summary>
		public TableMap(Type _type, string _name, int rowCount)
		{
			type = _type;
			name = _name;
			rows = new TableRow[rowCount];
		}
		
		/// <summary>
		/// Iterates all rows of this table and returns them as mySQL compliant query string (usually used for Insert).
		/// </summary>
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
		
		/// <summary>
		/// Iterates all rows of this table and returns them as mySQL compliant query string.
		/// </summary>
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
		
		/// <summary>
		/// Converts all rows of this table to an array of mySQL parameters (usually used as param args).
		/// </summary>
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
		
		/// <summary>
		/// Checks if any field of this table has the "Primary Key" attribute.
		/// </summary>
		public bool HasPrimaryKey
		{
			get
			{
				foreach (TableRow row in rows)
					if (row.primary) return true;
				return false;
			}
		}
		
		/// <summary>
		/// Returns the field that has the "Primary Key" attribute.
		/// </summary>
		public string GetPrimaryKey
		{
			get
			{
				foreach (TableRow row in rows)
					if (row.primary) return row.name;
				return "";
			}
		}
		
		/// <summary>
		/// Updates the value stored in the field of the given name. Where obj is a property value (like an integer or a string).
		/// </summary>
		public void UpdateValue(string rowname, object obj)
		{
			foreach (TableRow row in rows)
				if (row.name == rowname)
					row.value = obj;
		}
		
		/// <summary>
		/// Updates all values in the fields of this table according to obj. Where obj is a TableClass that has to match this table.
		/// </summary>
		public void UpdateValues(object obj)
		{
		
			PropertyInfo[] pInfo;
			pInfo = obj.GetType().GetProperties();
			
			for (int i = 0; i < pInfo.Length; i++)
				rows[i].value = pInfo[i].GetValue(obj);

		}
		
		/// <summary>
		/// Converts this TableMap into a type T. Where T is a TableClass.
		/// </summary>
		public T ToType<T>()
		{
		
			T result = (T)Activator.CreateInstance(typeof(T));
		
			PropertyInfo[] pInfo0;
			pInfo0 = result.GetType().GetProperties();
			
			for (int i = 0; i < pInfo0.Length; i++)
				pInfo0[i].SetValue(result, rows[i].value);
				
			return result;
			
		}
		
	}
	
	/// <summary>
	/// Used to represent the fields in a table row. Each field can be used like a class property or a mySQL query string.
	/// </summary>
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
		
		/// <summary>
		/// Converts the name and type of this field into a mySQL compliant query string.
		/// </summary>
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
		
	}
		
}