using UnityEngine;
using UnityEngine.Events;
using Wovencode.Network;
using Wovencode;
using System;

namespace Wovencode.Network
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