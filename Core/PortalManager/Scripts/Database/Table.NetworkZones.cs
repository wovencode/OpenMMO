
using OpenMMO;
using OpenMMO.Database;
using System;
using SQLite;

namespace OpenMMO.Database
{

	// -------------------------------------------------------------------------------
	// TablePlayerZones
	// -------------------------------------------------------------------------------
	public partial class TablePlayerZones
	{
		[PrimaryKey]
		public string 	player 	{ get; set; }
		public string 	zone 	{ get; set; }
	}
	
	// -------------------------------------------------------------------------------
	// TableNetworkZones
	// -------------------------------------------------------------------------------
	public partial class TableNetworkZones
	{
		[PrimaryKey]
		public string	zone 	{ get; set; }
		public string 	online 	{ get; set; }
		public int 		players { get; set; }
	}
	
	// -----------------------------------------------------------------------------------
	
}

// =======================================================================================