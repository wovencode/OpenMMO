
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
		public new Collider collider;
		
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