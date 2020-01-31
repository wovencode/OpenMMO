
using Wovencode;
using Wovencode.Database;
using UnityEngine;
using System;
using System.Collections.Generic;
using SQLite;

namespace Wovencode.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
	
		// -------------------------------------------------------------------------------
		// Init
		// Creates the table using the provided table structure, if it does not exist
		// -------------------------------------------------------------------------------
		[DevExtMethods("Init")]
		void Init_Example()
		{
	   		CreateTable<TableExample>();
        	CreateIndex(nameof(TableExample), new []{"owner", "name", "amount"});
		}
		
		// -------------------------------------------------------------------------------
		// CreateDefaultDataPlayer
		// -------------------------------------------------------------------------------
		[DevExtMethods("CreateDefaultDataPlayer")]
		void CreateDefaultDataPlayer_Example(GameObject player)
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
		void LoadDataPlayerPriority_Example(GameObject player)
		{
	   		/*
	   			The difference to "LoadData" is, that "LoadDataWithPriority" is executed
	   			first. This allows you to load data beforehand, that is required by other
	   			data (like the "level" of a player to set its "inventory size" before
	   			loading that players actual "inventory").
	   		*/
		}
		
		// -------------------------------------------------------------------------------
		// LoadDataPlayer_Example
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadDataPlayer")]
		void LoadDataPlayer_Example(GameObject player)
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
				if (ItemTemplate.dict.TryGetValue(row.hash, out ItemTemplate template))
				{
					manager.AddEntry(row.owner, row.name, row.amount);
				}
				else Debug.LogWarning("[Load] Skipped item " + row.name + " for " + player.name + " as it was not found in Resources.");
			}
			*/
		}
		
		// -------------------------------------------------------------------------------
		// SaveDataPlayer_Example
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveDataPlayer")]
		void SaveDataPlayer_Example(GameObject player, bool isOnline)
		{
		
			// you should delete all data of this player first, to prevent duplicates
	   		DeleteDataPlayer_Example(player.name);
	   		
	   		/*
	   			This function saves any kind of data from your player object to the
	   			database. You should use a GetComponent call (for example "Inventory")
	   			And then iterate all "Items" therein with a foreach and add them to
	   			the database.
	   			
	   			Usage Example:
	   		*/
	   		
	   		/*
	   		InventoryManager manager = player.GetComponent<InventoryManager>();
	   		
	   		List<ItemSyncStruct> list = manager.GetEntries(false);
	   		
	   		for (int i = 0; i < list.Count; i++)
	   		{
	   			
	   			ItemSyncStruct entry = list[i];
	   			
	   			InsertOrReplace(new TableExample{
                	owner 			= player.name,
                	name 			= entry.name,
                	amount 			= entry.amount,
            	});
	   		}
	   		*/
		}
	   	
	   	// -------------------------------------------------------------------------------
	   	// DeleteDataPlayer_Example
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("DeleteDataPlayer")]
	   	void DeleteDataPlayer_Example(string _name)
	   	{
	   		Execute("DELETE FROM "+nameof(TableExample)+" WHERE owner=?", _name);
	   	}
	   	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================