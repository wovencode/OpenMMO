
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
using OpenMMO.Portals;

namespace OpenMMO.Portals
{

	// ===================================================================================
	// PortalAnchor
	// ===================================================================================
	[DisallowMultipleComponent]
	public class PortalAnchor : MonoBehaviour
	{
	
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        public void Awake()
        {
            AnchorManager.RegisterPortalAnchor(name, transform.position);
        }
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
        public void OnDestroy()
        {
            AnchorManager.UnRegisterPortalAnchor(name);
        }
    
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================