//by Fhiz
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Partial abstract State Template as base class for all Animation State Templates.
	/// </summary>
	public abstract partial class StateTemplate : BaseTemplate
	{
    
		public static string _folderName = "";
		
		static StateTemplateDictionary _data;
		
		/// <summary>
		/// Abstract bool GetIsActive. Used to check if the Animation State is currently active.
		/// </summary>
		public abstract bool GetIsActive(MobileComponent mobileComponent);
		
		public static ReadOnlyDictionary<int, StateTemplate> data
		{
			get {
				StateTemplate.BuildCache();
				return _data.data;
			}
		}
		
		public static void BuildCache(bool forced=false)
		{
			if (_data == null || forced)
				_data = new StateTemplateDictionary(StateTemplate._folderName);
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