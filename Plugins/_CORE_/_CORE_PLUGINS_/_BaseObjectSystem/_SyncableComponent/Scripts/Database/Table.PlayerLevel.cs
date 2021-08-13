//by Fhiz
using OpenMMO;
using OpenMMO.Database;
using System;
using SQLite;

namespace OpenMMO.Database
{

	/// <summary>
	/// TablePlayerLevel contains all data related to the level of player components.
	/// </summary>
	partial class TablePlayerLevel
	{
		public string 	owner 		{ get; set; }
		public string 	name 		{ get; set; }
		public int 		level 		{ get; set; }
	}

}