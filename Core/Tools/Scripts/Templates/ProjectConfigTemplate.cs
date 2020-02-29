//by Fhiz
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Debugging;

namespace OpenMMO
{
	
	/// <summary>
	/// Contains configuration for the project itself (like debug mode, location of logs etc.)
	/// </summary>
	[CreateAssetMenu(fileName = "New Project Configuration", menuName = "OpenMMO - Configuration/New Project Configuration", order = 999)]
	public partial class ProjectConfigTemplate : ScriptableObject
	{

		[Header("Project Configuration")]
#pragma warning disable CS0649
		[Tooltip("Toggle type of build: Server, Client or Host+Play")]
		[SerializeField] internal NetworkType networkType;
#pragma warning restore CS0649
		[Tooltip("Toggle debug mode for logging (globally, affects all components with a DebugHelper as well)")]
		public bool globalDebugMode;
		
		[Header("Security")]
    	public string securitySalt = "16_or_more_bytes";
		
		[Header("Servers")]
		public ServerInfoTemplate[] serverList;
		
		[Header("Logging")]
		public bool logMode;
		[Tooltip("Filename for the text log file (zone and chat servers append a random suffix to this)")]
		public string logFilename = "OpenMMOLog";
		[Tooltip("Foldername for all text log files (normally found inside project folder or project package)")]
		public string logFolder = "OpenMMOLogs";
		
		static ProjectConfigTemplate _instance;
		
		/// <summary>
		/// Creates a singleton on this class to be accesible from code anywhere. Singleton is OK in this situation because this template (= Scriptable Object) exists only once.
		/// </summary>
		public static ProjectConfigTemplate singleton
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<ProjectConfigTemplate>().FirstOrDefault();
				return _instance;
			}
		}
		
		/// <summary>
		/// Public getter for NetworkType, so that the original property cannot be modified from external code.
		/// </summary>
		public NetworkType GetNetworkType
		{
			get
			{
				return networkType;
			}
		}
		
		/// <summary>
		/// After changing, updates scripting defines and recompiles the code.
		/// </summary>
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

	}

}