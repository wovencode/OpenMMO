
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
	// BarTemplateDictionary
	// ===================================================================================
	public partial class BarTemplateDictionary
	{
		
		public readonly ReadOnlyDictionary<int, BarTemplate> data;
		
		// -------------------------------------------------------------------------------
		public BarTemplateDictionary(string folderName="")
		{
			List<BarTemplate> templates = Resources.LoadAll<BarTemplate>(folderName).ToList();
			
			if (templates.HasDuplicates())
				DebugManager.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
			else
				data = new ReadOnlyDictionary<int, BarTemplate>(templates.ToDictionary(x => x.hash, x => x));
		}

		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================
