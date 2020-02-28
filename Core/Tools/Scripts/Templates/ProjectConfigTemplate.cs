
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Debugging;

namespace OpenMMO
{
	
	// ===================================================================================
	// ProjectConfigTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Project Configuration", menuName = "OpenMMO - Configuration/New Project Configuration", order = 999)]
	public partial class ProjectConfigTemplate : ScriptableObject
	{

		[Header("Project Configuration")]
#pragma warning disable CS0649
		[SerializeField] internal NetworkType networkType;
#pragma warning restore CS0649
		public bool globalDebugMode;
		
		[Header("Security")]
    	public string securitySalt = "16_or_more_bytes";
		
		[Header("Servers")]
		public ServerInfoTemplate[] serverList;
		
		[Header("Logging")]
		public bool logMode;
		public string logFilename = "OpenMMOLog";
		public string logFolder = "OpenMMOLogs";
		
		static ProjectConfigTemplate _instance;
		
		// -------------------------------------------------------------------------------
		// singleton
		// -------------------------------------------------------------------------------
		public static ProjectConfigTemplate singleton
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<ProjectConfigTemplate>().FirstOrDefault();
				return _instance;
			}
		}
		
		// -------------------------------------------------------------------------------
		// GetNetworkType
		// -------------------------------------------------------------------------------
		public NetworkType GetNetworkType
		{
			get
			{
				return networkType;
			}
		}
		
		// -----------------------------------------------------------------------------------
		// OnValidate
		// -----------------------------------------------------------------------------------
		public void OnValidate()
		{
#if UNITY_EDITOR
			if (networkType == NetworkType.Server)
			{
				EditorTools.RemoveScriptingDefine(Constants.BuildModeClient);
				EditorTools.AddScriptingDefine(Constants.BuildModeServer);
				DebugManager.Log("<b><color=yellow>[ProjectConfig] Switched to SERVER mode.</color></b>");
			}
			else if (networkType == NetworkType.HostAndPlay)
			{
				EditorTools.AddScriptingDefine(Constants.BuildModeServer);
				EditorTools.AddScriptingDefine(Constants.BuildModeClient);
				DebugManager.Log("<b><color=green>[ProjectConfig] Switched to HOST & PLAY mode.</color></b>");
			}
			else
			{
				EditorTools.AddScriptingDefine(Constants.BuildModeClient);
				EditorTools.RemoveScriptingDefine(Constants.BuildModeServer);
				DebugManager.Log("<b><color=blue>[ProjectConfig] Switched to CLIENT mode.</color></b>");
			}
#endif
		}
	
		// -------------------------------------------------------------------------------
	}

}

// =======================================================================================
