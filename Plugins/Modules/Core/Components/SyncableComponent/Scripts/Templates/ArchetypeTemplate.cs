//by Fhiz
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Archetypes represent the class or race of a entity and allow for base stats shared by all instances of that entity.
	/// </summary>
	[CreateAssetMenu(fileName = "New Archetype", menuName = "OpenMMO - Templates/New Archetype", order = 999)]
	public partial class ArchetypeTemplate : IterateableTemplate
	{
    	
    	/*
    		...added via partial...
    	*/

		public static string _folderName = "";
		
		static ArchetypeTemplateDictionary _data;
		
		public static ReadOnlyDictionary<int, ArchetypeTemplate> data
		{
			get {
				ArchetypeTemplate.BuildCache();
				return _data.data;
			}
		}
		
		public static void BuildCache(bool forced=false)
		{
			if (_data == null || forced)
				_data = new ArchetypeTemplateDictionary(ArchetypeTemplate._folderName);
		}
		
		public void OnEnable()
		{
			if (_folderName != folderName)
				_folderName = folderName;
			
			_data = null;
			
		}
		
		public override void OnValidate()
		{
			base.OnValidate();
		}
		
	}

}