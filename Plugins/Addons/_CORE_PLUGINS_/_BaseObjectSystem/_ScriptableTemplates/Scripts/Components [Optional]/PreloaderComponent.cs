//by Fhiz
using OpenMMO;
using OpenMMO.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace OpenMMO
{
	
	/// <summary>
	/// This component can be attached to any MonoBehaviour and will preload all templates (based on ScriptableTemplate) of the same type. 
	/// </summary>
	/// <remarks>
	/// You can enter any amount of templates, use one representative template each, it will iterate through all templates of the same type and load them into memory.
	/// </remarks>
	public class PreloaderComponent : MonoBehaviour
	{
		
		[Header("Add one representative template of each type in Resources")]
		public ScriptableTemplate[] preloadTemplates;
		public bool preloadOnAwake;
	
		/// <summary>
		/// Triggers the preloading on awake.
		/// </summary>
		void Awake()
		{
			if (preloadOnAwake)
				PreloadTemplates();
		}
	
		/// <summary>
		/// This function covers the actual preloading process.
		/// </summary>
		public void PreloadTemplates()
		{
		
			foreach (ScriptableTemplate template in preloadTemplates)
			{
				if (template == null) continue;
				
				/// <summary>
				/// This is a small workaround that calls "BuildCache" on the template itself, as we cannot override static methods.
				/// </summary>
				Type t = template.GetType();
				MethodInfo m = t.GetMethod("BuildCache");
				m.Invoke(this, new object[]{true});
			}	
		
		}

	}

}