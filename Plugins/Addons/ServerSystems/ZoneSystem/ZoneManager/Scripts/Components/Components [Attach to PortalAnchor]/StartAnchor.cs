
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.UI;
using OpenMMO.Debugging;
using OpenMMO.Zones;

namespace OpenMMO.Zones
{

	// ===================================================================================
	// StartAnchor
	// ===================================================================================
	[DisallowMultipleComponent]
	public class StartAnchor : MonoBehaviour
	{
		
		[Tooltip("Add any number of archetypes who can start the game here")]
		public ArchetypeTemplate[] archeTypes;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        public void Awake()
        {
            Invoke(nameof(AwakeLate), 0.1f);
        }
		
		// -------------------------------------------------------------------------------
		// AwakeLate
		// -------------------------------------------------------------------------------
		void AwakeLate()
		{
			AnchorManager.singleton.RegisterStartAnchor(name, transform.position, archeTypes);
		}
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
        public void OnDestroy()
        {
        	if (AnchorManager.singleton)
            	AnchorManager.singleton.UnRegisterStartAnchor(name);
        }
    
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================