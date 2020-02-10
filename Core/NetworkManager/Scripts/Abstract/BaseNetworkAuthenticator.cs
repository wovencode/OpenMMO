
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.DebugManager;
using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// BaseNetworkAuthenticator
	// ===================================================================================
    /// <summary>
    /// Abstract <c>Network Authenticator Base Class</c> that inherits the Mirror network authenticator.
    /// Partial class as to protect the various functions and methods. 
    /// </summary>
	public abstract partial class BaseNetworkAuthenticator : Mirror.NetworkAuthenticator
	{
		
		[Header("Debug Helper")]
		public DebugHelper debug;
		
		// -------------------------------------------------------------------------------
		// Awake (Base)
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Base <c> Awake </c> function for the network authentication components
        /// </summary>
        public virtual void Awake()
		{
			debug = new DebugHelper();
			debug.Init();
		}
		
	}

}

// =======================================================================================