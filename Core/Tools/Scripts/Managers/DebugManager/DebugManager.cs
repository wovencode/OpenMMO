
using OpenMMO;
using OpenMMO.Debugging;
using OpenMMO.Zones;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenMMO.Debugging
{
	
	// ===================================================================================
	// DebugManager
	// ===================================================================================
	public partial class DebugManager
	{
	
		public static bool debugMode;
		protected static StreamWriter streamWriter;
		protected static string fileName;
		
		// -------------------------------------------------------------------------------
		// Log
		// @debugMode
		// -------------------------------------------------------------------------------
		public static void Log(string message, bool trace=true)
		{
			WriteToLog(message, LogType.Log, trace);
		}
		
		// -------------------------------------------------------------------------------
		// LogWarning
		// @debugMode
		// -------------------------------------------------------------------------------
		public static void LogWarning(string message, bool trace=true)
		{
			WriteToLog(message, LogType.Log, trace);
		}
		
		// -------------------------------------------------------------------------------
		// LogError
		// @debugMode
		// -------------------------------------------------------------------------------
		public static void LogError(string message, bool trace=true)
		{
			WriteToLog(message, LogType.Log, trace);
		}
		
		// -------------------------------------------------------------------------------
		// LogFormat
		// @debugMode
		// -------------------------------------------------------------------------------
		public static void LogFormat(params string[] list)
		{
			
			if (list.Length == 0)
				return;
			
			if (!debugMode)
				debugMode = ProjectConfigTemplate.singleton.globalDebugMode;
			
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
		public static void WriteToLog(string message, LogType logType, bool trace=true)
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
		protected static void WriteToLogFile(string message)
		{

			if (!ProjectConfigTemplate.singleton.logMode || String.IsNullOrWhiteSpace(message))
				return;
			
			if (String.IsNullOrWhiteSpace(fileName))
				fileName = ProjectConfigTemplate.singleton.logFilename + Tools.GetRandomAlphaString() + ".txt";
			
			using (streamWriter = new StreamWriter(Tools.GetPath(fileName), true))
			{
				streamWriter.WriteLine(message);
				streamWriter.Close();
			}
			
		}
		
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================