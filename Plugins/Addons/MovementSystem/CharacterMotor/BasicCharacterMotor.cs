//BY DX4D
using UnityEngine;
using UnityEngine.AI;

namespace OpenMMO
{
    [CreateAssetMenu(menuName = "OpenMMO/Controls/Basic Character Motor")]
    public partial class BasicCharacterMotor : CharacterMotor
    {
        internal override Vector3 GetVelocity(MovementStateInfo movement, MovementModifiers movementConfig, NavMeshAgent agent)
        {
            return CalculateVelocity(movement, movementConfig, agent);
        }

        /// <summary>
        /// This recalculates the agent velocity based on the current input axis'
        /// </summary>
        protected virtual Vector3 CalculateVelocity(MovementStateInfo movement, MovementModifiers movementConfig, NavMeshAgent agent)
        {
            //FACE DIRECTION OF TRAVEL
            //if (config.faceCameraDirection && Camera.main) transform.LookAt(agent.velocity, Vector3.up);

            if (movement.verticalMovementInput != 0 || movement.horizontalMovementInput != 0 || movement.movementStrafeLeft || movement.movementStrafeRight)
            {
                Vector3 input = new Vector3(movement.horizontalMovementInput, 0, movement.verticalMovementInput);
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

                if (movement.verticalMovementInput > 0)                                  // -- Movement: Forward
                {
                    float factor = movement.movementRunning ? movementConfig.runSpeedScale : movementConfig.walkSpeedScale;
                    newVelocity = direction * movement.verticalMovementInput * agent.speed * factor * movementConfig.moveSpeedMultiplier;
                }
                else if (movement.verticalMovementInput < 0)                             // -- Movement: Backward
                {
                    float factor = movement.movementRunning ? movementConfig.runSpeedScale : movementConfig.walkSpeedScale;
                    newVelocity = direction * Mathf.Abs(movement.verticalMovementInput) * agent.speed * factor * movementConfig.backpedalSpeedScale * movementConfig.moveSpeedMultiplier;
                }
                else if (movement.horizontalMovementInput != 0) //STRAFE
                {
                    //NOTE: We do not want to factor run speed into strafing...we do not want both multipliers to make diagonal speed faster than forward speed.
                    //float factor = running ? config.runSpeedScale : config.walkSpeedScale; 
                    newVelocity = direction * agent.speed * movementConfig.strafeSpeedScale * movementConfig.moveSpeedMultiplier;
                }

                //STRAFE LEFT
                if (movement.movementStrafeLeft)
                {
                    if (!movement.movementStrafeRight) //NOTE: Holding both turn buttons cancels out turning
                    {
                        //direction = -agent.transform.right;
                        //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                        if (movementConfig.turnWhileStrafing) agent.transform.Rotate(0, -1.0f * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //TURN WHILE STRAFING
                        newVelocity = -agent.transform.right * 5f * agent.speed * movementConfig.strafeSpeedScale * movementConfig.moveSpeedMultiplier;
                    }
                }
                //STRAFE RIGHT
                else if (movement.movementStrafeRight)
                {
                    //direction = agent.transform.right;
                    //if (agent.velocity == Vector3.zero) agent.velocity = transform.forward.normalized; //FORWARD VELOCITY
                    if (movementConfig.turnWhileStrafing) agent.transform.Rotate(0, 1.0f * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //TURN WHILE STRAFING
                    //newVelocity = direction * horizontalMovementInput * agent.speed * config.strafeSpeedScale * config.moveSpeedMultiplier;
                    newVelocity = agent.transform.right * 5f * agent.speed * movementConfig.strafeSpeedScale * movementConfig.moveSpeedMultiplier;
                }

                if (!movement.movementStrafeLeft && !movement.movementStrafeRight)
                {
                    agent.transform.Rotate(0, movement.horizontalMovementInput * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //SET ROTATION - ON TRANSFORM - NOT WHILE STRAFING
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
