/*
#if UNITY_EDITOR

using OpenMMO;
using OpenMMO.Modules;
using UnityEditor;
using UnityEngine;

namespace OpenMMO.Modules
{

	// ===================================================================================
	// ModuleManager
	// ===================================================================================
	public partial class ModuleManager
	{
	
		// -------------------------------------------------------------------------------
		// Constructor
		// -------------------------------------------------------------------------------
		[DevExtMethods("Constructor")]
		public static void Constructor_DatabaseManager()
		{
			
			Module module = new Module();
			
			module.name				= "ExampleModule";
			module.define			= "_Test";
			module.version       	= "PreAlpha1";
			module.unity3dVersion 	= "2018.x-2019.x";
			module.nameSpace		= "OpenMMO";
        	module.package         	= "OpenMMO";
        	module.author        	= "Fhiz";
        	module.dependencies  	= "none";
       		module.comments      	= "none";
        	
        	AddModule(module);
        	
		}

	}

}

#endif

// =======================================================================================
*/