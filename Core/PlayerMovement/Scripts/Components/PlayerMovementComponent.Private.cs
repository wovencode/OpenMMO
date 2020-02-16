
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// PlayerMovement
	// ===================================================================================
	public partial class PlayerMovementComponent
	{
		
		[Header("Performance")]
		[Tooltip("How often are movement updates sent to the server (in seconds)?")]
		[Range(0.01f, 99)]
		public double movementUpdateInterval = 1f;
        
		double _timerMovement = 0;
		
		// -------------------------------------------------------------------------------
		// CheckMovementInterval
		// -------------------------------------------------------------------------------
		protected bool CheckMovementInterval => Time.time > _timerMovement;
		
		// -------------------------------------------------------------------------------
		// RefreshMovementInterval
		// -------------------------------------------------------------------------------
		void RefreshMovementInterval()
		{
			_timerMovement = Time.time + movementUpdateInterval;
		}
		
		// -------------------------------------------------------------------------------
		// FixedUpdateClient
		// @Client
		// -------------------------------------------------------------------------------
		[Client]
		protected override void FixedUpdateClient()
    	{
        	if (isLocalPlayer									// only for local Player
        		&& CheckMovementInterval						// we throttle a little bit
        		)
        	{
				Cmd_UpdateState(new MovementStruct(transform.position, transform.rotation, verticalMovementInput, horizontalMovementInput, running, strafing, strafeLeft, strafeRight));
				RefreshMovementInterval();
			}
        	
		}
		
		// -------------------------------------------------------------------------------
		// Cmd_UpdateState
		// @Client -> @Server
		// -------------------------------------------------------------------------------
		[Command]
		protected virtual void Cmd_UpdateState(MovementStruct movementStruct)
    	{
    		
    		transform.position			= movementStruct.position;
    		transform.rotation			= movementStruct.rotation;
    		
    		verticalMovementInput 		= Mathf.Clamp(movementStruct.verticalMovementInput, -1, 1);		// good enough for keyboard + controller
    		horizontalMovementInput 	= Mathf.Clamp(movementStruct.horizontalMovementInput, -1, 1);	// good enough for keyboard + controller
    		running						= movementStruct.movementRunning;
    		strafing                    = movementStruct.movementStrafing;

    		strafeLeft                    = movementStruct.movementTurnLeft;
    		strafeRight                   = movementStruct.movementTurnRight;
    		
    		UpdateVelocity();
    		RpcPosition(transform.position, transform.rotation, agent.velocity);
		}
		
		// -------------------------------------------------------------------------------
		// UpdateVelocity
		// This recalculates the agent velocity based on the current input axis'
		// @Client / @Server
		// -------------------------------------------------------------------------------
		protected virtual void UpdateVelocity()
        {
            //FACE DIRECTION OF TRAVEL
            //if (config.faceCameraDirection && Camera.main) transform.LookAt(agent.velocity, Vector3.up);

            if (verticalMovementInput != 0 || horizontalMovementInput != 0)
           	{
				Vector3 input = new Vector3(horizontalMovementInput, 0, verticalMovementInput);
				if (input.magnitude > 1) input = input.normalized;

                //Vector3 angles = transform.rotation.eulerAngles;
                Vector3 angles = 
                    (
                    (config.faceCameraDirection && Camera.main)
                    ? new Vector3(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)
                    : transform.rotation.eulerAngles
                    );
				angles.x = 0;
				Quaternion rotation = Quaternion.Euler(angles);

                Vector3 direction = rotation * input;

                Vector3 newVelocity = Vector3.zero;

                    if (verticalMovementInput > 0)                                  // -- Movement: Forward
                    {
                        float factor = running ? config.runSpeedScale : config.walkSpeedScale;
                        newVelocity = direction * verticalMovementInput * agent.speed * factor * config.moveSpeedMultiplier;
                    }
                    else if (verticalMovementInput < 0)                             // -- Movement: Backward
                    {
                        float factor = running ? config.runSpeedScale : config.walkSpeedScale;
                        newVelocity = direction * Mathf.Abs(verticalMovementInput) * agent.speed * factor * config.backpedalSpeedScale * config.moveSpeedMultiplier;
                    }
                    else if (horizontalMovementInput != 0) //STRAFE
                    {
                        //NOTE: We do not want to factor run speed into strafing...we do not want both multipliers to make diagonal speed faster than forward speed.
                        //float factor = running ? config.runSpeedScale : config.walkSpeedScale; 
                        newVelocity = direction * agent.speed * config.strafeSpeedScale * config.moveSpeedMultiplier;
                    }
                //else agent.velocity = Vector3.zero; // -- required? //DEPRECIATED: This now happens below

                /*if (config.turnWhileStrafing && !strafing) //TURN IF NOT STRAFING
                {
                    if (horizontalMovementInput != 0)       // -- Rotation (when not strafing)
                    {
                        transform.Rotate(0, horizontalMovementInput * config.turnSpeedMultiplier, 0);
                    }
                }*/

                //STRAFE LEFT
                if (strafeLeft)
                {
                    if (!strafeRight) //NOTE: Holding both turn buttons cancels out turning
                    {
                        direction = -agent.transform.right;
                        //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                        if (config.turnWhileStrafing) transform.Rotate(0, -1.0f * config.turnSpeedMultiplier, 0); //TURN WHILE STRAFING
                        newVelocity = direction * 5f * agent.speed * config.strafeSpeedScale * config.moveSpeedMultiplier;
                    }
                }
                //STRAFE RIGHT
                else if (strafeRight)
                {
                    direction = agent.transform.right;
                    //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                    if (config.turnWhileStrafing) transform.Rotate(0, 1.0f * config.turnSpeedMultiplier, 0); //TURN WHILE STRAFING
                    //newVelocity = direction * horizontalMovementInput * agent.speed * config.strafeSpeedScale * config.moveSpeedMultiplier;
                    newVelocity = direction * 5f * agent.speed * config.strafeSpeedScale * config.moveSpeedMultiplier;
                }

                agent.velocity = newVelocity; //SET VELOCITY - ON NAVMESH AGENT
                if (!strafeLeft && !strafeRight) transform.Rotate(0, horizontalMovementInput * config.turnSpeedMultiplier, 0); //SET ROTATION - ON TRANSFORM - NOT WHILE STRAFING
           	}
           	else
           	{
           		// -- required?
         	  	agent.ResetPath();
				agent.velocity = Vector3.zero;
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// RpcPosition
		// Updates the rotation, position and velocity on clients based on server stats
		// @Server -> @Clients
		// -------------------------------------------------------------------------------
		[ClientRpc]
   		public void RpcPosition(Vector3 _position, Quaternion _rotation, Vector3 _velocity)
    	{
    	
    		// -- not required to update local player
    		if (isLocalPlayer)
    			return;
    		
    		agent.ResetPath();
        	agent.velocity = _velocity;
        	transform.position = _position;
        	transform.rotation = _rotation;
        	
    	}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================