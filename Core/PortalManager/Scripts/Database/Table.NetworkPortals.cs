
using OpenMMO;
using OpenMMO.Database;
using System;
using SQLite;

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
		public int		token		{ get; set; }
		
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
		public string 	online 	{ get; set; }
	}
	
	// -----------------------------------------------------------------------------------
	
}

// =======================================================================================