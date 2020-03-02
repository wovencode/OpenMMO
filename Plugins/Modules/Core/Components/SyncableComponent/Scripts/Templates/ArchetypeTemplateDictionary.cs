
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
	// ArchetypeTemplateDictionary
	// ===================================================================================
	public partial class ArchetypeTemplateDictionary
	{
		
		public readonly ReadOnlyDictionary<int, ArchetypeTemplate> data;
		
		// -------------------------------------------------------------------------------
		public ArchetypeTemplateDictionary(string folderName="")
		{
			List<ArchetypeTemplate> templates = Resources.LoadAll<ArchetypeTemplate>(folderName).ToList();
			
			if (templates.HasDuplicates())
				DebugManager.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
			else
				data = new ReadOnlyDictionary<int, ArchetypeTemplate>(templates.ToDictionary(x => x.hash, x => x));
		}

		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================
