
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
	// FooTemplateDictionary
	// ===================================================================================
	public partial class FooTemplateDictionary
	{
		
		public readonly ReadOnlyDictionary<int, FooTemplate> data;
		
		// -------------------------------------------------------------------------------
		public FooTemplateDictionary(string folderName="")
		{
			List<FooTemplate> templates = Resources.LoadAll<FooTemplate>(folderName).ToList();
			
			if (templates.HasDuplicates())
				DebugManager.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
			else
				data = new ReadOnlyDictionary<int, FooTemplate>(templates.ToDictionary(x => x.hash, x => x));
		}

		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================
