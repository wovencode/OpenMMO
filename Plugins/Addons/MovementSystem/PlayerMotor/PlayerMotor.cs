//BY DX4D
using UnityEngine;
using UnityEngine.AI;

namespace OpenMMO
{
    /// <summary>
    /// A Motor is responsible for updates to the velocity of a NavMesh agent.
    /// </summary>
    public abstract class CharacterMotor : ScriptableObject
    {
        internal abstract Vector3 GetVelocity(MovementStateInfo movement, PlayerControlConfig movementConfig, NavMeshAgent agent);
    }
}
