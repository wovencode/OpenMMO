
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkAuthenticator
	// ===================================================================================
    /// <summary>
    /// Public Partial Class <c>NetworkAuthenticator</c> inherits from <c>BaseNetworkAuthenticator</c>.
    /// Requires the <c>NetworkManager</c> component. 
    /// Contains all authentication events, methods and functions.
    /// </summary>
	[RequireComponent(typeof(OpenMMO.Network.NetworkManager))]
    [DisallowMultipleComponent]
    public partial class NetworkAuthenticator : BaseNetworkAuthenticator
    {

		[Header("System Texts")]
		public NetworkAuthenticator_Lang 					systemText;
		
		public static OpenMMO.Network.NetworkAuthenticator singleton;
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Unity Method.
        /// Called before Start.
        /// Sets the connection delay and creates a singleton.
        /// </summary>
		public override void Awake()
		{
			base.Awake(); // required
		
			singleton = this;

			connectDelay = UnityEngine.Random.Range(connectDelayMin,connectDelayMax); // randomize connection delay
    		
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================