//by Fhiz
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using OpenMMO;
using OpenMMO.Debugging;

namespace OpenMMO
{

	/// <summary>
	/// A custom read only Dictionary exclusively used for this type of template. Speeds up loading and lookup of entries.
	/// <remarks>Every template type (ScriptableObject based on ScriptableTemplate or BaseTemplate) requires a dictionary of its own in order to work at full potential.</remarks>
	/// </summary>
	public partial class RarityTemplateDictionary
	{
		
		public readonly ReadOnlyDictionary<int, RarityTemplate> data;
		
		/// <summary>
		/// Constructor. Loads all templates of the matching type from the stated folder (or from the root folder if no name supplied).
		/// </summary>
		public RarityTemplateDictionary(string folderName="")
		{
			List<RarityTemplate> templates = Resources.LoadAll<RarityTemplate>(folderName).ToList();
			
			if (templates.HasDuplicates())
				DebugManager.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
			else
				data = new ReadOnlyDictionary<int, RarityTemplate>(templates.ToDictionary(x => x.hash, x => x));
		}

	}

}