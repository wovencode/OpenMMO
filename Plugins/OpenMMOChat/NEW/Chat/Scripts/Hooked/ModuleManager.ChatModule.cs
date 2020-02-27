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
		public static void Constructor_ChatModule()
		{
			
			Module module = new Module();
			
			module.name				= "ChatModule";
			module.define			= "oCHAT";
			module.version       	= "0.01";
			module.unity3dVersion 	= "2018.x-2019.x";
			module.nameSpace		= "OpenMMO.Chat";
        	module.package         	= "OpenMMO";
        	module.author        	= "Fhiz + Davil";
        	module.dependencies  	= "";
       		module.comments      	= "none";
        	
        	AddModule(module);
        	
		}

	}

}

#endif

// =======================================================================================