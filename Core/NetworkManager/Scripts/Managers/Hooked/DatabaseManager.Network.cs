
using Wovencode;
using Wovencode.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace Wovencode.Database
{
	
	// ===================================================================================
	// Database
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// -------------------------------------------------------------------------------
		// SavePlayers
		// -------------------------------------------------------------------------------
		[DevExtMethods("SavePlayers")]
		void SavePlayers_Network(bool online = true)
    	{

    		if (Wovencode.Network.NetworkManager.onlinePlayers.Count == 0)
    			return; 
    		
        	databaseLayer.BeginTransaction();
        	
        	foreach (GameObject player in Wovencode.Network.NetworkManager.onlinePlayers.Values)
            	SaveDataPlayer(player, online, false);
            
        	databaseLayer.Commit();
        	
        	if (Wovencode.Network.NetworkManager.onlinePlayers.Count > 0)
        		debug.Log("[Database] Saved " + Wovencode.Network.NetworkManager.onlinePlayers.Count + " player(s)");

    	}
    	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================