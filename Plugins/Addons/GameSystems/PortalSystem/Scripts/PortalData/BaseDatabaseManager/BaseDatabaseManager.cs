
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Debugging;
using UnityEngine;
using System;

namespace OpenMMO.Database
{
	
	// ===================================================================================
	// BaseDatabase
	// ===================================================================================
	public abstract partial class BaseDatabaseManager
	{
		
		// -------------------------------------------------------------------------------
		// TryPlayerAutoLogin
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerAutoLogin(string playername, string username)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		// TryPlayerSwitchServer
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerSwitchServer(string playername, string anchorname, string zonename, int token)
		{
			return (Tools.IsAllowedName(playername) && !String.IsNullOrWhiteSpace(anchorname) && !String.IsNullOrWhiteSpace(zonename) && token >= 1000 && token <= 9999);
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================