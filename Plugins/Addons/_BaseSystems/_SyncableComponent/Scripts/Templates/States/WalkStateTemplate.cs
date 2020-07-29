//by Fhiz
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// A basic, partial Template that represents the "Walk State".
	/// </summary>
	[CreateAssetMenu(fileName = "New WalkState", menuName = "OpenMMO - States/New WalkState", order = 999)]
	public partial class WalkStateTemplate : StateTemplate
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
				return ((PlayerAccount)mobileComponent).playerMovementComponent.Walking;
			else
				return mobileComponent.movementComponent.IsMoving;
		}

	}

}