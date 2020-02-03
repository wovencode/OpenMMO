
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.DebugManager;
using OpenMMO.Modules;

namespace OpenMMO
{
	
	// ===================================================================================
	// ModulesConfigTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Modules Configuration", menuName = "OpenMMO - Configuration/New Modules Configuration", order = 999)]
	public partial class ModulesConfigTemplate : ScriptableObject
	{

		[Header("Modules Configuration")]
#if UNITY_EDITOR
    	[SerializeField]
    	[Header("(Change list size to force refresh)")]
    	public List<Module> modules = new List<Module>();
#endif
		
		static ModulesConfigTemplate _instance;
		
		// -----------------------------------------------------------------------------------
		// OnValidate
		// -----------------------------------------------------------------------------------
		void OnValidate()
		{

#if UNITY_EDITOR
			
			if (ModuleManager.modules.Count() > 0 && modules.Count() != ModuleManager.modules.Count())
			{

				modules.Clear();

				for (int i = 0; i < ModuleManager.modules.Count(); ++i)
				{
					Module module = new Module();
					module.Copy(ModuleManager.modules[i]);
					modules.Add(module);
					
				}
				
				ModuleManager.UpdateDefines(modules);
				
			}
			
#endif

		}

		// -------------------------------------------------------------------------------
		// singleton
		// -------------------------------------------------------------------------------
		public static ModulesConfigTemplate singleton
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<ModulesConfigTemplate>().FirstOrDefault();
				return _instance;
			}
		}

		// -------------------------------------------------------------------------------
	}

}

// =======================================================================================
