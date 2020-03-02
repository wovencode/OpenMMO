
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// WalkStateTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New WalkState", menuName = "OpenMMO - States/New WalkState", order = 999)]
	public partial class WalkStateTemplate : StateTemplate
	{
    
		// -------------------------------------------------------------------------------
        // GetIsActive
        // -------------------------------------------------------------------------------
		public override bool GetIsActive(EntityComponent entityComponent)
		{
			return entityComponent.movementComponent.GetIsMoving;
		}

		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================