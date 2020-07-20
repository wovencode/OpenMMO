//by Fhiz
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Debugging;

namespace OpenMMO
{
	
	/// <summary>
	/// Contains configurations shared by all Entities (Players, Monsters, NPCs) like Race, Class etc.
	/// </summary>
	[CreateAssetMenu(fileName = "New Entity Configuration", menuName = "OpenMMO - Configuration/New Entity Configuration", order = 999)]
	public partial class EntityConfigTemplate : ScriptableObject
	{
		
		// .. empty, is added via partial ...
		
		static EntityConfigTemplate _instance;
		
		/// <summary>
		/// Creates a singleton on this class to be accesible from code anywhere. Singleton is OK in this situation because this template (= Scriptable Object) exists only once.
		/// </summary>
		public static EntityConfigTemplate singleton
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<EntityConfigTemplate>().FirstOrDefault();
				return _instance;
			}
		}
		
	}

}