//BY DX4D
using UnityEngine;
using UnityEngine.AI;

namespace OpenMMO
{
    [CreateAssetMenu(menuName = "OpenMMO/Controls/World Centric Character Motor")]
    public partial class WorldCentricCharacterMotor : CharacterMotor
    {
        //GET VELOCITY
        internal override Vector3 GetVelocity(MovementInput movement, MovementModifiers modifiers, NavMeshAgent agent)
        {
            return CalculateVelocity(movement, modifiers, agent);
        }
	    internal override float CalculateMoveSpeedFactor(MovementInput movement, MovementModifiers movementConfig)
	    {
		    float factor = movementConfig.moveSpeedMultiplier;
		    if (movement.running) factor *= movementConfig.runSpeedScale; //running
		    else	 factor *= movementConfig.walkSpeedScale; //walking
		    if (movement.sneaking) factor *= movementConfig.sneakSpeedScale; //sneaking
		    return factor;
	    }

        //CALCULATE VELOCITY
        /// <summary>
        /// Calculates an agent's velocity based on a set of modifiers
        /// </summary>
        protected virtual Vector3 CalculateVelocity(MovementInput input, MovementModifiers modifiers, NavMeshAgent agent)
        {
            Vector3 movement = new Vector3(
                input.verticalInput,
                0.0f,
                -input.horizontalInput);

            //movement.x = Camera.main.transform.forward;

            //SPEED FACTOR
            //NOTE: Run speed and Sneak speed will blend together.
            float factor;
            if (!input.running) factor = modifiers.walkSpeedScale; //walking
            else factor = modifiers.runSpeedScale; //running

            if (input.sneaking) factor *= modifiers.sneakSpeedScale; //sneaking

            if (movement != Vector3.zero)
            {
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(movement), agent.angularSpeed * factor);
            }
            
	        agent.transform.Translate(movement * agent.speed * CalculateMoveSpeedFactor(input, modifiers) * Time.deltaTime, Space.World);// * agent.speed * factor * modifiers.moveSpeedMultiplier

