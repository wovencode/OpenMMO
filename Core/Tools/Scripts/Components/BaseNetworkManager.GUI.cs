
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
			DrawDefaultInspector();

			SpawnablePrefabsTemplate template = (SpawnablePrefabsTemplate)target;
			
			if (GUILayout.Button("Search & add Prefabs"))
			{
				template.AutoRegisterSpawnablePrefabs();
			}
		}
		
		// -------------------------------------------------------------------------------
		
	}

#endif

}

// =======================================================================================