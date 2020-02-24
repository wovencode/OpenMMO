using UnityEngine;
using UnityEngine.Events;
using OpenMMO.Network;
using OpenMMO;
using System;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkManager_Events
	// ===================================================================================
    /// <summary>
    /// Public partial class <c>NetworkManager_Events</c> that contains the network manager's events;
    /// </summary>
    [System.Serializable]
	public partial class NetworkManager_Events
	{
	
		public UnityEvent OnStartServer;
		public UnityEvent OnStopServer;
		public UnityEvent OnStartClient;
		public UnityEvent OnStopClient;
		public UnityEventConnection OnLoginPlayer;
		public UnityEventConnection OnLogoutPlayer;
		
	}
	
}

// =======================================================================================