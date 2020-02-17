
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using OpenMMO;
using OpenMMO.Network;

namespace OpenMMO {
	
	// ===================================================================================
	// EntityComponent
	// ===================================================================================
	[System.Serializable]
	public abstract partial class EntityComponent : LevelableComponent
	{
		
		[Header("Components")]
		public NavMeshAgent agent;
		public NetworkProximityChecker proxChecker;
#pragma warning disable CS0109
		public new Collider collider;
#pragma warning restore CS0109
		
		[Header("Default Data")]
		public ArchetypeTemplate archeType;
		
		// -- Component Cache
		[HideInInspector]public EntityMovementComponent movementComponent;
		
		// -------------------------------------------------------------------------------
		// 
		// -------------------------------------------------------------------------------
		protected override void Start()
    	{
    		proxChecker = GetComponent<NetworkProximityChecker>();
    		
    		movementComponent = gameObject.GetComponent<EntityMovementComponent>();
    		
        	base.Start();
		}
		
		// -------------------------------------------------------------------------------
		// UpdateServer
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// UpdateClient
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================