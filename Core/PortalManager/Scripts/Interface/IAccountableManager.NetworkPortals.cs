
using System;
using UnityEngine;
using OpenMMO;
using OpenMMO.Database;

namespace OpenMMO.Database
{

	// ===================================================================================
	// IAccountableManager
	// ===================================================================================
	public partial interface IAccountableManager
	{
		
		bool TryPlayerSwitchServer(string playername, string anchorname, string zonename, int token);
		
	}
		
}

// =======================================================================================