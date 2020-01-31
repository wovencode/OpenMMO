
using Wovencode;
using Wovencode.DebugManager;
using UnityEngine;
using System;
using System.Collections.Generic;


namespace Wovencode.DebugManager
{
	
	// ===================================================================================
	// Debug
	// ===================================================================================
	public partial class debug
	{
		
		// ======================= PUBLIC METHODS - DEBUG ================================
		
		// -------------------------------------------------------------------------------
		// Log
		// @debugMode
		// -------------------------------------------------------------------------------
		public static void Log(string message)
		{
			if (ProjectConfigTemplate.singleton && ProjectConfigTemplate.singleton.globalDebugMode)
				UnityEngine.Debug.Log(message);
		}
		
		// -------------------------------------------------------------------------------
		// LogWarning
		// @debugMode
		// -------------------------------------------------------------------------------
		public static void LogWarning(string message)
		{
			if (ProjectConfigTemplate.singleton && ProjectConfigTemplate.singleton.globalDebugMode)
				UnityEngine.Debug.LogWarning(message);
		}
		
		// -------------------------------------------------------------------------------
		// LogError
		// @debugMode
		// -------------------------------------------------------------------------------
		public static void LogError(string message)
		{
			if (ProjectConfigTemplate.singleton && ProjectConfigTemplate.singleton.globalDebugMode)
				UnityEngine.Debug.LogError(message);
		}
		
		// -------------------------------------------------------------------------------
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================