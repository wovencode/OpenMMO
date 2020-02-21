
using OpenMMO;
using OpenMMO.DebugManager;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace OpenMMO.DebugManager
{
	
	// ===================================================================================
	// DebugHelper
	// ===================================================================================
	[System.Serializable]
	public partial class DebugHelper
	{
		
		public bool debugMode;
		
		protected List<DebugProfile> debugProfiles = new List<DebugProfile>();
		protected StreamWriter streamWriter;
		
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
			WriteToLog(message, LogType.Log, trace);
		}
		
		// -------------------------------------------------------------------------------
		// LogWarning
		// @debugMode
		// -------------------------------------------------------------------------------
		public void LogWarning(string message, bool trace=true)
		{
			WriteToLog(message, LogType.Warning, trace);
		}
		
		// -------------------------------------------------------------------------------
		// LogError
		// @debugMode
		// -------------------------------------------------------------------------------
		public void LogError(string message, bool trace=true)
		{
			WriteToLog(message, LogType.Error, trace);
		}
		
		// -------------------------------------------------------------------------------
		// LogFormat
		// @debugMode
		// -------------------------------------------------------------------------------
		public void LogFormat(params string[] list)
		{
			
			if (list.Length == 0)
				return;
			
			string message = "["+list[0]+"] ";
			
			for (int i = 1; i < list.Length; i++)
				message += list[i] + " ";
		
			WriteToLog(message, LogType.Log, false);
			
		}
		
		
		// =================== PROTECTED METHODS - DEBUG LOG =============================
		
		// -------------------------------------------------------------------------------
		// WriteToLog
		// @debugMode
		// -------------------------------------------------------------------------------
		protected void WriteToLog(string message, LogType logType, bool trace=true)
		{
		
			if (!debugMode || String.IsNullOrWhiteSpace(message))
				return;
			
			string traceString 	= "";
			string logString 	= "";
			
			if (trace)
				traceString = new System.Diagnostics.StackTrace().ToString();
			
			logString = "<b>" + message + "</b>";
			
			if (trace)
				logString += "\n" + traceString;
			
			if (logType == LogType.Log)
				UnityEngine.Debug.Log(logString);
			else if (logType == LogType.Warning)
				UnityEngine.Debug.LogWarning(logString);
			else if (logType == LogType.Error)
				UnityEngine.Debug.LogError(logString);

			WriteToLogFile(message);

		}
		
		// -------------------------------------------------------------------------------
		// WriteToLogFile
		// @debugMode
		// -------------------------------------------------------------------------------
		protected void WriteToLogFile(string message)
		{

			if (!ProjectConfigTemplate.singleton.logMode || String.IsNullOrWhiteSpace(message))
				return;
			
			streamWriter = new StreamWriter(Tools.GetPath(ProjectConfigTemplate.singleton.logFilename), true);

			streamWriter.WriteLine(message);
			
			streamWriter.Close();
			
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
