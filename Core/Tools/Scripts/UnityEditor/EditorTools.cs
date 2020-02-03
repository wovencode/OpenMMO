// =======================================================================================
// EditorTools
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

#if UNITY_EDITOR

using OpenMMO;
using System;
using UnityEditor;
using UnityEngine;

namespace OpenMMO
{
	
	// ===================================================================================
	// EditorTools
	// ===================================================================================
	[InitializeOnLoad]
	public static partial class EditorTools
	{
		
		public static string[] definesArray;
		public static string definesString;
		public static BuildTargetGroup buildTargetGroup;
		
		// ============================== EDITOR PREFERENCES =============================
		
		// -------------------------------------------------------------------------------
		// EditorPrefsUpdateString
		// -------------------------------------------------------------------------------
		public static string EditorPrefsUpdateString(string keyName, string value)
		{	
			if (EditorPrefs.HasKey(keyName))
			{
				
				if (!String.IsNullOrWhiteSpace(value))
				{
					EditorPrefs.SetString(keyName, value);
					return value;
				}
				else if (String.IsNullOrWhiteSpace(value))
				{
					return EditorPrefs.GetString(keyName);
				}		
			}
			else
			{
				EditorPrefs.SetString(keyName, value);
			}
			return value;		
		}
		
		// -------------------------------------------------------------------------------
		// EditorPrefsUpdateInt
		// -------------------------------------------------------------------------------
		public static int EditorPrefsUpdateInt(string keyName, int value)
		{	
			if (EditorPrefs.HasKey(keyName))
			{
				
				if (value != 0)
				{
					EditorPrefs.SetInt(keyName, value);
					return value;
				}
				else if (value == 0)
				{
					return EditorPrefs.GetInt(keyName);
				}		
			}
			else
			{
				EditorPrefs.SetInt(keyName, value);
			}
			return value;		
		}
		
		// ============================== SCRIPTING DEFINES ==============================
		
		// -------------------------------------------------------------------------------
		// AddScriptingDefine
		// Removes the passed define (string) from scripting defines of the current target
		// platform, without touching other defines.
		// -------------------------------------------------------------------------------
		public static void AddScriptingDefine(string define)
		{
			if (HasScriptingDefine(define))
				return;
				
			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (definesString + ";" + define));
		}

		// -------------------------------------------------------------------------------
		// RemoveScriptingDefine
		// Adds the passed define (string) to scripting defines of the current target 
		// platform, without adding duplicates.
		// -------------------------------------------------------------------------------
		public static void RemoveScriptingDefine(string define)
		{
			if (!HasScriptingDefine(define))
				return;

			definesArray = Tools.RemoveFromArray(definesArray, define);

			definesString = string.Join(";", definesArray);

			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (definesString));
		}
		
		// -------------------------------------------------------------------------------
		// HasScriptingDefine
		// Checks if the provided string is present in scripting defines
		// -------------------------------------------------------------------------------
		public static bool HasScriptingDefine(string define)
		{
			buildTargetGroup 	= EditorUserBuildSettings.selectedBuildTargetGroup;
			definesString 		= PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			definesArray 		= definesString.Split(';');

			if (Tools.ArrayContains(definesArray, define))
				return true;
			
			return false;
		}
	
		// -------------------------------------------------------------------------------
	
	}

}

#endif

// =======================================================================================