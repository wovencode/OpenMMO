//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;

namespace OpenMMO.Network
{

	// ===================================================================================
	// BaseNetworkManager
	// ===================================================================================
	public abstract partial class BaseNetworkManager
	{

		// ====================== PUBLIC METHODS - GENERAL ===============================
        /// <summary>
        /// Public abstract general Method <c>GetPlayerPrefab</c> that can be overwritten to load the player prefab using the player prefab name
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns> The player prefab gameobject </returns>
		protected abstract GameObject GetPlayerPrefab(string playerName);
	}

}
