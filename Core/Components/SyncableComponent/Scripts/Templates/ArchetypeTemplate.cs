
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// ArchetypeTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Archetype", menuName = "OpenMMO - Templates/New Archetype", order = 999)]
	public partial class ArchetypeTemplate : IterateableTemplate
	{
    	
    	/*
    		Reserved for future functionality
    	*/

    	// -------------------------------------------------------------------------------
    	
		public static string _folderName = "";
		
		static ArchetypeTemplateDictionary _data;
		
		// -------------------------------------------------------------------------------
        // data
        // -------------------------------------------------------------------------------
		public static ReadOnlyDictionary<int, ArchetypeTemplate> data
		{
			get {
				ArchetypeTemplate.BuildCache();
				return _data.data;
			}
		}
		
		// -------------------------------------------------------------------------------
        // BuildCache
        // -------------------------------------------------------------------------------
		public static void BuildCache(bool forced=false)
		{
			if (_data == null || forced)
				_data = new ArchetypeTemplateDictionary(ArchetypeTemplate._folderName);
		}
		
		// -------------------------------------------------------------------------------
        // OnEnable
        // -------------------------------------------------------------------------------
		public void OnEnable()
		{
			if (_folderName != folderName)
				_folderName = folderName;
			
			_data = null;
			
		}
		
		// -------------------------------------------------------------------------------
        // OnValidate
        // You can add custom validation checks here
        // -------------------------------------------------------------------------------
		public override void OnValidate()
		{
			base.OnValidate();
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================