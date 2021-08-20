//BY DX4D
#define POSITION_VALIDATION //TOGGLE ON/OFF WITH //

using Mirror;
using UnityEngine;
using System.Collections.Generic;

namespace OpenMMO.Network
{
    /// <summary>
	/// This partial section of the NetworkManager is responsible for player position validation.
	/// </summary>
    [RequireComponent(typeof(PlayerAccount))]
    public partial class PlayerPositionCorrector : SyncableComponent
    {
        [Header("CLIENT POSITION CORRECTION")]
        [Tooltip("How many update frames between each execution of this method. ex: 1 = every frame")]
        [SerializeField] [Range(ushort.MinValue, ushort.MaxValue)] ushort updateFrequency = 1;
        ushort tickCount = 0;


        [Client] protected override void StartClient() { }
        [Client] protected override void UpdateClient() { }
        [Client] protected override void FixedUpdateClient() { }
        [Client] protected override void LateUpdateClient() { }

        [Server] protected override void StartServer() { }
        [Server] protected override void UpdateServer() { CorrectPlayerPositions(); }

        //#if _SERVER
        //[Hook(nameof(OnServerSceneChanged))] //ADDED - ENABLES POSITION VALIDATION - DX4D
        //#elif _CLIENT
        //[Hook(nameof(OnClientSceneChanged))] //ADDED - ENABLES POSITION VALIDATION - DX4D
        //#endif
#if _SERVER && POSITION_VALIDATION
        //private void FixedUpdate() { CorrectPlayerPositions(); }
//#elif _CLIENT
        //private void FixedUpdate() { CorrectPlayerPositions(); }
        //private void FixedUpdate() { PredictPlayerPositions(); }
#endif
        //public void CorrectPlayerPosition(GameObject player) //REMOVED DX4D
        public void CorrectPlayerPositions()//Mirror.NetworkConnection conn) //ADDED DX4D
        {
            if (++tickCount < updateFrequency) return; //INCREMENT AND CHECK TICK COUNTER
            tickCount = 0; //RESET TICK COUNTER

            //GameObject player;// = Mirror.NetworkClient.localPlayer.gameObject;

            if (NetworkManager.onlinePlayers != null)
            {
                GameObject player = null; //INITIALIZE PLAYER

                foreach (KeyValuePair<string, GameObject> _player in NetworkManager.onlinePlayers)
                {
                    if (_player.Value != null)
                    {
                        player = _player.Value; //SET PLAYER

                        Debug.Log("[SERVER]" + " - "
                            + "Validating character " + player.name + "'s position...");

                        if (!ValidPosition(player.transform.position))
                        {
                            Debug.Log("[SERVER]" + " - "
                                + "Invalid Position Detected for character " + player.name + " - Correcting Position to " + transform.position + "...");

                            //DISTANCE APART IS OUTSIDE OF THRESHOLD
                            float distanceToDestination = Vector3.Distance(transform.position, player.transform.position);
                            if (distanceToDestination > ServerAuthorityTemplate.singleton.tolerence)
                            {
                                GetComponent<PlayerAccount>().Warp(transform.position);
                                Debug.Log("[SERVER]" + " - "
                                    + player.name + " was too far away from the expected distance threshold...position has been corrected without smoothing!");
                            }
                            else if (ServerAuthorityTemplate.singleton.smooth) //SMOOTH CORRECTION
                            {
                                //SMOOTH POSITION
                                Vector3 smoothedPosition = Vector3.Lerp(player.transform.position, transform.position, Time.deltaTime * ServerAuthorityTemplate.singleton.smoothing);

                                //WARP
                                player.GetComponent<PlayerAccount>().Warp(smoothedPosition);
                                Debug.Log("[SERVER]" + " - "
                                    + player.name + "'s position has been corrected with smoothing!");

                                Debug.LogWarning("TODO: SMOOTH MOVEMENT AUTHENTICATION IS IMPLEMENTED\nI have no idea what the smoothing factor should be...\nTESTERS: please test by building server + client in editor and try to change your player position...\nNOTE: You should not be out of sync with other players. TESTERS: verify this");
                            }
                            else //INSTANT CORRECTION
                            {
                                player.GetComponent<PlayerAccount>().Warp(transform.position);
                                Debug.Log("[SERVER]" + " - "
                                    + player.name + "'s position has been corrected without smoothing!");
                            }
                        }
                        else
                        {
                            Debug.Log("[SERVER]" + " - "
                                + player.name + "'s position is valid!");
                        }

                        //player = null; //RESET PLAYER //NOTE: Unnecessary, but leaving it here in case we need it later.
                    }
                }
            }
        }
        /// <summary>
        /// Used to validate the current position of the player (= character) according to the level set in ServerAuthorityTemplate.
        /// </summary>
        bool ValidPosition(Vector3 playerPosition)
        {
            switch (ServerAuthorityTemplate.singleton.validation)
            {
                case ValidationLevel.Complete:
                    {
                        return (transform.position == playerPosition);
                    }
                case ValidationLevel.Tolerant:
                    {
                        //TOLERENCE
                        float tolerence = ServerAuthorityTemplate.singleton.tolerence;

                        //X
                        float expectedX = transform.position.x;
                        float clientX = playerPosition.x;
                        float minToleratedX = clientX - tolerence;
                        float maxToleratedX = clientX + tolerence;
                        //Y
                        float expectedY = transform.position.y;
                        float clientY = playerPosition.y;
                        float minToleratedY = clientY - tolerence;
                        float maxToleratedY = clientY + tolerence;
                        //Z
                        float expectedZ = transform.position.z;
                        float clientZ = playerPosition.z;
                        float minToleratedZ = clientZ - tolerence;
                        float maxToleratedZ = clientZ + tolerence;

                        return (
                            //X
                            (expectedX >= minToleratedX
                                && expectedX <= maxToleratedX)
                            //Y
                            && (expectedY >= minToleratedY
                                && expectedY <= maxToleratedY)
                            //Z
                            && (expectedZ >= minToleratedZ
                                && expectedZ <= maxToleratedZ)
                            //NOTE: Rotation can be turned on here, but it barely matters for this purpose
                             //&& (transform.rotation != playerTransform.rotation) //ROTATION - I don't think we care? Some games might, so just leave this here
                            );
                    }
                case ValidationLevel.Low:
                    {
                        return (transform.position != playerPosition);
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
