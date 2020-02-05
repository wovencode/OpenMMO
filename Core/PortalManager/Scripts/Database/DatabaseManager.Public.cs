
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
	
	   	// -------------------------------------------------------------------------------
	   	// SaveZoneTime
	   	// -------------------------------------------------------------------------------
	   	public void SaveZoneTime(string zoneName, int playerCount)
	   	{
	   		
	   		// delete data first, to prevent duplicates
	   		Execute("DELETE FROM "+nameof(TableNetworkZones)+" WHERE zone=?", zoneName);
	   		
	   		string onlineString = DateTime.UtcNow.ToString("s");
	   		
	   		InsertOrReplace(new TableNetworkZones{
                	zone 			= zoneName,
                	online 			= onlineString,
                	players 		= playerCount,
            });
	   			   		
	   	}
	   	
	   	// -------------------------------------------------------------------------------
	   	// LoadZoneTime
	   	// Returns the number of seconds this zone has been online
	   	// Usually only used for the main zone
	   	// -------------------------------------------------------------------------------
	   	public double LoadZoneTime(string zoneName)
	   	{
	   		
	   		TableNetworkZones row = FindWithQuery<TableNetworkZones>("SELECT * FROM "+nameof(TableNetworkZones)+" WHERE zone=?", zoneName);
	   	
	   		if (row != null)
	   			return (DateTime.UtcNow - DateTime.Parse(row.online)).TotalSeconds;
	   		
	   		return Mathf.Infinity;
	   		
	   	}
	   	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================