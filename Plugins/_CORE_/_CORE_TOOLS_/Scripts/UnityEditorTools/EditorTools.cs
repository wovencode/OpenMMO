//by Fhiz
#if UNITY_EDITOR

using OpenMMO;
using System;
using UnityEditor;
using UnityEngine;

namespace OpenMMO
{
	
	/// <summary>
	/// Static partial class EditorTools is only used inside the Unity Editor and not in builds.
	/// </summary>
	[InitializeOnLoad]
	public static partial class EditorTools
	{
		
		public static string[] definesArray;
		public static string definesString;
		public static BuildTargetGroup buildTargetGroup;
		
		/// <summary>
		/// Updates a string value in editor preferences or adds if if it does not exist.
		/// </summary>
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
		
		/// <summary>
		/// Updates a integer value in editor preferences or adds if if it does not exist.
		/// </summary>
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
		
		/// <summary>
		/// Removes the passed define (string) from scripting defines of the current target platform, without touching other defines.
		/// </summary>
		/// <remarks>
		/// Results in recompile if the symbol is added.
		/// </remarks>
		public static void AddScriptingDefine(string define)
		{
			if (HasScriptingDefine(define))
				return;
				
			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (definesString + ";" + define));
		}
		
		/// <summary>
		/// Adds the passed define (string) to scripting defines of the current target platform, without adding duplicates.
		/// </summary>
		/// <remarks>
		/// Results in recompile if the symbol is added.
		/// </remarks>
		public static void RemoveScriptingDefine(string define)
		{
			if (!HasScriptingDefine(define))
				return;

			definesArray = Tools.RemoveFromArray(definesArray, define);

			definesString = string.Join(";", definesArray);

			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (definesString));
		}
		
		/// <summary>
		/// Checks if the provided string is present in scripting defines
		/// </summary>
		public static bool HasScriptingDefine(string define)
		{
			buildTargetGroup 	= EditorUserBuildSettings.selectedBuildTargetGroup;
			definesString 		= PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			definesArray 		= definesString.Split(';');

			if (Tools.ArrayContains(definesArray, define))
				return true;
			
			return false;
		}
	
	}

}

#endif