//BY DX4D

using UnityEngine;
using UnityEngine.AI;
using OpenMMO.Zones;

namespace OpenMMO.Network
{
    /// <summary>
	/// This partial section of the NetworkManager is responsible for player position validation.
	/// </summary>
    public partial class NetworkManager
    {
        /// <summary>
		/// Validates the player (= character) position on login and teleport. Checks if the target destination is on the NavMesh and teleports to the closest start position if it is not.
		/// </summary>
        public void ValidateWarpLocation(GameObject player)
        {
            Transform transform = player.transform;

            if (!NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
            {
                Debug.LogWarning("Last saved position invalid, reverting to start position for: " + player.name);
                player.GetComponent<PlayerAccount>().WarpLocal(AnchorManager.singleton.GetArchetypeStartPositionAnchorName(player));
            }
        }
	}
}