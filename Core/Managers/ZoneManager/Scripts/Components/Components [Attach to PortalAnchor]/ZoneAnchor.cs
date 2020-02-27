
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
	// ZoneAnchor
	// ===================================================================================
	[DisallowMultipleComponent]
	public class ZoneAnchor : MonoBehaviour
	{
	
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
			AnchorManager.singleton.RegisterPortalAnchor(name, transform.position);
		}
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
        public void OnDestroy()
        {
        	if (AnchorManager.singleton)
            	AnchorManager.singleton.UnRegisterPortalAnchor(name);
        }
    
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================