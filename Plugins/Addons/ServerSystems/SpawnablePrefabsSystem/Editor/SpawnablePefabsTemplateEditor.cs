
using UnityEngine;
using System.Collections;
using UnityEditor;
using OpenMMO;

namespace OpenMMO
{

#if UNITY_EDITOR

	// ===================================================================================
	// SpawnablePefabsTemplateEditor
	// @Editor
	// ===================================================================================
	[CustomEditor(typeof(SpawnablePrefabsTemplate))]
	public class SpawnablePefabsTemplateEditor : Editor
	{

        // -------------------------------------------------------------------------------
        // OnInspectorGUI
        // -------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {

            SpawnablePrefabsTemplate template = (SpawnablePrefabsTemplate)target;

            GUILayout.BeginVertical();
            {
                GUILayout.BeginVertical("box");
                {
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("FETCH PREFABS", GUILayout.Height(35)))//, GUILayout.Width(100)))
                    {
                        template.AutoRegisterSpawnablePrefabs();
                    }
                    GUI.backgroundColor = Color.white;
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("box");
                {
                    DrawDefaultInspector();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }
		
		// -------------------------------------------------------------------------------
		
	}

#endif

}

// =======================================================================================