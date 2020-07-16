//by Fhiz
using System;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// This is the base class all templates (= Scriptable Objects) should derive from in order to benefit from performance increases due to caching and exclusive dictionaries.
	/// </summary>
	public abstract partial class ScriptableTemplate : ScriptableObject
	{
		
		/// <summary>
		/// We never use the object name as the name that is shown to users, instead we provide a custom string property called 'title'.
		/// This way, you can rename your objects anytime without messing up the dictionary (as the dictionary keys are generated using the true name of the object).
		/// </summary>
		[Header("Basic Info")]
		[Tooltip("This name will be displayed to the end-user (copied from object name if empty)")]
		public string 			title;				
		
		/// <summary>
		/// By specifying a subfolder, we can improve the Resources.LoadAll call a lot. You just have to go sure that
		/// all templates of the corresponding type are in the correct subfolder.
		/// </summary>												
		[Tooltip("The name of a subfolder in the project 'Resources' folder (case-sensitive)")]
    	public string 			folderName;
    	
    	/// <summary>
		/// We cache a few properties here for performance
		/// </summary>		
		protected string 		_name;
		protected int			_hash;
		
		/// <summary>
		/// This methods builds the actual dictionary and fills it with data, its up to the derived class how to do that.
		/// </summary>
		/// <remarks>Must be disabled as it is up to the derived class to care about that.</remarks>
		//public abstract static void BuildCache(bool force=false);
		
		/// <summary>
		/// Instead of returning the objects (base) name each time it is accessed, we cache it in a local variable and return that one (less allocations)
		/// </summary>
		public new string name
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_name))
					_name = base.name;
				return _name;
			}
		}
		
		/// <summary>
		/// Instead of calculating the objects hash (from its name) everytime it is accessed, we cache it in a local variable instead and return that one
		/// </summary>
		public int hash
		{
			get {
				if (_hash == 0)
					_hash = name.GetDeterministicHashCode();
				return _hash;
			}
		}
		
		/// <summary>
		/// If the title is empty, we simply copy the object name into the title
		/// </summary>
		/// <remarks>
		/// Note that we cannot cache the folderName here, because it would the same for all objects of this type
		/// </remarks>
		public virtual void OnValidate()
		{
	
			if (String.IsNullOrWhiteSpace(title))
				title = name;
			
		}
		
	}

}