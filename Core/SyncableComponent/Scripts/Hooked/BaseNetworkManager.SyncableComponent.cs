
using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.DebugManager;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// BaseNetworkManager
	// ===================================================================================
	public abstract partial class BaseNetworkManager
	{

		[Header("Spawnable Prefab Categories")]
		[Tooltip("Only prefabs of the listed categories are included")]
    	public string[] sortCategories;

#if UNITY_EDITOR
	
		// -------------------------------------------------------------------------------
		// AutoRegisterSpawnablePrefabs
		// @Editor
		// -------------------------------------------------------------------------------
		public void AutoRegisterSpawnablePrefabs()
		{

			var guids = AssetDatabase.FindAssets("t:Prefab", ProjectConfigTemplate.singleton.spawnablePrefabFolders);
			List<GameObject> toSelect = new List<GameObject>();
			
			spawnPrefabs.Clear();

			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				UnityEngine.Object[] toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
				
				foreach (UnityEngine.Object obj in toCheck)
				{
				
					var go = obj as GameObject;
					
					if (go == null)
						continue;
					
					NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
					
					if (ni != null && !ni.serverOnly)
					{

						EntityComponent entityComponent = go.GetComponent<EntityComponent>();

						if (entityComponent == null || sortCategories == null || sortCategories.Length == 0)
							toSelect.Add(go);
						else if (Tools.ArrayContains(sortCategories, entityComponent.archeType.sortCategory))
							toSelect.Add(go);

					}
					
				}
			}

			spawnPrefabs.AddRange(toSelect.ToArray());
			
			debug.Log("[NetworkManager] Added [" + toSelect.Count + "] prefabs to spawnables prefabs list.");

		}
		
#endif
			
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================