
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// MovementStruct
	// ===================================================================================
	public partial struct MovementStruct
	{
		
		public Quaternion 	movementRotation;
		public float 		verticalMovementInput;
		public float 		horizontalMovementInput;
		public bool 		movementRunning;
		
		// -------------------------------------------------------------------------------
		// MovementStruct (Constructor)
		// -------------------------------------------------------------------------------
		public MovementStruct(Quaternion _movementRotation, float _verticalMovementInput, float _horizontalMovementInput, bool _movementRunning)
		{
			movementRotation		= _movementRotation;
			verticalMovementInput 	= _verticalMovementInput;
			horizontalMovementInput	= _horizontalMovementInput;
			movementRunning 		= _movementRunning;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================