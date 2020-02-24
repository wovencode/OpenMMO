
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
		
		public float roty			{ get; set; }
		// TODO: do we need rotation x and z ?
		
		// -------------------------------------------------------------------------------
		// Create
		// -------------------------------------------------------------------------------
		public void Create(GameObject player, string userName="", string prefabName="")
		{

			Update(player, userName);
			
			prefab = prefabName;
			
			created = DateTime.UtcNow;			// has to be updated here, otherwise it never happens
			
		}
				
		// -------------------------------------------------------------------------------
		// Update
		// -------------------------------------------------------------------------------
		public void Update(GameObject player, string userName="")
		{
			
			lastsaved = DateTime.UtcNow;		// has to be updated here, otherwise it never happens
			
			playername = player.name;
			
			if (!String.IsNullOrWhiteSpace(userName))
				username = userName;
			
			cooldown = player.GetComponent<PlayerComponent>().GetCooldownTimeRemaining();
			
			x = player.transform.position.x;
			y = player.transform.position.y;
			z = player.transform.position.z;
			
			roty = player.transform.rotation.eulerAngles.y;
			
		}
		
		// ---------------------------------------------------------------------------
		
	}

}

// =======================================================================================