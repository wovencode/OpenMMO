//BY DX4D
using UnityEngine;
using UnityEngine.AI;

namespace OpenMMO
{
    [CreateAssetMenu(menuName = "OpenMMO/Controls/Camera Centric Character Motor")]
    public partial class CameraCentricCharacterMotor : CharacterMotor
    {
        public bool lookInMoveDirection = true;

        internal override Vector3 GetVelocity(MovementInput movement, MovementModifiers movementConfig, NavMeshAgent agent)
        {
            return CalculateVelocity(movement, movementConfig, agent);
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
                /* //DEPRECIATED
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
                */

                Vector3 verticalDirection = Vector3.zero;
                Vector3 horizontalDirection = Vector3.zero;
                Vector3 newVelocity = Vector3.zero;

                //SPEED FACTOR
                float factor = movement.running ? movementConfig.runSpeedScale : movementConfig.walkSpeedScale; //running
                factor *= movement.sneaking ? movementConfig.sneakSpeedScale : 1f; //sneaking

                if (movement.verticalInput != 0)
                {
                    if (movement.verticalInput > 0 && Camera.main) { verticalDirection = Camera.main.transform.forward; }
                    else if (movement.verticalInput < 0 && Camera.main) { verticalDirection = -Camera.main.transform.forward; }
                    //newVelocity = verticalDirection * Mathf.Abs(movement.verticalInput);// * agent.speed * factor * movementConfig.moveSpeedMultiplier;
                    //if (Camera.main) direction = rotation * Camera.main.transform.forward;
                    //if (Camera.main) direction = rotation * -Camera.main.transform.forward;
                    //newVelocity = direction * Mathf.Abs(movement.verticalInput) * agent.speed * factor * movementConfig.moveSpeedMultiplier; //* movementConfig.backpedalSpeedScale
                }

                if (movement.horizontalInput != 0) //STRAFE
                {
                    if (movement.horizontalInput > 0 && Camera.main) { horizontalDirection = Camera.main.transform.right; }
                    if (movement.horizontalInput < 0 && Camera.main) { horizontalDirection = -Camera.main.transform.right; }
                    //newVelocity = horizontalDirection * Mathf.Abs(movement.horizontalInput);// * agent.speed * factor * movementConfig.moveSpeedMultiplier;
                    //if (movement.horizontalInput > 0 && Camera.main) direction = rotation * -Camera.main.transform.right;
                    //if (movement.horizontalInput < 0 && Camera.main) direction = rotation * Camera.main.transform.right;
                    //NOTE: We do not want to factor run speed into strafing...we do not want both multipliers to make diagonal speed faster than forward speed.
                    //float factor = running ? config.runSpeedScale : config.walkSpeedScale; 
                }

                newVelocity = (horizontalDirection * Mathf.Abs(movement.horizontalInput)) + (verticalDirection * Mathf.Abs(movement.verticalInput));
                newVelocity *= agent.speed * factor * movementConfig.moveSpeedMultiplier; //FACTOR IN SPEED

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
                    //agent.transform.Rotate(0, movement.horizontalInput * movementConfig.turnSpeedMultiplier * Time.deltaTime * 100f, 0); //SET ROTATION - ON TRANSFORM - NOT WHILE STRAFING
                }

                if (lookInMoveDirection && (horizontalDirection + verticalDirection) != Vector3.zero)
                {
                    agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(horizontalDirection + verticalDirection), agent.angularSpeed * factor);
                }

                //if () agent.gameObject.transform.LookAt(agent.gameObject.transform.position + direction, Vector3.up); //LOOK AT MOVE DIRECTION

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
