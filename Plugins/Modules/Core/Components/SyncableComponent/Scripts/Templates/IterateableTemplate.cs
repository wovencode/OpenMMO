
using System;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// IterateableTemplate
	// ===================================================================================
	public abstract partial class IterateableTemplate : BaseTemplate
	{
		
		[Header("Settings")]
		[Tooltip("Allows to adjust the impact of this object (mostly for modifiers)")]
		public int level;
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================