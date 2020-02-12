
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
		
		public Vector3		position;
		public Quaternion 	rotation;
		public float 		verticalMovementInput;
		public float 		horizontalMovementInput;
		public bool 		movementRunning;
		
		// -------------------------------------------------------------------------------
		// MovementStruct (Constructor)
		// -------------------------------------------------------------------------------
		public MovementStruct(Vector3 _position, Quaternion _rotation, float _verticalMovementInput, float _horizontalMovementInput, bool _movementRunning)
		{
			position				= _position;
			rotation				= _rotation;
			verticalMovementInput 	= _verticalMovementInput;
			horizontalMovementInput	= _horizontalMovementInput;
			movementRunning 		= _movementRunning;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================