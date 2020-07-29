//by Fhiz
//MODIFIED BY DX4D
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;
using OpenMMO.Debugging;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO
{
    /// <summary> Holds a list of all prefabs that can be spawned on the network server. 
    /// The default spawnable prefabs list on NetworkManager is very weakly implemented, thats why we use our own. </summary>
    [CreateAssetMenu(fileName = "New SpawnablePrefabs", menuName = "OpenMMO - Templates/New SpawnablePrefabs", order = 999)]
    public partial class SpawnablePrefabsTemplate : ScriptableObject
    {
        [Header("PREFAB FILTERS")]

        [Tooltip("Only Prefabs with a SpawnableComponent attached to them will be included.")]
        public bool spawnComponentRequired = false;

        [Tooltip("Only Prefabs with one of these category tags will be included.")]
        public string[] requiredCategoryTags;
        [Tooltip("Prefabs with these category tags will not be included.")]
        public string[] excludedCategoryTags;

        [Tooltip("Prefabs in this list will not be included.")]
        public List<GameObject> excludedPrefabs;

        [Header("PREFAB FOLDERS")]
        [Tooltip("This is the root folder that the spawnable prefab folders are found in.")]
        public string assetBaseFolder = "/Assets/OpenMMO/";
        [Tooltip("When automatically adding spawnable prefabs to the network manager, these sub-directories of the asset base folder will be searched for spawnable prefabs.")]
        public string[] spawnablePrefabFolders;


        [Header("PREFABS (MANUAL)")]
        public List<GameObject> manualPrefabs;

        [Header("PREFABS (AUTOMATIC) - Add via Fetch Prefabs button")]
        [SerializeField, ReadOnly] public List<GameObject> autoPrefabs;

        /// <summary> Returns a list of registered prefabs during runtime. </summary>
        /// <remarks> The list has been built inside the editor already and is available during runtime. </remarks>
        public List<GameObject> GetRegisteredSpawnablePrefabs
        {
            get
            {

                List<GameObject> spawnPrefabs = new List<GameObject>();

                spawnPrefabs.AddRange(manualPrefabs);
                spawnPrefabs.AddRange(autoPrefabs);

                return spawnPrefabs;
            }
        }

#if UNITY_EDITOR

        /// <summary> Searches the "spawnablePrefabs" folder(s) inside the project and adds all matching prefabs to the spawnable prefabs list. </summary>
        /// <remarks> Only prefabs inside the "spawnablePrefabs" folder(s) are added and only if they have a NetworkIdentity on them. </remarks>
        public void AutoRegisterSpawnablePrefabs()
        {
            #region DEBUG - initialize debug log
#if UNITY_EDITOR && DEBUG
            System.Text.StringBuilder debugLog = new System.Text.StringBuilder(); //DEBUG LOG
            double startTime = NetworkTime.time; //START TIME
            uint counter = 0; //TICK COUNT
#endif
            #endregion //DEBUG

            List<string> prefabFolders = new List<string>();
            prefabFolders.Clear();

            #region DEBUG - folder title
#if UNITY_EDITOR && DEBUG
            debugLog.AppendLine("<b>" + "SEARCHED FOLDERS" + "</b>");
#endif
            #endregion //DEBUG
            foreach (string folder in spawnablePrefabFolders)
            {
                if (folder == string.Empty) continue;

                prefabFolders.Add((assetBaseFolder + folder)); //APPEND BASE ASSET FOLDER
                #region DEBUG - folder path
#if UNITY_EDITOR && DEBUG
                debugLog.AppendLine(" @" + assetBaseFolder + folder);
#endif
                #endregion //DEBUG
            }


            var guids = AssetDatabase.FindAssets("t:Prefab", prefabFolders.ToArray());
            List<GameObject> toSelect = new List<GameObject>();

            autoPrefabs = new List<GameObject>();
            autoPrefabs.Clear();

            #region DEBUG - reset tick counter
#if UNITY_EDITOR && DEBUG
            counter = 0; //RESET TICK COUNTER
#endif
            #endregion //DEBUG

            #region DEBUG - prefabs title
#if UNITY_EDITOR && DEBUG
            debugLog.AppendLine("<b>" + "FOUND PREFABS" + "</b>");
#endif
            #endregion //DEBUG
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                UnityEngine.Object[] toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
                foreach (UnityEngine.Object obj in toCheck)
                {
                    #region DEBUG - increment tick counter
#if UNITY_EDITOR && DEBUG
                    counter++; //TICK COUNTER
#endif
                    #endregion //DEBUG

                    var go = obj as GameObject;

                    if (go == null || excludedPrefabs.Contains(go)) continue; //NULL OR EXCLUDED PREFAB?


                    NetworkIdentity ni = go.GetComponent<NetworkIdentity>();

                    if (ni != null) //MUST HAVE A NETWORK ID
                    {

                        SpawnableComponent spawnable = go.GetComponent<SpawnableComponent>();
                        if (spawnComponentRequired && spawnable == null) continue; //MUST HAVE A SPAWNABLE COMPONENT?

                        //NO CATEGORY REQUIREMENTS?
                        if ((requiredCategoryTags == null || requiredCategoryTags.Length == 0) && (excludedCategoryTags == null || excludedCategoryTags.Length == 0))
                        {
                            toSelect.Add(go);
                            #region DEBUG - added without category reqs
#if UNITY_EDITOR && DEBUG
                            debugLog.AppendLine("    " + go.name);
#endif
                            #endregion //DEBUG
                        }
                        else if (Tools.ArrayContains(requiredCategoryTags, spawnable.category) //HAS A REQUIRED TAG
                            && !Tools.ArrayContains(excludedCategoryTags, spawnable.category)) //DOES NOT HAVE AN EXCLUDED TAG
                        {
                            toSelect.Add(go);
                            #region DEBUG - added by category
#if UNITY_EDITOR && DEBUG
                            debugLog.AppendLine("    " + go.name + " (" + spawnable.category + ")");
#endif
                            #endregion //DEBUG
                        }
                    }
                }
            }

            autoPrefabs.AddRange(toSelect.ToArray());

            #region DEBUG - print debug log
#if UNITY_EDITOR && DEBUG
            Debug.Log("<color=orange><b>[FETCH PREFAB REPORT]</b></color>"
                + " - Sifted through " + counter + " prefabs in " + (NetworkTime.time - startTime) + " seconds"
                + "\n" + "added " + toSelect.Count + " prefabs"
                + "\n" + debugLog.ToString());
#endif
            #endregion //DEBUG
            DebugManager.Log("[NetworkManager] Added [" + toSelect.Count + "] prefabs to spawnables prefabs list.");

        }
#endif
    }
}