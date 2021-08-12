//by Fhiz
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// A basic, partial Template that represents the "Run State".
	/// </summary>
	[CreateAssetMenu(fileName = "New RunState", menuName = "OpenMMO - States/New RunState", order = 999)]
	public partial class RunStateTemplate : StateTemplate
	{
    
		/// <summary>
		/// Returns if the state is currently active.
		/// </summary>
		public override bool GetIsActive(MobileComponent mobileComponent)
		{
            // -- special case:
            // only players have a "walk state" while monsters and NPCs only feature
            // a "move state".

            if (!mobileComponent || !mobileComponent.movementComponent) return false;

            if (mobileComponent is PlayerAccount)
				return ((PlayerAccount)mobileComponent).playerMovementComponent.Running;
			else
				return mobileComponent.movementComponent.IsMoving;
		}

	}

}