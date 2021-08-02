//BY DX4D
#define POSITION_VALIDATION //TOGGLE ON/OFF WITH //

using UnityEngine;

namespace OpenMMO.Network
{
    /// <summary>
	/// This partial section of the NetworkManager is responsible for player position validation.
	/// </summary>
    public partial class NetworkManager
    {
#if POSITION_VALIDATION
#if _SERVER
        [Hook(nameof(OnServerSceneChanged))] //ADDED - ENABLES POSITION VALIDATION - DX4D
#elif _CLIENT
        [Hook(nameof(OnClientSceneChanged))] //ADDED - ENABLES POSITION VALIDATION - DX4D
#endif
#endif
        public void ValidatePlayerPosition(GameObject player)
        {
            if (!ValidPosition(player.transform))
            {
                Debug.Log("[SERVER]" + " - "
                    + "Invalid Position Detected for character " + player.name );
                if (ServerAuthorityTemplate.singleton.smooth)
                {
                    //SMOOTH POSITION
                    Vector3 smoothedPosition = Vector3.Lerp(player.transform.position, transform.position, Time.deltaTime * ServerAuthorityTemplate.singleton.smoothing);

                    //WARP
                    player.GetComponent<PlayerAccount>().Warp(smoothedPosition);

                    Debug.LogWarning("TODO: SMOOTH MOVEMENT AUTHENTICATION IS IMPLEMENTED\nI have no idea what the smoothing factor should be...\nTESTERS: please test by building server + client in editor and try to change your player position...\nNOTE: You should not be out of sync with other players. TESTERS: verify this");
                }
                else
                {
                    player.GetComponent<PlayerAccount>().Warp(transform.position);
                }
            }
        }
        
		/// <summary>
		/// Used to validate the current position of the player (= character) according to the level set in ServerAuthorityTemplate.
		/// </summary>
        bool ValidPosition(Transform playerTransform)
        {
            switch (ServerAuthorityTemplate.singleton.validation)
            {
                case ValidationLevel.Complete:
                    {
                        return (transform == playerTransform);
                    }
                case ValidationLevel.Tolerant:
                    {
                        return (
                            //X
                            ( playerTransform.position.x >= transform.position.x - ServerAuthorityTemplate.singleton.tolerence
                                && playerTransform.position.x <= transform.position.x + ServerAuthorityTemplate.singleton.tolerence)
                            //Y
                            && (playerTransform.position.y >= transform.position.y - ServerAuthorityTemplate.singleton.tolerence
                                && transform.position.y <= playerTransform.position.y + ServerAuthorityTemplate.singleton.tolerence)
                            //Z
                            && (transform.position.z >= playerTransform.position.z - ServerAuthorityTemplate.singleton.tolerence
                                && transform.position.z <= playerTransform.position.z + ServerAuthorityTemplate.singleton.tolerence)
                           //NOTE: Rotation can be turned on here, but it barely matters for this purpose
                           // && (transform.rotation != playerTransform.rotation) //ROTATION - I don't think we care? Some games might, so just leave this here
                            );
                    }
                case ValidationLevel.Low:
                    {
                        return (transform.position != playerTransform.position);
                    }
                case ValidationLevel.None:
                    {
                        return true;
                    }
            }

            return false;
        }
	}
}