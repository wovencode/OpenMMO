
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.UI;
using OpenMMO.DebugManager;
using OpenMMO.Portals;

namespace OpenMMO.Portals
{

	// ===================================================================================
	// StartAnchor
	// ===================================================================================
	[DisallowMultipleComponent]
	public class StartAnchor : MonoBehaviour
	{
		
		[Tooltip("Add any number of archetypes")]
		public ArchetypeTemplate[] archeTypes;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        public void Awake()
        {
            AnchorManager.RegisterStartAnchor(this.gameObject);
        }
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
        public void OnDestroy()
        {
            AnchorManager.UnRegisterStartAnchor(this.gameObject);
        }
    
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================