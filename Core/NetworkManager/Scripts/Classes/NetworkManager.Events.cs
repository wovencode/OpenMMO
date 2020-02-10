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
    /// <summary>
    /// Public partial class <c>NetworkManager_Events</c> that contains the network manager's events;
    /// </summary>
    [System.Serializable]
	public partial class NetworkManager_Events
	{
		/// <summary>
        /// Event called on server start
        /// </summary>
		public UnityEvent OnStartServer;
        /// <summary>
        /// Event called on client start
        /// </summary>
		public UnityEvent OnStartClient;
        /// <summary>
        /// Event called when a player succesfully logs in
        /// </summary>
		public UnityEventConnection OnLoginPlayer;
        /// <summary>
        /// Event called when a player succesfully logs out
        /// </summary>
		public UnityEventConnection OnLogoutPlayer;
		
	}
	
}

// =======================================================================================