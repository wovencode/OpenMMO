
using OpenMMO;
using OpenMMO.Database;
using System;
using SQLite;

namespace OpenMMO.Database
{

	// ===================================================================================
	// TableUser
	// ===================================================================================
	public partial class TableUser
	{
		[PrimaryKey]
		public string username		{ get; set; }
		public string password 		{ get; set; }
		public string email			{ get; set; }
		public string deviceid		{ get; set; }
		public DateTime created 	{ get; set; }
		public DateTime lastonline 	{ get; set; }
		public bool deleted 		{ get; set; }
		public bool banned 			{ get; set; }
		public bool confirmed		{ get; set; }
		public DateTime lastsaved 	{ get; set; }
		public int token			{ get; set; }
		public int maxPlayers		{ get; set; }
		
	}
	
	// -------------------------------------------------------------------------------
	
}