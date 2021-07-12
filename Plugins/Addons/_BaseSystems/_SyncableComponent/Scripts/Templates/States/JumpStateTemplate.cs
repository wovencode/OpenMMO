//BY DX4D

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// A basic, partial Template that represents the "Jump State".
	/// </summary>
	[CreateAssetMenu(fileName = "New JumpState", menuName = "OpenMMO - States/New JumpState", order = 999)]
	public partial class JumpStateTemplate : StateTemplate
	{
    
		/// <summary>
		/// Returns if the state is currently active.
		/// </summary>
		public override bool GetIsActive(MobileComponent mobileComponent)
		{
            // -- special case:
            // only players have a "jump state" while monsters and NPCs do not

            if (!mobileComponent || !mobileComponent.movementComponent) return false;

            if (mobileComponent is PlayerAccount)
                return ((PlayerAccount)mobileComponent).playerMovementComponent.Jumping;
            else
                return false; //NPCs do not jump // mobileComponent.movementComponent.IsMoving;
		}

	}

}