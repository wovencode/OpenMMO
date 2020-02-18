
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
	   	// CheckZoneTimeout
	   	// -------------------------------------------------------------------------------
		public bool CheckZoneTimeout(string zoneName, float timeoutInterval)
		{
			
			TableNetworkZones row = FindWithQuery<TableNetworkZones>("SELECT * FROM "+nameof(TableNetworkZones)+" WHERE zone=?", zoneName);
			
			if (row != null)
			{
			
				double timePassed = (DateTime.UtcNow - row.online).TotalSeconds;

				// -- enough time passed = shutdown
				if (timePassed > timeoutInterval)
					return true;
				
				// -- if we reach this = do not shutdown
				return false;
				
			}
			
			// -- in any other case = shutdown
			return true;
		
		}
		
	   	// -------------------------------------------------------------------------------
	   	// SaveZoneTime
	   	// -------------------------------------------------------------------------------
	   	public void SaveZoneTime(string zoneName)
	   	{
	   		
	   		// delete data first, to prevent duplicates
	   		Execute("DELETE FROM "+nameof(TableNetworkZones)+" WHERE zone=?", zoneName);
	   		
	   		InsertOrReplace(new TableNetworkZones{
                	zone 			= zoneName,
                	online 			= DateTime.UtcNow
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
	   			return (DateTime.UtcNow - row.online).TotalSeconds;
	   		
	   		return Mathf.Infinity;
	   		
	   	}
	   	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================