
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Debugging;

namespace OpenMMO
{
	
	// ===================================================================================
	// EntityConfigTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Entity Configuration", menuName = "OpenMMO - Configuration/New Entity Configuration", order = 999)]
	public partial class EntityConfigTemplate : ScriptableObject
	{
		
		static EntityConfigTemplate _instance;
		
		// -------------------------------------------------------------------------------
		// singleton
		// -------------------------------------------------------------------------------
		public static EntityConfigTemplate singleton
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<EntityConfigTemplate>().FirstOrDefault();
				return _instance;
			}
		}
		
		// -------------------------------------------------------------------------------
	}

}

// =======================================================================================
