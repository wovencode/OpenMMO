
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// FooTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New FooTemplate", menuName = "Templates/New FooTemplate", order = 999)]
	public partial class FooTemplate : ScriptableTemplate
	{
    
    	/*
    	
    		Add your custom properties here, like:
    		
    		public int level;
    		public Sprite icon;
    		public string description;
    	
    	*/
    	
    	// performance improvement:
    	// Resources.LoadAll on a specific folder is much faster than on the whole Resources folder
		public static string _folderName = "";
		
		// this is our custom class that holds the dictionary data
		static FooTemplateDictionary _data;
		
		// -------------------------------------------------------------------------------
        // data
        // loads the dictionary the first time it is accessed
        // skips if there are any duplicates and notifies the user
        // returns the cached dictionary
        // -------------------------------------------------------------------------------
		public static ReadOnlyDictionary<int, FooTemplate> data
		{
			get {
				FooTemplate.BuildCache();
				return _data.data;
			}
		}
		
		// -------------------------------------------------------------------------------
        // BuildCache
        // called when the dictionary is accessed the first time in order to build it
        // BuildCache can be called manually as well to load the dictionary
        // -------------------------------------------------------------------------------
		public static void BuildCache(bool forced=false)
		{
			if (_data == null || forced)
				_data = new FooTemplateDictionary(FooTemplate._folderName);
		}
		
		// -------------------------------------------------------------------------------
        // OnEnable
        // We have to cache the folder name here (and not on the base class),
        // otherwise it would be the same for all objects
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
	
			base.OnValidate(); // always call base OnValidate as well!
			
			/*
				Example validation check:
				
				level = Mathf.Max(1, level);
			*/
			
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================