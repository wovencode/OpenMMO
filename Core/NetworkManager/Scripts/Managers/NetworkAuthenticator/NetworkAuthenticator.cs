
using Wovencode;
using Wovencode.Network;
using Wovencode.UI;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace Wovencode.Network
{

    // ===================================================================================
	// NetworkAuthenticator
	// ===================================================================================
	[RequireComponent(typeof(Wovencode.Network.NetworkManager))]
    [DisallowMultipleComponent]
    public partial class NetworkAuthenticator : BaseNetworkAuthenticator
    {

		[Header("System Texts")]
		public NetworkAuthenticator_Lang 					systemText;
		
		public static Wovencode.Network.NetworkAuthenticator singleton;
		
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