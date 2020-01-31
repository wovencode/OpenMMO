// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
//
// =======================================================================================

using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using Wovencode;
using Wovencode.Network;

namespace Wovencode {
	
	// ===================================================================================
	// EntityComponent
	// ===================================================================================
	[System.Serializable]
	public abstract partial class EntityComponent : UpgradableComponent
	{
		
		
		[Header("Components")]
		public NavMeshAgent agent;
		public NetworkProximityChecker proxChecker;
		public new Collider collider;
		
		[Header("Default Data")]
		public ArchetypeTemplate archeType;
		
		
		
		// -------------------------------------------------------------------------------
		// 
		// -------------------------------------------------------------------------------
		protected override void Start()
    	{
        	base.Start();
		}
		
		// -------------------------------------------------------------------------------
		// OnStartLocalPlayer
		// -------------------------------------------------------------------------------
		public override void OnStartLocalPlayer()
    	{
    		agent = GetComponent<NavMeshAgent>();
		}
		
		// -------------------------------------------------------------------------------
		// UpdateServer
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer));
		}
		
		// -------------------------------------------------------------------------------
		// UpdateClient
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient));
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================