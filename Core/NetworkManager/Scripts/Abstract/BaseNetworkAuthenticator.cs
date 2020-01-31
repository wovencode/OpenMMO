
using Wovencode;
using Wovencode.Network;
using Wovencode.DebugManager;
using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;

namespace Wovencode.Network
{
	
	// ===================================================================================
	// BaseNetworkAuthenticator
	// ===================================================================================
	public abstract partial class BaseNetworkAuthenticator : Mirror.NetworkAuthenticator
	{
		
		[Header("Debug Helper")]
		public DebugHelper debug;
		
		// -------------------------------------------------------------------------------
		// Awake (Base)
		// -------------------------------------------------------------------------------
		public virtual void Awake()
		{
			debug = new DebugHelper();
			debug.Init();
		}
		
	}

}

// =======================================================================================