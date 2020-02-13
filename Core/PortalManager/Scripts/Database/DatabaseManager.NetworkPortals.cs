
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
			TablePlayerZones tablePlayerZones = FindWithQuery<TablePlayerZones>("SELECT * FROM "+nameof(TablePlayerZones)+" WHERE playername=?", player.name);
	   		player.GetComponent<PlayerComponent>().tablePlayerZones = tablePlayerZones;
		}
		
		// -------------------------------------------------------------------------------
		// SaveDataPlayer
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveDataPlayer")]
		void SaveDataPlayer_NetworkZones(GameObject player)
		{
		
			// you should delete all data of this player first, to prevent duplicates
	   		DeleteDataPlayer_NetworkZones(player.name);
	   		
	   		InsertOrReplace(
	   				new TablePlayerZones
	   				{
                		playername 		= player.name,
                		zonename 		= player.GetComponent<PlayerComponent>().tablePlayerZones.zonename,
                		anchorname 		= player.GetComponent<PlayerComponent>().tablePlayerZones.anchorname,
                		token			= player.GetComponent<PlayerComponent>().GetToken
            		}
            );
	   		
		}
	   	
	   	// -------------------------------------------------------------------------------
	   	// DeleteDataPlayer_Example
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("DeleteDataPlayer")]
	   	void DeleteDataPlayer_NetworkZones(string _name)
	   	{
	   		Execute("DELETE FROM "+nameof(TablePlayerZones)+" WHERE playername=?", _name);
	   	}
	   	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================