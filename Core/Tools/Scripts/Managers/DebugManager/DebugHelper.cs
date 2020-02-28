//by Fhiz
using OpenMMO;
using OpenMMO.Debugging;
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
		/// 
		/// </summary>
		public void LogFormat(params string[] list)
		{
			DebugManager.LogFormat(list);	
		}
		
		/// <summary>
		/// 
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
		/// 
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
		/// 
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
		/// 
		/// </summary>
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
