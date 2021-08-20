//by Fhiz
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

using System.Collections.ObjectModel;

namespace OpenMMO {
	
	/// <summary>
	/// Partial abstract State Template as base class for all Animation State Templates.
	/// </summary>
	public abstract partial class StateTemplate : BaseTemplate
	{
    
		public static string _folderName = "";
		
		static StateTemplateDictionary _states;
		
		/// <summary>
		/// Abstract bool GetIsActive. Used to check if the Animation State is currently active.
		/// </summary>
		public abstract bool GetIsActive(MobileComponent mobileComponent);
		
		public static ReadOnlyDictionary<int, StateTemplate> states
		{
			get {
				StateTemplate.BuildCache();
				return _states.data;
			}
		}
		
		public static void BuildCache(bool forced=false)
		{
			if (_states == null || forced)
				_states = new StateTemplateDictionary(StateTemplate._folderName);
		}
		
		public void OnEnable()
		{
			if (_folderName != folderName)
				_folderName = folderName;
			
			_states = null;
			
		}
		
		public override void OnValidate()
		{
			base.OnValidate();
		}
		
	}

}