
using OpenMMO;
using OpenMMO.Database;
using System;
using SQLite;
using UnityEngine;

namespace OpenMMO.Database
{

	// -----------------------------------------------------------------------------------
	// TablePlayerZones
	// -----------------------------------------------------------------------------------
	public partial class TablePlayerZones
	{
		[PrimaryKey]
		public string 	playername 	{ get; set; }
		public string 	zonename 	{ get; set; }
		public string	anchorname  { get; set; }
		public bool		startpos	{ get; set; }
		public int		token		{ get; set; }
		
		// -------------------------------------------------------------------------------
		// ValidateToken
		// -------------------------------------------------------------------------------
		public bool ValidateToken(int _token)
		{
			return (token == _token);
		}
		
	}
	
	// -----------------------------------------------------------------------------------
	// TableNetworkZones
	// -----------------------------------------------------------------------------------
	public partial class TableNetworkZones
	{
		[PrimaryKey]
		public string	zone 	{ get; set; }
		public DateTime online 	{ get; set; }
	}
	
	// -----------------------------------------------------------------------------------
	
}

// =======================================================================================