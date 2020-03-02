
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.Zones;
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
    
        protected List<GameObject> _playerPrefabs = null;

        // -------------------------------------------------------------------------------
        // LoginPlayer_PlayerComponent
        // -------------------------------------------------------------------------------
        [DevExtMethods(nameof(LoginPlayer))]
        public void LoginPlayer_PlayerComponent(NetworkConnection conn, GameObject player, string playerName, string userName)
        {
            player.GetComponent<PlayerComponent>().tablePlayer.Update(player, userName);
        }
        
        // -------------------------------------------------------------------------------
        // RegisterPlayer_PlayerComponent
        // -------------------------------------------------------------------------------
        [DevExtMethods(nameof(RegisterPlayer))]
        public void RegisterPlayer_PlayerComponent(GameObject player, string userName, string prefabName)
        {
            // --  setup local zone position, required for host+play
            string anchorName = AnchorManager.singleton.GetArchetypeStartPositionAnchorName(player);
            player.transform.position = AnchorManager.singleton.GetStartAnchorPosition(anchorName); 
            
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
        // ValidatePlayerPosition
        // -------------------------------------------------------------------------------
        public void ValidatePlayerPosition(GameObject player)
        {
            Transform transform = player.transform;

            if (!NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
            {
				debug.Log("Last saved position invalid, reverting to start position for: "+player.name);
				player.GetComponent<PlayerComponent>().WarpLocal(AnchorManager.singleton.GetArchetypeStartPositionAnchorName(player));
           	}
           
            if (!ValidPosition(player.transform))
            {
                if (ServerAuthorityTemplate.singleton.smooth)
                {
                    //SMOOTH POSITION
                    Vector3 smoothedPosition = Vector3.Lerp(player.transform.position, transform.position, Time.deltaTime * ServerAuthorityTemplate.singleton.smoothing);

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
        
		// -------------------------------------------------------------------------------
		// ValidPosition
		// -------------------------------------------------------------------------------
        //[Server],0
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