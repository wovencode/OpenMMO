//by Fhiz
using UnityEngine;
using Mirror;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;
using UnityEngine.AI;
using OpenMMO;
using OpenMMO.Database;

namespace OpenMMO.Database
{

	/// <summary>
	/// This partial section of DatabaseManager is responsible to save and load level data of players.
	/// </summary>
	public partial class DatabaseManager
	{
	
		/// <summary>
		/// Hooks into Init. Creates the Level table if it does not exist yet.
		/// </summary>
		[DevExtMethods(nameof(Init))]
		void Init_Level()
		{
			CreateTable<TablePlayerLevel>();
        	CreateIndex(nameof(TablePlayerLevel), new []{"owner", "name"});
		}
		
		/// <summary>
		/// Hooks into LoadDataPlayerPriority to load levels first, because inventory size (etc.) could depend on them.
		/// </summary>
		[DevExtMethods(nameof(LoadDataPlayerPriority))]
		void LoadDataPlayerPriority_Level(GameObject player)
		{
			
			Component[] components = player.GetComponents<LevelableComponent>();
			
			List<TablePlayerLevel> levels = Query<TablePlayerLevel>("SELECT * FROM "+nameof(TablePlayerLevel)+" WHERE owner=?", player.name);
			
			if (levels == null || levels.Count < 1) return;
			
	   		foreach (TablePlayerLevel row in levels)
			{
				foreach (Component component in components)
	   			{
	   				if (component is LevelableComponent)
	   				{
	   				
	   					LevelableComponent manager = (LevelableComponent)component;
	   				
	   					if (manager.GetType().ToString() == row.name)
	   						manager.level = row.level;
	   					
	   				}
	   			}
			}
		}
		
		/// <summary>
		/// Hooks into SaveDataPlayer. Saves all levels of all components on a player.
		/// </summary>
		[DevExtMethods(nameof(SaveDataPlayer))]
		void SaveDataPlayer_Level(GameObject player, bool isNew)
		{
		
			// you should delete all data of this player first, to prevent duplicates
	   		DeleteDataPlayer_Level(player.name);
	   		
	   		Component[] components = player.GetComponents<LevelableComponent>();
	   		
	   		foreach (Component component in components)
	   		{
	   			if (component is LevelableComponent)
	   			{
	   			
	   				LevelableComponent lc = (LevelableComponent)component;
	   			
	   				InsertOrReplace(new TablePlayerLevel{
                		owner 			= player.name,
                		name 			= lc.GetType().ToString(),
                		level 			= lc.level
            		});
            	
            	}
	   		}
		
		}
		
		/// <summary>
		/// Hooks into DeleteDataPlayer. Deletes all component level data from the database.
		/// </summary>
	   	[DevExtMethods(nameof(DeleteDataPlayer))]
	   	void DeleteDataPlayer_Level(string _name)
	   	{
	   		Execute("DELETE FROM "+nameof(TablePlayerLevel)+" WHERE owner=?", _name);
	   	}
		
	}

}