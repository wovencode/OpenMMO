
//using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using Mirror;
//using OpenMMO;

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
		// UpdateVelocity
		// This recalculates the agent velocity based on the current input axis'
		// @Client / @Server
		// -------------------------------------------------------------------------------
		protected virtual void UpdateVelocity()
        {
            //FACE DIRECTION OF TRAVEL
            //if (config.faceCameraDirection && Camera.main) transform.LookAt(agent.velocity, Vector3.up);

            if (verticalMovementInput != 0 || horizontalMovementInput != 0 || strafeLeft || strafeRight)
           	{
				Vector3 input = new Vector3(horizontalMovementInput, 0, verticalMovementInput);
				if (input.magnitude > 1) input = input.normalized;

                //Vector3 angles = transform.rotation.eulerAngles;
                Vector3 angles = 
                    (
                    (movementConfig.faceCameraDirection && Camera.main)
                    ? new Vector3(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)
                    : transform.rotation.eulerAngles
                    );
				angles.x = 0;
				Quaternion rotation = Quaternion.Euler(angles);

                Vector3 direction = rotation * input;

                Vector3 newVelocity = Vector3.zero;

                    if (verticalMovementInput > 0)                                  // -- Movement: Forward
                    {
                        float factor = running ? movementConfig.runSpeedScale : movementConfig.walkSpeedScale;
                        newVelocity = direction * verticalMovementInput * agent.speed * factor * movementConfig.moveSpeedMultiplier;
                    }
                    else if (verticalMovementInput < 0)                             // -- Movement: Backward
                    {
                        float factor = running ? movementConfig.runSpeedScale : movementConfig.walkSpeedScale;
                        newVelocity = direction * Mathf.Abs(verticalMovementInput) * agent.speed * factor * movementConfig.backpedalSpeedScale * movementConfig.moveSpeedMultiplier;
                    }
                    else if (horizontalMovementInput != 0) //STRAFE
                    {
                        //NOTE: We do not want to factor run speed into strafing...we do not want both multipliers to make diagonal speed faster than forward speed.
                        //float factor = running ? config.runSpeedScale : config.walkSpeedScale; 
                        newVelocity = direction * agent.speed * movementConfig.strafeSpeedScale * movementConfig.moveSpeedMultiplier;
                    }
                
                //STRAFE LEFT
                if (strafeLeft)
                {
                    if (!strafeRight) //NOTE: Holding both turn buttons cancels out turning
                    {
                        //direction = -agent.transform.right;
                        //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                        if (movementConfig.turnWhileStrafing) transform.Rotate(0, -1.0f * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //TURN WHILE STRAFING
                        newVelocity = -agent.transform.right * 5f * agent.speed * movementConfig.strafeSpeedScale * movementConfig.moveSpeedMultiplier;
                    }
                }
                //STRAFE RIGHT
                else if (strafeRight)
                {
                    //direction = agent.transform.right;
                    //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                    if (movementConfig.turnWhileStrafing) transform.Rotate(0, 1.0f * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //TURN WHILE STRAFING
                    //newVelocity = direction * horizontalMovementInput * agent.speed * config.strafeSpeedScale * config.moveSpeedMultiplier;
                    newVelocity = agent.transform.right * 5f * agent.speed * movementConfig.strafeSpeedScale * movementConfig.moveSpeedMultiplier;
                }

                if (!strafeLeft && !strafeRight) transform.Rotate(0, horizontalMovementInput * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //SET ROTATION - ON TRANSFORM - NOT WHILE STRAFING

                agent.velocity = newVelocity; //SET VELOCITY - ON NAVMESH AGENT
           	}
           	else
           	{
           		// -- required?
         	  	if (agent.isOnNavMesh) agent.ResetPath();
				agent.velocity = Vector3.zero;
			}
			
		}


        // S E R V E R  A U T H O R I T A T I V E  M O V E M E N T

		// -------------------------------------------------------------------------------
		// FixedUpdateClient
		// @Client
		// -------------------------------------------------------------------------------
		[Client]
		protected override void FixedUpdateClient()
    	{
        	if (isLocalPlayer && ReadyToMove()) // CHECK FOR THROTTLING
        	{
				Cmd_UpdateMovementState(new MovementStateInfo(transform.position, transform.rotation, verticalMovementInput, horizontalMovementInput, running, strafeLeft, strafeRight));
				LogMovement();
			}
        	
		}

        // -------------------------------------------------------------------------------
        // ReadyToMove
        // -------------------------------------------------------------------------------
        /// <summary>Movement Throttling</summary>
        /// <returns>Enough time has passed...ready to move again.</returns>
        protected bool ReadyToMove() { return Time.time > _timerMovement; }

        // -------------------------------------------------------------------------------
        // LogMovement
        // -------------------------------------------------------------------------------
        /// <summary>Logs the last time that movement was processed.</summary>
        private void LogMovement()
        {
            _timerMovement = Time.time + movementUpdateInterval;
        }
		
		
		// -------------------------------------------------------------------------------
		// Cmd_UpdateState
		// @Client -> @Server
		// -------------------------------------------------------------------------------
        /// <summary>Sends movement state to the server, where the server updates velocity, then returns updated position info to clients.</summary>
        /// <param name="moveState"></param>
		[Command]
		protected virtual void Cmd_UpdateMovementState(MovementStateInfo moveState)
    	{
    		
    		transform.position			= moveState.position;
    		transform.rotation			= moveState.rotation;
    		
    		verticalMovementInput 		= Mathf.Clamp(moveState.verticalMovementInput, -1, 1);		// good enough for keyboard + controller
    		horizontalMovementInput 	= Mathf.Clamp(moveState.horizontalMovementInput, -1, 1);	// good enough for keyboard + controller
    		running						= moveState.movementRunning;

    		strafeLeft                    = moveState.movementStrafeLeft;
    		strafeRight                   = moveState.movementStrafeRight;
    		
    		UpdateVelocity();
    		RpcCorrectClientPosition(transform.position, transform.rotation, agent.velocity);
		}
		
		// -------------------------------------------------------------------------------
		// RpcCorrectClientPosition
		// Updates the rotation, position and velocity on clients based on server stats
		// @Server -> @Clients
		// -------------------------------------------------------------------------------
        /// <summary>Corrects the Client's position based upon the Server's interpretation of the simulation.</summary>
		[ClientRpc]
   		public void RpcCorrectClientPosition(Vector3 _position, Quaternion _rotation, Vector3 _velocity)
    	{
    		if (isLocalPlayer) return; //IGNORE LOCAL CLIENTS //TODO: Are we positive that local player does not need correction?
    		
    		agent.ResetPath();
        	agent.velocity = _velocity;
        	transform.position = _position;
        	transform.rotation = _rotation;
        	
    	}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================