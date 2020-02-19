
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

namespace OpenMMO.Network
{
    // ===================================================================================
    // NetworkManager
    // ===================================================================================
    public partial class NetworkManager
    {
        [Header("SERVER AUTHORITY PROFILE")]
        [SerializeField] public ServerAuthority serverAuthority;

        protected List<GameObject> _playerPrefabs = null;

#if UNITY_EDITOR
        private new void OnValidate()
        {
            if (!serverAuthority) serverAuthority = Resources.Load<ServerAuthority>("Server/DefaultServerAuthority");
            base.OnValidate(); //TODO: Does this make it Validate twice? - TESTERS: place debug statements in the base method to find out...I expect it will not, but please verify
        }
#endif
        // -------------------------------------------------------------------------------
        // LoginPlayer_PlayerComponent
        // -------------------------------------------------------------------------------
        [DevExtMethods(nameof(LoginPlayer))]
        public void LoginPlayer_PlayerComponent(NetworkConnection conn, GameObject player, GameObject prefab, string userName, string playerName)
        {
            //Debug.LogWarning("TESTING: Bugfix: Changed " + prefab.name + " to " + player.name);
            //player.GetComponent<PlayerComponent>().tablePlayer.Update(player, userName);
            player.GetComponent<PlayerComponent>().tablePlayer.Update(prefab, userName); //TODO?
        }
        
        // -------------------------------------------------------------------------------
        // RegisterPlayer_PlayerComponent
        // -------------------------------------------------------------------------------
        [DevExtMethods(nameof(RegisterPlayer))]
        public void RegisterPlayer_PlayerComponent(GameObject player, string userName, string prefabName)
        {
            player.transform.position = GetStartPosition(player).position;
            player.GetComponent<PlayerComponent>().tablePlayer.Create(player, userName, prefabName);
        }

        // ================================== PUBLIC =====================================

        // -------------------------------------------------------------------------------
        // playerPrefabs
        // -------------------------------------------------------------------------------
        public List<GameObject> playerPrefabs
        {
            get
            {
                if (_playerPrefabs == null)
                    FilterPlayerPrefabs();
                return _playerPrefabs;
            }
        }

        // ================================== PROTECTED ==================================

        // -------------------------------------------------------------------------------
        // GetStartPosition
        // -------------------------------------------------------------------------------
        protected Transform GetStartPosition(GameObject player)
        {

            //TODO: Add start position randomization here

            foreach (Transform startPosition in startPositions)
            {

                OpenMMO.Network.NetworkStartPosition position = startPosition.GetComponent<OpenMMO.Network.NetworkStartPosition>();

                if (position == null || position.archeTypes.Length == 0)
                    continue;

                PlayerComponent playerComponent = player.GetComponent<PlayerComponent>();

                foreach (ArchetypeTemplate template in position.archeTypes)
                    if (template == playerComponent.archeType)
                        return position.transform;

            }

            return GetStartPosition();

        }
        
        // -------------------------------------------------------------------------------
        // ValidatePlayerPosition
        // -------------------------------------------------------------------------------
        protected void ValidatePlayerPosition(GameObject player)
        {
            Transform transform = player.transform;

            if (!NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
                transform = GetStartPosition(player);


            if (!ValidPosition(player.transform))
            {
                if (serverAuthority.smooth)
                {
                    //SMOOTH POSITION
                    Vector3 smoothedPosition = Vector3.Lerp(player.transform.position, transform.position, Time.deltaTime * serverAuthority.smoothing);

                    //WARP
                    player.GetComponent<PlayerComponent>().Warp(smoothedPosition);

                    Debug.LogWarning("TODO: SMOOTH MOVEMENT AUTHENTICATION IS IMPLEMENTED\nI have no idea what the smoothing factor should be...\nTESTERS: please test by building server + client in editor and try to change your player position...\nNOTE: You should not be out of sync with other players. TESTERS: verify this");
                }
                else
                {
                    player.GetComponent<PlayerComponent>().Warp(transform.position);
                }
            }

        }

        //[Server]
        bool ValidPosition(Transform playerTransform)
        {
            switch (serverAuthority.validation)
            {
                case ValidationLevel.Complete:
                    {
                        return (transform == playerTransform);
                    }
                case ValidationLevel.Tolerant:
                    {
                        return (
                            //X
                            ( playerTransform.position.x >= transform.position.x - serverAuthority.tolerence
                                && playerTransform.position.x <= transform.position.x + serverAuthority.tolerence)
                            //Y
                            && (playerTransform.position.y >= transform.position.y - serverAuthority.tolerence
                                && transform.position.y <= playerTransform.position.y + serverAuthority.tolerence)
                            //Z
                            && (transform.position.z >= playerTransform.position.z - serverAuthority.tolerence
                                && transform.position.z <= playerTransform.position.z + serverAuthority.tolerence)
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
		
		// -------------------------------------------------------------------------------
		// GetPlayerPrefab
		// -------------------------------------------------------------------------------
		protected override GameObject GetPlayerPrefab(string prefabName)
		{
			
			GameObject prefab = playerPrefabs.Find(p => p.name == prefabName);

			if (prefab == null)
				return playerPrefab;
			
			return prefab;
			
		}
		
		// -------------------------------------------------------------------------------
		// FilterPlayerPrefabs
		// -------------------------------------------------------------------------------
		protected void FilterPlayerPrefabs()
    	{
       		
       		_playerPrefabs = new List<GameObject>();
        	
        	foreach (GameObject prefab in spawnPrefabs)
        	{

        		if (prefab == null) continue;
        		
            	PlayerComponent player = prefab.GetComponent<PlayerComponent>();
            	if (player != null)
               		_playerPrefabs.Add(prefab);
               		
        	}
        	
    	}
    	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================