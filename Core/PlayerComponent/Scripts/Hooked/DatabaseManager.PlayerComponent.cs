
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{
	
	// ===================================================================================
	// Database
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// -------------------------------------------------------------------------------
		// DeleteUsers_PlayerComponent
		// -------------------------------------------------------------------------------
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
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================