//using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
//using Mirror;
//using OpenMMO;

namespace OpenMMO
{

    // ===================================================================================
    // MovementStateInfo
    // ===================================================================================
    /// <summary>Holds data related to the current movement input state of a controlled character.</summary>
    public partial struct MovementStateInfo
    {
        //ORIENTATION
        public Vector3 position;
        public Quaternion rotation;

        //MOVE
        public float verticalMovementInput;
        public float horizontalMovementInput;
        public bool movementRunning;

        //TURN
        public bool movementStrafeLeft;
        public bool movementStrafeRight;

        // -------------------------------------------------------------------------------
        // MovementStruct (Constructor)
        // -------------------------------------------------------------------------------
        public MovementStateInfo(Vector3 _position, Quaternion _rotation, float _verticalMovementInput, float _horizontalMovementInput, bool _movementRunning, bool _movementStrafeLeft, bool _movementStrafeRight)
        {
            //ORIENTATION
            position = _position;
            rotation = _rotation;
            //MOVE
            verticalMovementInput = _verticalMovementInput;
            horizontalMovementInput = _horizontalMovementInput;
            movementRunning = _movementRunning;
            //TURN
            movementStrafeLeft = _movementStrafeLeft;
            movementStrafeRight = _movementStrafeRight;
        }

        // -------------------------------------------------------------------------------
    }
}
// =======================================================================================