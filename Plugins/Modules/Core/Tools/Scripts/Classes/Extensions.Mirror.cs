using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;

namespace Mirror
{

	// =======================================================================================
	// Extensions
	// =======================================================================================
	public static partial class Extensions
	{
	
		// -----------------------------------------------------------------------------------
		// Id
		// Returns the connection id of a NetworkConnection as formatted string
		// used very often in logfiles
		// Extends: NetworkConnection
		// -----------------------------------------------------------------------------------
		public static string Id(this NetworkConnection conn)
		{
				return "ID" + conn.connectionId.ToString();
		}
	
		// -----------------------------------------------------------------------------------
	
	}

}

// =======================================================================================