
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using OpenMMO;
using OpenMMO.Debugging;

namespace OpenMMO
{

	// ===================================================================================
	// ServerInfoTemplateDictionary
	// ===================================================================================
	public partial class ServerInfoTemplateDictionary
	{
		
		public readonly ReadOnlyDictionary<int, ServerInfoTemplate> data;
		
		// -------------------------------------------------------------------------------
		public ServerInfoTemplateDictionary(string folderName="")
		{
			List<ServerInfoTemplate> templates = Resources.LoadAll<ServerInfoTemplate>(folderName).ToList();
			
			if (templates.HasDuplicates())
				DebugManager.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
			else
				data = new ReadOnlyDictionary<int, ServerInfoTemplate>(templates.ToDictionary(x => x.hash, x => x));
		}

		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================
