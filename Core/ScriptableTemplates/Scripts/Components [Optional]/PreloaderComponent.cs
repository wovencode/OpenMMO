
using Wovencode;
using Wovencode.DebugManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace Wovencode
{
	
	// ===================================================================================
	// PreloaderComponent
	// ===================================================================================
	public class PreloaderComponent : MonoBehaviour
	{
		
		[Header("Add one representative template of each type in Resources")]
		public ScriptableTemplate[] preloadTemplates;
		public bool preloadOnAwake;
	
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		void Awake()
		{
			if (preloadOnAwake)
				PreloadTemplates();
		}
	
		// -------------------------------------------------------------------------------
		// PreloadTemplates
		// -------------------------------------------------------------------------------
		public void PreloadTemplates()
		{
		
			foreach (ScriptableTemplate template in preloadTemplates)
			{
				if (template == null) continue;
				
				// workaround that calls "BuildCache"
				// as we cannot override static methods
				Type t = template.GetType();
				MethodInfo m = t.GetMethod("BuildCache");
				m.Invoke(this, new object[]{true});
			}	
		
		}
	
		// -------------------------------------------------------------------------------
	
	}

}