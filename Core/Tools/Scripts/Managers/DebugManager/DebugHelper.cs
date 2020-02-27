
using OpenMMO;
using OpenMMO.Debugging;
using OpenMMO.Portals;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace OpenMMO.Debugging
{
	
	// ===================================================================================
	// DebugHelper
	// ===================================================================================
	[System.Serializable]
	public partial class DebugHelper
	{
		
		public bool debugMode;
		
		protected List<DebugProfile> debugProfiles = new List<DebugProfile>();
		
		// -------------------------------------------------------------------------------
		// Init (Constructor)
		// -------------------------------------------------------------------------------
		public void Init()
		{
			if (!debugMode)
				debugMode = ProjectConfigTemplate.singleton.globalDebugMode;
		}
		
		// ===================== PUBLIC METHODS - DEBUG LOG ==============================
		
		// -------------------------------------------------------------------------------
		// Log
		// @debugMode
		// -------------------------------------------------------------------------------
		public void Log(string message, bool trace=true)
		{
			DebugManager.WriteToLog(message, LogType.Log, trace);
		}
		
		// -------------------------------------------------------------------------------
		// LogWarning
		// @debugMode
		// -------------------------------------------------------------------------------
		public void LogWarning(string message, bool trace=true)
		{
			DebugManager.WriteToLog(message, LogType.Warning, trace);
		}
		
		// -------------------------------------------------------------------------------
		// LogError
		// @debugMode
		// -------------------------------------------------------------------------------
		public void LogError(string message, bool trace=true)
		{
			DebugManager.WriteToLog(message, LogType.Error, trace);
		}
		
		// -------------------------------------------------------------------------------
		// LogFormat
		// @debugMode
		// -------------------------------------------------------------------------------
		public void LogFormat(params string[] list)
		{
			DebugManager.LogFormat(list);	
		}
		
		// ===================== PUBLIC METHODS - PROFILING ==============================
		
		// -------------------------------------------------------------------------------
		// StartProfile
		// @debugMode
		// -------------------------------------------------------------------------------
		public void StartProfile(string name)
		{
			if (!debugMode)
				return;
				
			if (HasProfile(name))
				RestartProfile(name);
			else
				AddProfile(name);
		}
		
		// -------------------------------------------------------------------------------
		// StopProfile
		// @debugMode
		// -------------------------------------------------------------------------------
		public void StopProfile(string name)
		{
			if (!debugMode)
				return;
				
			int index = GetProfileIndex(name);
			if (index != -1)
				debugProfiles[index].Stop();
		}
		
		// -------------------------------------------------------------------------------
		// PrintProfile
		// @debugMode
		// -------------------------------------------------------------------------------
		public void PrintProfile(string name)
		{
			if (!debugMode)
				return;
				
			int index = GetProfileIndex(name);
			if (index != -1)
				Log(debugProfiles[index].Print);
		}
		
		// -------------------------------------------------------------------------------
		// Reset
		// @debugMode
		// -------------------------------------------------------------------------------
		public void Reset()
		{
			if (!debugMode)
				return;
				
			foreach (DebugProfile profile in debugProfiles)
				profile.Reset();
		}
		
		// ==================== PROTECTED METHODS - PROFILING ============================
		
		// -------------------------------------------------------------------------------
		// HasProfile
		// @debugMode
		// -------------------------------------------------------------------------------
		protected bool HasProfile(string _name)
		{
			return debugProfiles.Any(x => x.name == _name);
		}
		
		// -------------------------------------------------------------------------------
		// GetProfileIndex
		// @debugMode
		// -------------------------------------------------------------------------------
		protected int GetProfileIndex(string _name)
		{
			return debugProfiles.FindIndex(x => x.name == _name);
		}
		
		// -------------------------------------------------------------------------------
		// AddProfile
		// @debugMode
		// -------------------------------------------------------------------------------
		protected void AddProfile(string name)
		{
			debugProfiles.Add(new DebugProfile(name));
		}
		
		// -------------------------------------------------------------------------------
		// RestartProfile
		// @debugMode
		// -------------------------------------------------------------------------------
		protected void RestartProfile(string name)
		{
			int index = GetProfileIndex(name);
			if (index != -1)
				debugProfiles[index].Restart();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================
