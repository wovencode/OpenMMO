
using OpenMMO;
using OpenMMO.Database;
using System;
using SQLite;
using UnityEngine;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class TablePlayer
	{
		[PrimaryKey]
		[Collation("NOCASE")]
		public string playername	{ get; set; }
		public string username		{ get; set; }
		public DateTime created 	{ get; set; }
		public DateTime lastonline 	{ get; set; }
		public bool deleted 		{ get; set; }
		public bool banned 			{ get; set; }
		public DateTime lastsaved 	{ get; set; }
		public float cooldown		{ get; set; }
		
		public string prefab		{ get; set; }
		public float x 				{ get; set; }
		public float y 				{ get; set; }
		public float z 				{ get; set; }
		
		// -------------------------------------------------------------------------------
		// Create
		// -------------------------------------------------------------------------------
		public void Create(GameObject player, string userName="", string prefabName="")
		{

			Update(player, userName);
			
			created = DateTime.UtcNow;
			lastonline = DateTime.UtcNow;
			
			prefab = prefabName;
			
		}
				
		// -------------------------------------------------------------------------------
		// Update
		// -------------------------------------------------------------------------------
		public void Update(GameObject player, string userName="")
		{
			
			playername = player.name;
			
			if (!String.IsNullOrWhiteSpace(userName))
				username = userName;
			
			lastsaved = DateTime.UtcNow;
			lastonline = DateTime.UtcNow;
			
			cooldown = player.GetComponent<PlayerComponent>().GetCooldownTimeRemaining();
			
			x = player.transform.position.x;
			y = player.transform.position.y;
			z = player.transform.position.z;
			
		}
		
		// ---------------------------------------------------------------------------
		
	}

}

// =======================================================================================