
using System;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// BaseTemplate
	// ===================================================================================
	public abstract partial class BaseTemplate : ScriptableTemplate
	{
		
		
		public string			sortCategory;
		public int				sortOrder;
		
		public Sprite 			smallIcon;
		public Sprite			backgroundIcon;
		public RarityTemplate 	rarity;
		
		[TextArea(5,5)]
		public string 			description;				
		
		// -------------------------------------------------------------------------------
        // OnValidate
        // if the title is empty, we simple copy the object name into the title
        // note that we cannot cache the folderName here, because it would the same for all objects of this type
        // -------------------------------------------------------------------------------
		public override void OnValidate()
		{
			base.OnValidate();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================