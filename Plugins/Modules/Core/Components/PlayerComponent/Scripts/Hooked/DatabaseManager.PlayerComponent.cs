//by  Fhiz
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{
	
	/// <summary>
	/// This partial section of DataBaseManager removes soft-deleted users (= accounts) permanently from the database in regular intervals.
	/// </summary>
	public partial class DatabaseManager
	{
		
		/// <summary>
		/// Hooks into the DeleteUsers function that is called in regular intervals, to permanently delete soft-deleted users (= accounts).
		/// </summary>
		[DevExtMethods(nameof(DeleteUsers))]
		void DeleteUsers_PlayerComponent()
		{

			List<TableUser> users = Query<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE deleted=1");
			
			if (users == null)
				return;
			
			foreach (TableUser user in users)
				this.InvokeInstanceDevExtMethods(nameof(DeleteUser), user.username); //HOOK
			
			if (users.Count > 0)
				debug.Log("[DatabaseManager] Pruned " + users.Count + " inactive user(s)");

		}
		
	}

}