//by Fhiz
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	[CreateAssetMenu(fileName = "New Rarity", menuName = "OpenMMO - Templates/New Rarity", order = 999)]
	public partial class RarityTemplate : ScriptableTemplate
	{
		
		public Color			color;
		public Sprite			borderImage;
		
		public static string _folderName = "";	
		
		static RarityTemplateDictionary _data;
		
		public static ReadOnlyDictionary<int, RarityTemplate> data
		{
			get {
				RarityTemplate.BuildCache();
				return _data.data;
			}
		}
		
		public static void BuildCache(bool forced=false)
		{
			if (_data == null || forced)
				_data = new RarityTemplateDictionary(RarityTemplate._folderName);
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