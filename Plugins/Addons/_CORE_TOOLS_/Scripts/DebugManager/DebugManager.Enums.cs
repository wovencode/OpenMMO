//by Fhiz
using System;
using System.Text;
using UnityEngine;
using OpenMMO;
using OpenMMO.Database;

namespace OpenMMO.Debugging {

	/// <summary>
	/// Used to turn the debug mode on or off on components, mono and network behaviours.
	/// <para>On = Debug Mode on this object is always on.</para>
	/// <para>Off = Debug Mode on this object is always off.</para>
	/// <para>UseGlobal = Uses the global setting from ProjectConfiguration to determine if the Debug Mode is on or off.</para>
	/// </summary>
	public enum DebugMode {On, Off, UseGlobal}

	/// <summary>
	/// Similar to Unity's built-in log modes: Log, Warning and Error.
	/// <para>Log = Standard log entry</para>
	/// <para>Warning = Warning log entry (yellow)</para>
	/// <para>Error = Error log entry (red)</para>
	/// </summary>
	public enum LogType {Log, Warning, Error}
	
}