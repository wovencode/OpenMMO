
using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.DebugManager;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// BaseNetworkManager
	// ===================================================================================
	public abstract partial class BaseNetworkManager : Mirror.NetworkManager
	{

		[Header("Debug Helper")]
		public DebugHelper debug;
		
		// -------------------------------------------------------------------------------
		// Awake (Base)
		// -------------------------------------------------------------------------------
		public override void Awake()
		{
			debug = new DebugHelper();
			debug.Init();
			base.Awake(); // required
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================