
using Wovencode;
using Wovencode.Database;
using System;
using SQLite;
using UnityEngine;

namespace Wovencode.Database
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
		public DateTime lastlogin 	{ get; set; }
		public bool deleted 		{ get; set; }
		public bool banned 			{ get; set; }
		public bool online 			{ get; set; }
		public DateTime lastsaved 	{ get; set; }
		public int token			{ get; set; }
		
		public string prefab		{ get; set; }
		public float x 				{ get; set; }
		public float y 				{ get; set; }
		public float z 				{ get; set; }
		
		// -------------------------------------------------------------------------------
		// Create
		// -------------------------------------------------------------------------------
		public void Create(GameObject player, string userName="", string prefabName="")
		{

			Update(player, false, userName);
			
			created = DateTime.UtcNow;
			lastlogin = DateTime.UtcNow;
			
			prefab = prefabName;
			
		}
				
		// -------------------------------------------------------------------------------
		// Update
		// -------------------------------------------------------------------------------
		public void Update(GameObject player, bool isOnline, string userName="")
		{
			
			playername = player.name;
			
			if (!String.IsNullOrWhiteSpace(userName))
				username = userName;
			
			online = isOnline;
			lastsaved = DateTime.UtcNow;
			
			x = player.transform.position.x;
			y = player.transform.position.y;
			z = player.transform.position.z;
			
		}
		
		// ---------------------------------------------------------------------------
		
	}

}

// =======================================================================================