
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
	[RequireComponent(typeof(OpenMMO.Network.NetworkManager))]
    [DisallowMultipleComponent]
    public partial class NetworkAuthenticator : BaseNetworkAuthenticator
    {

		[Header("System Texts")]
		public NetworkAuthenticator_Lang 					systemText;
		
		public static OpenMMO.Network.NetworkAuthenticator singleton;
		
		// -------------------------------------------------------------------------------
		public override void Awake()
		{
			base.Awake(); // required
		
			singleton = this;

			connectDelay = UnityEngine.Random.Range(connectDelayMin,connectDelayMax);
    		
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================