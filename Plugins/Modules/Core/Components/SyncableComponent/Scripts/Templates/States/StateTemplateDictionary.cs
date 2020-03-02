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
	/// Basic, partial State Template Dictionary that exclusively holds State Templates.
	/// </summary>
	public partial class StateTemplateDictionary
	{
		
		public readonly ReadOnlyDictionary<int, StateTemplate> data;
		
		public StateTemplateDictionary(string folderName="")
		{
			List<StateTemplate> templates = Resources.LoadAll<StateTemplate>(folderName).ToList();
			
			if (templates.HasDuplicates())
				DebugManager.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
			else
				data = new ReadOnlyDictionary<int, StateTemplate>(templates.ToDictionary(x => x.hash, x => x));
		}

	}

}