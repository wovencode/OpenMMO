//by Fhiz
using OpenMMO;
using OpenMMO.Debugging;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace OpenMMO.Debugging
{
	
	/// <summary>
	/// Non-static class DebugHelper can be added to any class, component, mono or network behaviour
	/// </summary>
	/// <remarks>
	/// The DebugHelper is able to generate DebugProfiles of the object it is part of. DebugProfiles are used to measure code execution time.
	/// </remarks>
	[System.Serializable]
	public partial class DebugHelper
	{
		
		/// <summary>
		/// Used to set the debug mode on a per object basis.
		/// </summary>
		[Tooltip("Toggle debug mode for logging (On = always on, Off = always off, Use Global = use global setting in ProjectConfigTemplate)")]
		public DebugMode _debugMode;
		
		protected List<DebugProfile> debugProfiles = new List<DebugProfile>();
		
		/// <summary>
		/// Returns a bool that indicates if the debug mode is on or off. 
		/// </summary>
		public bool debugMode
		{
			get {
				if (_debugMode == DebugMode.On)
					return true;
				else if (_debugMode == DebugMode.Off)
					return false;
				else
					return ProjectConfigTemplate.singleton.globalDebugMode;
			}
		}
		
		/// <summary>
		/// Adds a standard entry to the log. It depends on the debug settings if the entry is also added to the text log.
		/// </summary>
		public void Log(string message, bool trace=true)
		{
			DebugManager.WriteToLog(message, LogType.Log, trace);
		}
		
		/// <summary>
		/// Adds a warning entry to the log. It depends on the debug settings if the entry is also added to the text log.
		/// </summary>
		public void LogWarning(string message, bool trace=true)
		{
			DebugManager.WriteToLog(message, LogType.Warning, trace);
		}
		
		/// <summary>
		/// Adds a error entry to the log. It depends on the debug settings if the entry is also added to the text log.
		/// </summary>
		public void LogError(string message, bool trace=true)
		{
			DebugManager.WriteToLog(message, LogType.Error, trace);
		}
		
		/// <summary>
		/// Adds a formatted error to the log. This special log type expects a list of strings and formats them.
		/// <para>The first entry is always the name of the object itself. Its parsed in bold and brackets.</para>
		/// <para>All subsequent entries are simply parsed as strings with whitespace between them.</para>
		/// </summary>
		public void LogFormat(params string[] list)
		{
			DebugManager.LogFormat(list);	
		}
		
		/// <summary>
		/// Starts a new performance profile with the given name or restarts a profile of the same name.
		/// </summary>
		public void StartProfile(string name)
		{
			if (!debugMode)
				return;
				
			if (HasProfile(name))
				RestartProfile(name);
			else
				AddProfile(name);
		}
		
		/// <summary>
		/// Stops a existing performance profile of the given name.
		/// </summary>
		public void StopProfile(string name)
		{
			if (!debugMode)
				return;
				
			int index = GetProfileIndex(name);
			if (index != -1)
				debugProfiles[index].Stop();
		}
		
		/// <summary>
		/// Prints the last and total average results of a existing debug profile of the given name.
		/// </summary>
		public void PrintProfile(string name)
		{
			if (!debugMode)
				return;
				
			int index = GetProfileIndex(name);
			if (index != -1)
				Log(debugProfiles[index].Print);
		}
		
		/// <summary>
		/// Resets all debug profiles, deleting names, last results and all average results altogether.
		/// </summary>
		public void Reset()
		{
			if (!debugMode)
				return;
				
			foreach (DebugProfile profile in debugProfiles)
				profile.Reset();
		}
		
		/// <summary>
		/// Checks if a profile of the given name exists.
		/// </summary>
		protected bool HasProfile(string _name)
		{
			return debugProfiles.Any(x => x.name == _name);
		}
		
		/// <summary>
		/// Gets the index of a profile of the given name (better check for HasProfile first).
		/// </summary>
		protected int GetProfileIndex(string _name)
		{
			return debugProfiles.FindIndex(x => x.name == _name);
		}
		
		/// <summary>
		/// Adds a new profile of the given name (better check for HasProfile first).
		/// </summary>
		protected void AddProfile(string name)
		{
			debugProfiles.Add(new DebugProfile(name));
		}
		
		/// <summary>
		/// Restarts a profile of the given name if it exists.
		/// </summary>
		protected void RestartProfile(string name)
		{
			int index = GetProfileIndex(name);
			if (index != -1)
				debugProfiles[index].Restart();
		}
		
	}

}
