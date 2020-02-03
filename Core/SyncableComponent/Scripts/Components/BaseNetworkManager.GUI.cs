
using UnityEngine;
using System.Collections;
using UnityEditor;
using OpenMMO;
using OpenMMO.Network;

namespace OpenMMO.Network
{

#if UNITY_EDITOR

	// ===================================================================================
	// NetworkManagerEditor
	// @Editor
	// ===================================================================================
	[CustomEditor(typeof(NetworkManager))]
	public class NetworkManagerEditor : Editor
	{
	
		// -------------------------------------------------------------------------------
		// OnInspectorGUI
		// -------------------------------------------------------------------------------
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			BaseNetworkManager networkManager = (BaseNetworkManager)target;
			
			if (GUILayout.Button("Search & add Prefabs"))
			{
				networkManager.AutoRegisterSpawnablePrefabs();
			}
		}
		
		// -------------------------------------------------------------------------------
		
	}

#endif

}

// =======================================================================================