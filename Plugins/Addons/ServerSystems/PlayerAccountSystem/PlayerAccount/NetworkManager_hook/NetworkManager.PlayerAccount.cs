//by  Fhiz
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
    /// <summary>
	/// This partial section of the NetworkManager is responsible for player (= character) related actions.
	/// </summary>
    public partial class NetworkManager
    {
    
        protected List<GameObject> _playerPrefabs = null;

        /// <summary>
		/// Hooks into LoginPlayer and updates the core data in TablePlayer.
		/// </summary>
<<<<<<< HEAD
        [DevExtMethods(nameof(DatabaseManager.LoginPlayer))]
        public void LoginPlayer_PlayerComponent(NetworkConnection conn, GameObject player, string playerName, string userName)
        //[DevExtMethods(nameof(PlayerCharacterLogin))] //REMOVED - DX4D
        //public void LoginPlayer_PlayerComponent(NetworkConnection conn, string playerName, string userName, int token)
        {
            //string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playerName);
            //GameObject prefab = GetPlayerPrefab(prefabname);
            //GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playerName);

            player.GetComponent<PlayerAccount>()._tablePlayer.Update(player, userName);
=======
        [DevExtMethods(nameof(LoginPlayer))]
        //[DevExtMethods(nameof(DatabaseManager.LoadPlayerFromDatabase))] //REMOVED - DX4D
        //public void LoadPlayerFromDatabase_PlayerComponent(NetworkConnection conn, GameObject player, string playerName, string userName)
        public void LoadPlayerFromDatabase_PlayerComponent(NetworkConnection conn, string playername, string userName, int token)
        {
            //GET PLAYER ACCOUNT FROM PLAYER NAME
            string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playername);
            GameObject prefab = GetPlayerPrefab(prefabname);
            GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playername);
            if (!player) { Debug.Log("NETWORK ISSUE: Player not found in database..."); }
            PlayerAccount pc = player.GetComponent<PlayerAccount>();
            if (!pc) { Debug.Log("NETWORK ISSUE: Player must have a PlayerAccount component..."); }

            pc._tablePlayer.Update(player, userName);
>>>>>>> master
        }
        
       	/// <summary>
		/// Hooks into RegisterPlayer and adds player (= character) related core data to it. This includes the TablePlayer as well as the start position (that is used for Host+Play only).
		/// </summary>
        [DevExtMethods(nameof(RegisterPlayer))]
        public void RegisterPlayer_PlayerComponent(GameObject player, string userName, string prefabName)
        {
            // --  setup local zone position, required for host+play
            string anchorName = LocationMarkerManager.singleton.GetArchetypeStartPositionAnchorName(player);
            player.transform.position = LocationMarkerManager.singleton.GetStartAnchorPosition(anchorName); 
            
            player.GetComponent<PlayerAccount>()._tablePlayer.Create(player, userName, prefabName);
        }

       	/// <summary>
		/// Returns the player (= character) prefabs from cache. If no cache available, it filters and caches them first.
		/// </summary>
        public List<GameObject> playerPrefabs
        {
            get
            {
                if (_playerPrefabs == null)
                    FilterPlayerPrefabs();
                return _playerPrefabs;
            }
        }

        /// <summary>
		/// Validates the player (= character) position on login and teleport. Checks if the target destination is on the NavMesh and teleports to the closest start position if it is not.
		/// </summary>
        public void ValidatePlayerPosition(GameObject player)
        {
            Transform transform = player.transform;

            if (!NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
            {
				debug.Log("Last saved position invalid, reverting to start position for: "+player.name);
				player.GetComponent<PlayerAccount>().WarpLocal(LocationMarkerManager.singleton.GetArchetypeStartPositionAnchorName(player)); //TODO: Fix this - it should have a null check for the GetComponent call before invoking the method
           	}
           
            if (!ValidPosition(player.transform))
            {
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
		
		/// <summary>
		/// Returns the player (= character) prefab that matches the provided prefab name.
		/// </summary>
		protected override GameObject GetPlayerPrefab(string prefabName)
		{
			
			GameObject prefab = playerPrefabs.Find(p => p.name == prefabName);

			if (prefab == null)
				return playerPrefab;
			
			return prefab;
			
		}
		
		/// <summary>
		/// Filters all network identities contained in the 'spawnPrefabs' list of NetworkManager and only returns player (= character) prefabs.
		/// </summary>
		protected void FilterPlayerPrefabs()
    	{
       		
       		_playerPrefabs = new List<GameObject>();
        	
        	foreach (GameObject prefab in spawnPrefabs)
        	{

        		if (prefab == null) continue;
        		
            	PlayerAccount player = prefab.GetComponent<PlayerAccount>();
            	if (player != null)
               		_playerPrefabs.Add(prefab);
               		
        	}
        	
    	}
    	
	}

}