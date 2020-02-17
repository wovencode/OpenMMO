#if UNITY_EDITOR

using OpenMMO;
using OpenMMO.Modules;
using OpenMMO.DebugManager;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenMMO.Modules
{
	
	// ===================================================================================
	// ModuleManager
	// ===================================================================================
	[InitializeOnLoad]
	public partial class ModuleManager
	{
		
		public static List<Module> modules = new List<Module>();

        // -------------------------------------------------------------------------------
        // Constructor Hooks
        // -------------------------------------------------------------------------------
        static void Constructor()
        {
            DevExtUtils.InvokeStaticDevExtMethods(typeof(ModuleManager), nameof(Constructor));
        }

		// -------------------------------------------------------------------------------
		// ModuleManager (Constructor)
		// -------------------------------------------------------------------------------
		static ModuleManager()
		{
			
			modules.Clear();

            // -- gather all modules
            Constructor();
			
			// -- add/update all modules	
			UpdateDefines(modules);
			
		}
		
		// -------------------------------------------------------------------------------
		// AddModule
		// -------------------------------------------------------------------------------
		public static void AddModule(Module module)
		{
			modules.Add(module);
		}
		
		// -------------------------------------------------------------------------------
		// UpdateDefines
		// -------------------------------------------------------------------------------
		public static void UpdateDefines(List<Module> _modules)
		{

			foreach (Module module in _modules)
				EditorTools.AddScriptingDefine(module.define);
			
			ValidateModules(_modules);
			
		}
		
		// -------------------------------------------------------------------------------
		// ValidateModules
		// -------------------------------------------------------------------------------
		public static void ValidateModules(List<Module> _modules)
		{

			debug.Log("[ModuleManager] Validating installed Modules...");
			
			int i = 0;
			
			foreach (Module module in _modules)
			{
				if (!String.IsNullOrWhiteSpace(module.dependencies))
				{	
					string[] requiredDefines = module.dependencies.Split(',');
					foreach (string define in requiredDefines)
					{
						if (!EditorTools.HasScriptingDefine(define))
							debug.Log("* Module '"+module.name+"' requires define '"+define+"' - activate module or add define manually.");
					}
				}
			}
			
			debug.Log("[ModuleManager] Validation complete, encountered "+i.ToString()+" error(s).");

		}
		
		// -------------------------------------------------------------------------------
	
	}

}

#endif

// =======================================================================================