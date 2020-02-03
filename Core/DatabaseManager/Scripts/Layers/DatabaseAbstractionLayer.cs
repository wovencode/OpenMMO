
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace OpenMMO.Database
{
	
	// ===================================================================================
	// DatabaseAbstractionLayer
	// ===================================================================================
	[System.Serializable]
	public abstract partial class DatabaseAbstractionLayer : IAbstractableDatabase
	{

		public abstract void Init();
		public abstract void OpenConnection();
		public abstract void CloseConnection();
		public abstract void CreateTable<T>();
		public abstract void CreateIndex(string tableName, string[] columnNames, bool unique = false);
		public abstract List<T> Query<T>(string query, params object[] args) where T : new();
		public abstract void Execute(string query, params object[] args);
		public abstract T FindWithQuery<T>(string query, params object[] args)  where T : new();
		public abstract void Insert(object obj);
		public abstract void InsertOrReplace(object obj);
		public abstract void BeginTransaction();
		public abstract void Commit();
		public abstract void OnValidate();
		
	}

}

// =======================================================================================