
using System;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// ScriptableTemplate
	// ===================================================================================
	public abstract partial class ScriptableTemplate : ScriptableObject
	{
		
		[Header("Basic Info")]
		[Tooltip("This name will be displayed to the end-user (copied from object name if empty)")]
		public string 			title;				
		// we never use the object name as the name that is shown to users
		// instead we provide a custom string property called 'title'
		// this way, you can rename your objects anytime without messing up
		// the dictionary (as the dictionary keys are generated using the
		// true name of the object)
													
		[Tooltip("The name of a subfolder in the project 'Resources' folder (case-sensitive)")]
    	public string 			folderName;
    	// by specifying a subfolder, we can improve the Resources.LoadAll call a lot
    	// you just have to go sure that all templates of the corresponding type are
    	// in the correct subfolder
    	
		// we cache a few properties here for performance
		protected string 		_name;
		protected int			_hash;
		
		// -------------------------------------------------------------------------------
        // BuildCache
        // This methods builds the actual dictionary and fills it with data, its up to the
        // derived class how to do that.
        // -------------------------------------------------------------------------------
		//public abstract static void BuildCache(bool force=false);
		
		// -------------------------------------------------------------------------------
        // name
        // instead of returning the objects name each time it is accessed, we cache it in
        // a local variable and return that one (less allocations)
        // -------------------------------------------------------------------------------
		public new string name
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_name))
					_name = base.name;
				return _name;
			}
		}
		
		// -------------------------------------------------------------------------------
        // hash
        // instead of calculating the objects hash (from its name) everytime it is
        // accessed, we cache it in a local variable instead and return that one
        // -------------------------------------------------------------------------------
		public int hash
		{
			get {
				if (_hash == 0)
					_hash = name.GetDeterministicHashCode();
				return _hash;
			}
		}
		
		// -------------------------------------------------------------------------------
        // OnValidate
        // if the title is empty, we simply copy the object name into the title
        // note that we cannot cache the folderName here, because it would the same for all objects of this type
        // -------------------------------------------------------------------------------
		public virtual void OnValidate()
		{
	
			if (String.IsNullOrWhiteSpace(title))
				title = name;
			
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================