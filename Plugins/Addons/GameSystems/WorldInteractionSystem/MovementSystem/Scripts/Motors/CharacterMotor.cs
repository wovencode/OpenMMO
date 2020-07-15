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
        internal abstract Vector3 GetVelocity(MovementInput movement, MovementModifiers movementConfig, NavMeshAgent agent);
    }
}
