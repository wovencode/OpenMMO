//by Fhiz
using OpenMMO;
using OpenMMO.Debugging;
using System;
using System.IO;
using System.Collections.Generic;

namespace OpenMMO.Debugging
{
	
	/// <summary>
	/// Static DebugManager can be accessed globally w/o adding it to any other class.
	/// </summary>
	/// <remarks>
	/// Contrary to the DebugHelper, the DebugManager cannot generate DebugProfiles.
	/// </remarks>
	public partial class DebugManager
	{
	
		public static bool debugMode;
		protected static StreamWriter streamWriter;
		protected static string fileName;
		
		/// <summary>
		/// Adds a standard log entry and (optionally) the stack trace as well.
		/// </summary>
		public static void Log(string message, bool trace=true)
		{
			WriteToLog(message, LogType.Log, trace);
		}
		
		/// <summary>
		/// Adds a warning log entry and (optionally) the stack trace as well.
		/// </summary>
		public static void LogWarning(string message, bool trace=true)
		{
			WriteToLog(message, LogType.Log, trace);
		}
		
		/// <summary>
		/// Adds a error log entry and (optionally) the stack trace as well.
		/// </summary>
		public static void LogError(string message, bool trace=true)
		{
			WriteToLog(message, LogType.Log, trace);
		}
		
		/// <summary>
		/// Adds a formatted standard log entry of separated strings (w/o stack trace).
		/// </summary>
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
		
		/// <summary>
		/// Formats and writes the actual log entry to the log and the text log (when in debug mode only).
		/// </summary>
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
		
		/// <summary>
		/// Writes the actual log entry to the text log, using a subfolder and appending a suffix (to differentiate logs from various server processes).
		/// </summary>
		protected static void WriteToLogFile(string message)
		{

			if (!ProjectConfigTemplate.singleton.logMode || String.IsNullOrWhiteSpace(message))
				return;
			
			if (!Directory.Exists(Tools.GetPath(ProjectConfigTemplate.singleton.logFolder)))
				Directory.CreateDirectory(Tools.GetPath(ProjectConfigTemplate.singleton.logFolder));
						
			if (String.IsNullOrWhiteSpace(fileName))
				fileName = ProjectConfigTemplate.singleton.logFolder + "/" + ProjectConfigTemplate.singleton.logFilename + "_" + Tools.GetRandomAlphaString() + ".txt";
			
			using (streamWriter = new StreamWriter(Tools.GetPath(fileName), true))
			{
				streamWriter.WriteLine(message);
				streamWriter.Close();
			}
			
		}
		
	}

}