
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.DebugManager;
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
		public virtual bool TryPlayerAutoLogin(string playername, string username)
		{
		Debug.Log("TryPlayerAutoLogin: "+playername+"/"+username);
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username) );
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerSwitchServer(string playername, string anchorname, string zonename, int token)
		{
			return (Tools.IsAllowedName(playername) && !String.IsNullOrWhiteSpace(anchorname) && !String.IsNullOrWhiteSpace(zonename) && token >= 1000 && token <= 9999);
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================