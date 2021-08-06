//BY DX4D
using UnityEngine;
using UnityEngine.AI;

namespace OpenMMO
{
    [CreateAssetMenu(menuName = "OpenMMO/Controls/Basic Character Motor")]
    public partial class BasicCharacterMotor : CharacterMotor
    {
       // [Header("MOVEMENT CONFIG")]
       // [SerializeField] bool faceDirectionOfMovement = false;

        internal override Vector3 GetVelocity(MovementInput movement, MovementModifiers movementConfig, NavMeshAgent agent)
        {
            return CalculateVelocity(movement, movementConfig, agent);
        }
        
	    internal override float CalculateMoveSpeedFactor(MovementInput movement, MovementModifiers movementConfig)
	    {
		    float factor = movementConfig.moveSpeedMultiplier;
		    if (movement.running) factor *= movementConfig.runSpeedScale; //running
		    else	 factor *= movementConfig.walkSpeedScale; //walking
		    if (movement.sneaking) factor *= movementConfig.sneakSpeedScale; //sneaking
		    return factor;
	    }

        /// <summary>
        /// This recalculates the agent velocity based on the current input axis'
        /// </summary>
        protected virtual Vector3 CalculateVelocity(MovementInput movement, MovementModifiers movementConfig, NavMeshAgent agent)
        {
            //FACE DIRECTION OF TRAVEL
            //if (config.faceCameraDirection && Camera.main) transform.LookAt(agent.velocity, Vector3.up);

            if (movement.verticalInput != 0 || movement.horizontalInput != 0 || movement.strafeLeft || movement.strafeRight)
            {
                Vector3 input = new Vector3(movement.horizontalInput, 0, movement.verticalInput);
                if (input.magnitude > 1) input = input.normalized;

                //Vector3 angles = transform.rotation.eulerAngles;
                Vector3 angles =
                    (
                    (movementConfig.faceCameraDirection && Camera.main)
                    ? new Vector3(agent.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, agent.transform.rotation.eulerAngles.z)
                    : agent.transform.rotation.eulerAngles
                    );
                angles.x = 0;
                Quaternion rotation = Quaternion.Euler(angles);

                Vector3 direction = rotation * input;

                Vector3 newVelocity = Vector3.zero;

                if (movement.verticalInput > 0)                                  // -- Movement: Forward
                {
	                newVelocity = direction * movement.verticalInput * agent.speed * CalculateMoveSpeedFactor(movement, movementConfig);
                }
                else if (movement.verticalInput < 0)                             // -- Movement: Backward
                {
	                newVelocity = direction * Mathf.Abs(movement.verticalInput) * agent.speed * movementConfig.backpedalSpeedScale * CalculateMoveSpeedFactor(movement, movementConfig);
                }
                else if (movement.horizontalInput != 0) //STRAFE
                {
                    //NOTE: We do not want to factor run speed into strafing...we do not want both multipliers to make diagonal speed faster than forward speed.
                    //float factor = running ? config.runSpeedScale : config.walkSpeedScale; 
                    newVelocity = direction * agent.speed * movementConfig.walkSpeedScale * movementConfig.strafeSpeedScale * movementConfig.moveSpeedMultiplier;
                }

                //STRAFE LEFT
                if (movement.strafeLeft)
                {
                    if (!movement.strafeRight) //NOTE: Holding both turn buttons cancels out turning
                    {
                        //direction = -agent.transform.right;
                        //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                        if (movementConfig.turnWhileStrafing) agent.transform.Rotate(0, -1.0f * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //TURN WHILE STRAFING
                        newVelocity = -agent.transform.right * 5f * agent.speed * movementConfig.strafeSpeedScale * movementConfig.moveSpeedMultiplier;
                    }
                }
                //STRAFE RIGHT
                else if (movement.strafeRight)
                {
                    //direction = agent.transform.right;
                    //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                    if (movementConfig.turnWhileStrafing) agent.transform.Rotate(0, 1.0f * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //TURN WHILE STRAFING
                    //newVelocity = direction * horizontalMovementInput * agent.speed * config.strafeSpeedScale * config.moveSpeedMultiplier;
                    newVelocity = agent.transform.right * 5f * agent.speed * movementConfig.strafeSpeedScale * movementConfig.moveSpeedMultiplier;
                }
                if (!movement.strafeLeft && !movement.strafeRight)
                {
                    agent.transform.Rotate(0, movement.horizontalInput * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //SET ROTATION - ON TRANSFORM - NOT WHILE STRAFING
                }

                //JUMP
                if (movement.jumping)
                {
                    //direction = agent.transform.right;
                    //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                    //if (movementConfig.turnWhileStrafing) agent.transform.Rotate(0, 1.0f * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //TURN WHILE STRAFING
                    //newVelocity = direction * horizontalMovementInput * agent.speed * config.strafeSpeedScale * config.moveSpeedMultiplier;
                    newVelocity = (Quaternion.Euler(20f, 0, 0) * agent.transform.forward) * 5f * agent.speed * movementConfig.jumpSpeedScale * movementConfig.moveSpeedMultiplier;
                }

                return newVelocity; //SET VELOCITY - ON NAVMESH AGENT
            }
            else
            {
                // -- required?
                if (agent.isOnNavMesh) agent.ResetPath();
                return Vector3.zero;
            }
        }
    }
}
