//by Fhiz
using System;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Abstract partial Iterateable Template used as a base for all kinds of templates that can be iterated in order to provide stat boni of any kind.
	/// </summary>
	public abstract partial class IterateableTemplate : BaseTemplate
	{
		
		[Header("Settings")]
		[Tooltip("Allows to adjust the impact of this object (mostly for modifiers)")]
		public int level;
		
	}

}