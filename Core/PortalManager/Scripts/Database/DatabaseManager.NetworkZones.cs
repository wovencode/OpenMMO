
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
		// Init
		// -------------------------------------------------------------------------------
		[DevExtMethods("Init")]
		void Init_NetworkZones()
		{
	   		CreateTable<TablePlayerZones>();
        	CreateTable<TableNetworkZones>();
		}
		
		// -------------------------------------------------------------------------------
		// CreateDefaultDataPlayer
		// -------------------------------------------------------------------------------
		[DevExtMethods("CreateDefaultDataPlayer")]
		void CreateDefaultDataPlayer_NetworkZones(GameObject player)
		{
	 		/*
	 			Fills the table with default data (if any)
	 			You should use a GetComponent call (for example to your "Inventory")
	 			And then fill the inventory with "DefaultItems"
	 			No need to save them in the database right away
	 			As the playerSaving or next saveInterval will take care of it
	 		*/
	 		
		}
		
		// -------------------------------------------------------------------------------
		// LoadDataPlayerPriority
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadDataPlayerPriority")]
		void LoadDataPlayerPriority_NetworkZones(GameObject player)
		{
	   		/*
	   			The difference to "LoadData" is, that "LoadDataWithPriority" is executed
	   			first. This allows you to load data beforehand, that is required by other
	   			data (like the "level" of a player to set its "inventory size" before
	   			loading that players actual "inventory").
	   		*/
		}
		
		// -------------------------------------------------------------------------------
		// LoadDataPlayer
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadDataPlayer")]
		void LoadDataPlayer_NetworkZones(GameObject player)
		{
			
			/*
				
				This function loads any kind of data from the database and adds it to the
				player GameObject.
							
				Usage Example:
			*/
			
	   		/*
	   		InventoryManager manager = player.GetComponent<InventoryManager>();
	   		
			foreach (TableExample row in Query<TableExample>("SELECT * FROM TableExample WHERE owner=?", player.name))
			{
				if (ItemTemplate.data.TryGetValue(row.hash, out ItemTemplate template))
				{
					manager.AddEntry(row.owner, row.name, row.amount);
				}
				else Debug.LogWarning("[Load] Skipped item " + row.name + " for " + player.name + " as it was not found in Resources.");
			}
			*/
		}
		
		// -------------------------------------------------------------------------------
		// SaveDataPlayer
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveDataPlayer")]
		void SaveDataPlayer_NetworkZones(GameObject player, bool isOnline)
		{
		
			// you should delete all data of this player first, to prevent duplicates
	   		DeleteDataPlayer_NetworkZones(player.name);
	   		
	   		InsertOrReplace(
	   				new TablePlayerZones
	   				{
                		player 			= player.name,
                		zone 			= player.GetComponent<PlayerComponent>().currentZone.name
            		}
            );
	   		
		}
	   	
	   	// -------------------------------------------------------------------------------
	   	// DeleteDataPlayer_Example
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("DeleteDataPlayer")]
	   	void DeleteDataPlayer_NetworkZones(string _name)
	   	{
	   		Execute("DELETE FROM "+nameof(TablePlayerZones)+" WHERE player=?", _name);
	   	}
	   	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================