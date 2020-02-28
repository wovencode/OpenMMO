// by Fhiz
using System;
using System.Text;
using UnityEngine;
using OpenMMO;
using OpenMMO.Database;

namespace OpenMMO.Database {

	/// <summary>
	/// Simple enum to allow switching between the built-in database types.
	/// </summary>
	public enum DatabaseType {SQLite, mySQL}
	
	/// <summary>
	/// Simple enum to denote an action to be "done" or "undone".
	/// </summary>
	public enum DatabaseAction {Do, Undo}
	
}