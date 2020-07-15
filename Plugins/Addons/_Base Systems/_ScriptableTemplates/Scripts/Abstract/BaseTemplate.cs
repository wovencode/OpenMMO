//by Fhiz
using System;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Abstract partial class BaseTemplate where most Scriptable Objects should derive from. It offers basic data like Icon, sort Order, Category and Description that is used often.
	/// </summary>
	public abstract partial class BaseTemplate : ScriptableTemplate
	{
		
		[Header("General Info")]
		[Tooltip("Used to categorize templates into groups (commonly used by Items, Skills, Buffs etc.)")]
		public string			sortCategory;
		[Tooltip("Used to determine sort order of lists (0 = high, 1+ = lower)")]
		public int				sortOrder;
		[Tooltip("Icon used to visualize this template (Item icon, Buff icon etc.)")]
		public Sprite 			smallIcon;
		[Tooltip("Background of icon used to visualize this template (background will be visible on icons with transparency)")]
		public Sprite			backgroundIcon;
		[Tooltip("Rarity of the template (commonly used by Items, Currencies, Equipment etc.)")]
		public RarityTemplate 	rarity;
		
		[Tooltip("Description of the template used as part of it's tooltip")]
		[TextArea(5,5)]
		public string 			description;				
		
		/// <summary>
		/// If the title is empty, we simply copy the object name into the title.
		/// </summary>
		/// <remarks>
		/// Note that we cannot cache the folderName right here, because it would the same for all objects of this type.
		/// </remarks>
		public override void OnValidate()
		{
			base.OnValidate();
		}
		
	}

}