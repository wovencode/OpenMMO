using UnityEngine;
using UnityEngine.Events;
using OpenMMO.Network;
using OpenMMO;
using System;

namespace OpenMMO.Network
{
	
	// -----------------------------------------------------------------------------------
	// NetworkManager_Events
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class NetworkManager_Events
	{
		
		public UnityEvent OnStartServer;
		public UnityEvent OnStartClient;
		
		
	}
	
}

// =======================================================================================