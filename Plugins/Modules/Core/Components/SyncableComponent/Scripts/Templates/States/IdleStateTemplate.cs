
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// IdleStateTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New IdleState", menuName = "OpenMMO - States/New IdleState", order = 999)]
	public partial class IdleStateTemplate : StateTemplate
	{
        
    	// -------------------------------------------------------------------------------
        // GetIsActive
        // -------------------------------------------------------------------------------
		public override bool GetIsActive(EntityComponent entityComponent)
		{
			return !entityComponent.movementComponent.GetIsMoving;
		}

		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================