//BY FHIZ
//MODIFIED BY DX4D

using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.UI;
using OpenMMO.Debugging;
using OpenMMO.Zones;
using UnityEngine;

namespace OpenMMO.Zones
{

    // ===================================================================================
    // StartingLocationAnchor
    // ===================================================================================
    [DisallowMultipleComponent]
	public class StartingLocationMarker : MonoBehaviour
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
			LocationMarkerManager.singleton.RegisterStartingLocationMarker(name, transform.position, archeTypes);
		}
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
        public void OnDestroy()
        {
        	if (LocationMarkerManager.singleton)
            	LocationMarkerManager.singleton.UnRegisterStartingLocationMarker(name);
        }
    
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================