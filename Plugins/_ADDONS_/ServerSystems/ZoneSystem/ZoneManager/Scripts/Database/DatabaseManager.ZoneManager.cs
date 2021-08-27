//By Fhiz
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
		[DevExtMethods(nameof(Init))]
		void Init_NetworkZones()
		{
	   		CreateTable<TablePlayerZones>();
        	CreateTable<TableNetworkZones>();
		}
		
		// -------------------------------------------------------------------------------
		// CreateDefaultDataPlayer
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(CreateDefaultDataPlayer))]
		void CreateDefaultDataPlayer_NetworkZones(GameObject player)
		{
	 		player.GetComponent<PlayerAccount>().tablePlayerZones = new TablePlayerZones
	   		{
                playername 		= player.name,
                zonename 		= player.GetComponent<PlayerAccount>().startingZone.name,
                anchorname 		= "",
                startpos		= true,
                token			= 0
            };
		}
		
		// -------------------------------------------------------------------------------
		// LoadDataPlayerPriority
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoadDataPlayerPriority))]
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
		[DevExtMethods(nameof(LoadDataPlayer))]
		void LoadDataPlayer_NetworkZones(GameObject player)
		{
			TablePlayerZones tablePlayerZones = FindWithQuery<TablePlayerZones>("SELECT * FROM "+nameof(TablePlayerZones)+" WHERE playername=?", player.name);
	   		player.GetComponent<PlayerAccount>().zoneInfo = tablePlayerZones;
		}
		
		// -------------------------------------------------------------------------------
		// SaveDataPlayer
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(SaveDataPlayer))]
		void SaveDataPlayer_NetworkZones(GameObject player, bool isNew)
		{
		
			// -- delete all data of this player first, to prevent duplicates
	   		DeleteDataPlayer_NetworkZones(player.name);

            PlayerAccount pc = player.GetComponent<PlayerAccount>(); //ADDED DX4D

	   		InsertOrReplace(
	   				new TablePlayerZones
	   				{
                		playername 		= player.name,
                		zonename 		= pc.zoneInfo.zonename,
                		anchorname 		= pc.zoneInfo.anchorname,
                		startpos		= pc.zoneInfo.startpos,
                		token			= pc.GetToken
            		}
            );
	   		
		}
	   	
	   	// -------------------------------------------------------------------------------
	   	// DeleteDataPlayer_Example
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(DeleteDataPlayer))]
	   	void DeleteDataPlayer_NetworkZones(string _name)
	   	{
	   		Execute("DELETE FROM "+nameof(TablePlayerZones)+" WHERE playername=?", _name);
	   	}
	   	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================