
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.DebugManager;

namespace OpenMMO
{
	
	// ===================================================================================
	// ProjectConfigTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Project Configuration", menuName = "OpenMMO - Configuration/New Project Configuration", order = 999)]
	public partial class ProjectConfigTemplate : ScriptableObject
	{

		[Header("Project Configuration")]
		[SerializeField] internal NetworkType networkType;
		public bool globalDebugMode;
		
		[Header("Security")]
    	public string securitySalt = "16_or_more_bytes";
		
		[Header("Servers")]
		public ServerInfoTemplate[] serverList;
		
		static ProjectConfigTemplate _instance;
		
		protected const string isServer = "_SERVER";
    	protected const string isClient = "_CLIENT";
		
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
				EditorTools.RemoveScriptingDefine(isClient);
				EditorTools.AddScriptingDefine(isServer);
				debug.Log("[ProjectConfig] Switched to SERVER mode.");
			}
			else if (networkType == NetworkType.HostAndPlay)
			{
				EditorTools.AddScriptingDefine(isServer);
				EditorTools.AddScriptingDefine(isClient);
				debug.Log("[ProjectConfig] Switched to HOST & PLAY mode.");
			}
			/*else if (networkType == NetworkType.Development)
			{
				EditorTools.AddScriptingDefine(isServer);
				EditorTools.AddScriptingDefine(isClient);
				debug.Log("[ProjectConfig] Switched to DEVELOPMENT mode.");
			}*/
			else
			{
				EditorTools.AddScriptingDefine(isClient);
				EditorTools.RemoveScriptingDefine(isServer);
				debug.Log("[ProjectConfig] Switched to CLIENT mode.");
			}
#endif
		}
	
		// -------------------------------------------------------------------------------
	}

}

// =======================================================================================
