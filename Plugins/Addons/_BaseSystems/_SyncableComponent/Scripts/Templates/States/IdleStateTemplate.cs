//by Fhiz
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Basic template for the "Idle State".
	/// </summary>
	[CreateAssetMenu(fileName = "New IdleState", menuName = "OpenMMO - States/New IdleState", order = 999)]
	public partial class IdleStateTemplate : StateTemplate
	{
        
    	/// <summary>
		/// Checks if the state is currently active.
		/// </summary>
		public override bool GetIsActive(MobileComponent entityComponent)
		{
			return !entityComponent.movementComponent.GetIsMoving;
		}

	}

}