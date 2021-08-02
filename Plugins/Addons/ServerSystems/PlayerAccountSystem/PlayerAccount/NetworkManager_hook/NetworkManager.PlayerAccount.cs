//by  Fhiz
using System;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.Zones;
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
        }
        
       	/// <summary>
		/// Hooks into RegisterPlayer and adds player (= character) related core data to it. This includes the TablePlayer as well as the start position (that is used for Host+Play only).
		/// </summary>
        [DevExtMethods(nameof(RegisterPlayer))]
        public void RegisterPlayer_PlayerComponent(GameObject player, string userName, string prefabName)
        {
            // --  setup local zone position, required for host+play
            string anchorName = AnchorManager.singleton.GetArchetypeStartPositionAnchorName(player);
            player.transform.position = AnchorManager.singleton.GetStartAnchorPosition(anchorName); 
            
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