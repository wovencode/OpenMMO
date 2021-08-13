//by Fhiz, DX4D
using UnityEngine;
using Mirror;

namespace OpenMMO {
	
	/// <summary>
    /// Partial PlayerMovementComponent class is responsible for all player related input and movement.
    /// </summary>
	public partial class PlayerMovementComponent
	{
		
		[Header("Performance")]
		[Tooltip("How often are movement updates sent to the server (in seconds)?")]
		[Range(0.01f, 99)]
		public double movementUpdateInterval = 1f;
        
		double _timerMovement = 0;
		
        /* //MOVED TO PLAYERMOTOR
		/// <summary>
    	/// This recalculates the agent velocity based on the current input axis'
   	 	/// </summary>
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
        */


        // S E R V E R  A U T H O R I T A T I V E  M O V E M E N T

		/// <summary>
    	/// Client-side, fixed update.
    	/// </summary>
		[Client]
		protected override void FixedUpdateClient()
    	{
        	if (isLocalPlayer && ReadyToMove()) // CHECK FOR THROTTLING
        	{
                Cmd_UpdateMovementState(new MovementInput(transform.position, transform.rotation, input));// verticalMovementInput, horizontalMovementInput, running, sneaking, strafeLeft, strafeRight));
				LogMovement();
			}
        	
		}

        /// <summary>
        /// Movement Throttling
        /// </summary>
        /// <returns>
        /// Enough time has passed...ready to move again.
        /// </returns>
        protected bool ReadyToMove() { return Time.time > _timerMovement; }

        
        /// <summary>
        /// Logs the last time that movement was processed.
        /// </summary>
        private void LogMovement()
        {
            _timerMovement = Time.time + movementUpdateInterval;
        }
		
        /// <summary>
        /// Sends movement state to the server, where the server updates velocity, then returns updated position info to clients.
        /// </summary>
        /// <param name="moveState"></param>
		[Command]
		protected virtual void Cmd_UpdateMovementState(MovementInput moveState)
    	{
    		
    		transform.position			= moveState.position;
    		transform.rotation			= moveState.rotation;
    		
    		input.verticalMovement 		= Mathf.Clamp(moveState.verticalInput, -1, 1);		// accurate enough for keyboard + controller
    		input.horizontalMovement 	= Mathf.Clamp(moveState.horizontalInput, -1, 1);    // accurate enough for keyboard + controller

            input.strafeLeft            = moveState.strafeLeft;
            input.strafeRight           = moveState.strafeRight;

            input.running				= moveState.running;
            input.sneaking				= moveState.sneaking;

            input.jumping				= moveState.jumping;

            agent.velocity = input.motor.GetVelocity(moveState, input.movement, agent); //UPDATE THE MOTOR

            //agent.transform.LookAt(agent.velocity, agent.transform.up); //look in the direction you are going
            RpcCorrectClientPosition(transform.position, transform.rotation, agent.velocity); //CLIENT CORRECTION
		} 
		
		/// <summary>
    	/// Corrects the Client's position based upon the Server's interpretation of the simulation
    	/// </summary>
		[ClientRpc]
   		public void RpcCorrectClientPosition(Vector3 _position, Quaternion _rotation, Vector3 _velocity)
    	{
    		if (isLocalPlayer) return; //IGNORE LOCAL CLIENTS //TODO: Are we positive that local player does not need correction?
    		
    		agent.ResetPath();
        	agent.velocity = _velocity;
        	transform.position = _position;
        	transform.rotation = _rotation;
        	
    	}
		
	}

}