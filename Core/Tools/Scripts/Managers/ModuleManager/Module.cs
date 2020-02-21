
using OpenMMO;
using OpenMMO.Debugging;
using OpenMMO.Modules;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenMMO.Modules
{
	
	// ===================================================================================
	// Module
	// ===================================================================================
	[System.Serializable]
	public partial class Module
	{
		
		[HideInInspector] public string name;
		[ReadOnly] public string define;
		[ReadOnly] public string version;
		[ReadOnly] public string unity3dVersion;
		[ReadOnly] public string nameSpace;
		[ReadOnly] public string package;
		[ReadOnly] public string author;
		[ReadOnly] public string dependencies;
		[ReadOnly] [TextArea(1, 30)]public string comments;
		
		// -------------------------------------------------------------------------------
		// Copy
		// -------------------------------------------------------------------------------
		public void Copy(Module module)
		{
			name			= module.name;
			define			= module.define;
			version       	= module.version;
			unity3dVersion 	= module.unity3dVersion;
			nameSpace		= module.nameSpace;
        	package         = module.package;
        	author        	= module.author;
        	dependencies  	= module.dependencies;
       		comments      	= module.comments;
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================