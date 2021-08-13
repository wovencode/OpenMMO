
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
    /// <summary>
    /// Public Partial Class <c>DatabaseManager</c>
    /// Contains Methods and Functions that interact with the database
    /// Supports Mysql and Sqlite3 out of the box.
    /// Other database engines require further setup.
    /// </summary>
	public partial class DatabaseManager
	{
		
		// -------------------------------------------------------------------------------
		// SavePlayers
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Method <c>SavePlayers_Network</c>.
        /// Saves the player data for each player on the network and commits it to the database.
        /// </summary>
		[DevExtMethods(nameof(SavePlayers))]
		void SavePlayers_Network()
    	{

    		if (OpenMMO.Network.NetworkManager.onlinePlayers.Count == 0)
    			return; 
    		
        	databaseLayer.BeginTransaction();
        	
        	foreach (GameObject player in OpenMMO.Network.NetworkManager.onlinePlayers.Values)
        		if (player != null)
            		SaveDataPlayer(player, false, false); // isNew = false / Transaction = false
            
        	databaseLayer.Commit();
        	
        	if (OpenMMO.Network.NetworkManager.onlinePlayers.Count > 0)
        		debug.LogFormat(this.name, nameof(SavePlayers_Network), OpenMMO.Network.NetworkManager.onlinePlayers.Count.ToString());

    	}
    	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================