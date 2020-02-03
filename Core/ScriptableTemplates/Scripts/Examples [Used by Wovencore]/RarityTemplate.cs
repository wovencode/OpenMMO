
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// BaseTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Rarity", menuName = "OpenMMO - Templates/New Rarity", order = 999)]
	public partial class RarityTemplate : ScriptableTemplate
	{
		
		public Color			color;
		public Sprite			borderImage;
		
		// -------------------------------------------------------------------------------
		
		public static string _folderName = "";	
		
		static RarityTemplateDictionary _data;
		
		// -------------------------------------------------------------------------------
        // data
        // -------------------------------------------------------------------------------
		public static ReadOnlyDictionary<int, RarityTemplate> data
		{
			get {
				RarityTemplate.BuildCache();
				return _data.data;
			}
		}
		
		// -------------------------------------------------------------------------------
        // BuildCache
        // -------------------------------------------------------------------------------
		public static void BuildCache(bool forced=false)
		{
			if (_data == null || forced)
				_data = new RarityTemplateDictionary(RarityTemplate._folderName);
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
        // -------------------------------------------------------------------------------
		public override void OnValidate()
		{
			base.OnValidate();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================