            return agent.velocity;
            /*
            if (input.verticalInput != 0 || input.horizontalInput != 0 || input.strafeLeft || input.strafeRight)
            {
                //GET ROTATION
                Vector3 angles = agent.transform.rotation.eulerAngles;
                    //(
                    //(modifiers.faceCameraDirection && Camera.main)
                    //? new Vector3(agent.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, agent.transform.rotation.eulerAngles.z)
                    //: agent.transform.rotation.eulerAngles
                    //);
                angles.x = 0;
                Quaternion rotation = Quaternion.Euler(angles);

                //GET INPUT VECTOR
                Vector3 inputVector = new Vector3(input.horizontalInput, 0, input.verticalInput);

                if (inputVector.magnitude > 1) inputVector = inputVector.normalized; //NORMALIZE INPUT

                //CALCULATE DIRECTION
                Vector3 direction = rotation * inputVector;

                //INITIALIZE NEW VELOCITY
                Vector3 newVelocity = Vector3.zero;

                //SPEED FACTOR
                //NOTE: Run speed and Sneak speed will blend together.
                float factor = input.running ? modifiers.runSpeedScale : modifiers.walkSpeedScale; //running
                factor *= input.sneaking ? modifiers.sneakSpeedScale : 1f; //sneaking

                //CALCULATE FORWARD VELOCITY
                if (input.verticalInput > 0)
                {
                    direction = Vector3.forward;// agent.transform.forward;
                    agent.transform.LookAt(Vector3.forward, Vector3.up);
                    //agent.transform.forward = direction; //FACE DIRECTION OF TRAVEL
                    newVelocity = direction * input.verticalInput * agent.speed * factor * modifiers.moveSpeedMultiplier; //velocity
                }
                //CALCULATE BACKWARDS VELOCITY
                else if (input.verticalInput < 0)
                {
                    direction = -Vector3.forward;// -agent.transform.forward;
                    agent.transform.LookAt(-Vector3.forward, Vector3.up);
                    //agent.transform.forward = direction; //FACE DIRECTION OF TRAVEL
                    newVelocity = direction * Mathf.Abs(input.verticalInput) * agent.speed * factor * modifiers.backpedalSpeedScale * modifiers.moveSpeedMultiplier;
                }

                //CALCULATE RIGHT VELOCITY
                if (input.horizontalInput > 0)
                {
                    direction = Vector3.right;// agent.transform.right;
                    agent.transform.LookAt(Vector3.right, Vector3.up);
                    //agent.transform.forward = direction; //FACE DIRECTION OF TRAVEL
                    newVelocity = direction * input.horizontalInput * agent.speed * factor * modifiers.moveSpeedMultiplier; //velocity
                }
                //CALCULATE LEFT VELOCITY
                else if (input.horizontalInput < 0)
                {
                    direction = -Vector3.right;// -agent.transform.right;
                    agent.transform.LookAt(-Vector3.right, Vector3.up);
                    //agent.transform.forward = direction; //FACE DIRECTION OF TRAVEL
                    newVelocity = direction * Mathf.Abs(input.horizontalInput) * agent.speed * factor * modifiers.moveSpeedMultiplier;
                }
                //CALCULATE STRAFE VELOCITY
                //else if (input.horizontalInput != 0)
                //{
                //NOTE: We do not want to factor run speed into strafing...we do not want both multipliers to make diagonal speed faster than forward speed.
                //float factor = running ? config.runSpeedScale : config.walkSpeedScale; //running
                //factor *= input.sneaking ? modifiers.sneakSpeedScale : 1f; //sneaking
                //    newVelocity = direction * agent.speed * modifiers.strafeSpeedScale * modifiers.moveSpeedMultiplier;
                //}

                //CALCULATE STRAFE LEFT
                if (input.strafeLeft)
                {
                    if (!input.strafeRight) //NOTE: Holding both turn buttons cancels out turning
                    {
                        //direction = -agent.transform.right;
                        //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                        if (modifiers.turnWhileStrafing) agent.transform.Rotate(0, -1.0f * modifiers.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //TURN WHILE STRAFING
                        newVelocity = -agent.transform.right * 5f * agent.speed * modifiers.strafeSpeedScale * modifiers.moveSpeedMultiplier;
                    }
                }
                //CALCULATE STRAFE RIGHT
                else if (input.strafeRight)
                {
                    //direction = agent.transform.right;
                    //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                    if (modifiers.turnWhileStrafing) agent.transform.Rotate(0, 1.0f * modifiers.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //TURN WHILE STRAFING
                    //newVelocity = direction * horizontalMovementInput * agent.speed * config.strafeSpeedScale * config.moveSpeedMultiplier;
                    newVelocity = agent.transform.right * 5f * agent.speed * modifiers.strafeSpeedScale * modifiers.moveSpeedMultiplier;
                }
                //TURN
                //else //if (!movement.movementStrafeLeft && !movement.movementStrafeRight)
                //{
                //    agent.transform.Rotate(0, input.horizontalInput * modifiers.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //SET ROTATION - ON TRANSFORM - NOT WHILE STRAFING
                //}

               
                //agent.transform.forward = direction; //FACE DIRECTION OF TRAVEL
                //NOTE: Choose one LookAt or the other or the last one cancels the rest out.
                //agent.transform.LookAt(direction, agent.transform.up); //look in the direction you are going
                //agent.transform.LookAt(direction, Vector3.up); //rotate around worldspace up

                //RETURN NEW VELOCITY
                return newVelocity; 
            }
            //else
            //{
            //    if (agent.isOnNavMesh) agent.ResetPath(); //NOTE: May not be required, but called as a failsafe.
                return Vector3.zero;
            //}
            */
        }
    }
}
