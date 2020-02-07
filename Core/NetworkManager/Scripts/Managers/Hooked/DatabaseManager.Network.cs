
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
		// SavePlayers
		// -------------------------------------------------------------------------------
		[DevExtMethods("SavePlayers")]
		void SavePlayers_Network()
    	{

    		if (OpenMMO.Network.NetworkManager.onlinePlayers.Count == 0)
    			return; 
    		
        	databaseLayer.BeginTransaction();
        	
        	foreach (GameObject player in OpenMMO.Network.NetworkManager.onlinePlayers.Values)
            	SaveDataPlayer(player, false);
            
        	databaseLayer.Commit();
        	
        	if (OpenMMO.Network.NetworkManager.onlinePlayers.Count > 0)
        		debug.Log("[Database] Saved " + OpenMMO.Network.NetworkManager.onlinePlayers.Count + " player(s)");

    	}
    	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================