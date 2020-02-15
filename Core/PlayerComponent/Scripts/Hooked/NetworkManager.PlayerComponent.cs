
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

        protected List<GameObject> _playerPrefabs = null;

        // -------------------------------------------------------------------------------
        // LoginPlayer_PlayerComponent
        // -------------------------------------------------------------------------------
        [DevExtMethods("LoginPlayer")]
        public void LoginPlayer_PlayerComponent(NetworkConnection conn, GameObject player, GameObject prefab, string userName, string playerName)
        {
            player.GetComponent<PlayerComponent>().tablePlayer.Update(prefab, userName);
        }
        
        // -------------------------------------------------------------------------------
        // RegisterPlayer_PlayerComponent
        // -------------------------------------------------------------------------------
        [DevExtMethods("RegisterPlayer")]
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
        public enum ValidationLevel { Complete, Tolerant, Low, None }
        // -------------------------------------------------------------------------------
        // ValidatePlayerPosition
        // -------------------------------------------------------------------------------
        [Tooltip("When using Tolerant validation, a value will be tolerated if it is out of range by up to this amount in each direction.")]
        [SerializeField] float tolerence = 7f;
        [Tooltip("Smoothly Move back to the server dictated position.")]
        [SerializeField] bool smooth = true;
        [Tooltip("The level of validation to player movement desired." +
            "\nComplete: Validates the entire transform - Warps the player instantly back in position if they stray."
            + "Tolerant: Validates (only) positon with tolerance factored in - can return the player to the destired postion smoothly. - preferred"
            + "Low: Just validates position and nothing else.")]
        [SerializeField] ValidationLevel validation = ValidationLevel.Complete;
        protected void ValidatePlayerPosition(GameObject player)
        {
            Transform transform = player.transform;

            if (!NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
                transform = GetStartPosition(player);


            if (!ValidPosition(player.transform))
            {
                if (smooth)
                {
                    //TODO TODO TODO
                    //Ease Position
                    player.GetComponent<PlayerComponent>().Warp(transform.position);

                    Debug.LogWarning("TODO: SMOOTH MOVE IS NOT IMPLEMENTED");
                }
                else
                {
                    player.GetComponent<PlayerComponent>().Warp(transform.position);
                }
            }

        }

        bool ValidPosition(Transform playerTransform)
        {
            switch (validation)
            {
                case ValidationLevel.Complete:
                    {
                        return (transform == playerTransform);
                    }
                case ValidationLevel.Tolerant:
                    {
                        return (
                            //X
                            (transform.position.x >= playerTransform.position.x - tolerence
                                && transform.position.x <= playerTransform.position.x + tolerence)
                            //Y
                            && (transform.position.y >= playerTransform.position.y - tolerence
                                && transform.position.y <= playerTransform.position.y + tolerence)
                            //Z
                            && (transform.position.z >= playerTransform.position.z - tolerence
                                && transform.position.z <= playerTransform.position.z + tolerence)
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