//by Fhiz
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using OpenMMO;
using OpenMMO.Network;

namespace OpenMMO {
	
	/// <summary>
	/// Abstract partial EntityComponent is the base class for all Entities (Players, NPCs, Monsters, Pets etc.)
	/// </summary>
	[System.Serializable]
	public abstract partial class MobileComponent : LevelableComponent
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
		
		/// <summary>
		/// The Start methods initializes the component and prepares its basic properties.
		/// </summary>
		protected override void Start()
    	{
    		proxChecker = GetComponent<NetworkProximityChecker>();
    		
    		movementComponent = gameObject.GetComponent<EntityMovementComponent>();
    		
        	base.Start();
		}
		
		/// <summary>
		/// Server-side throttled update
		/// </summary>
		[Server]
		protected override void UpdateServer()
		{
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK
		}
		
		/// <summary>
		/// Client-side throttled update
		/// </summary>
		[Client]
		protected override void UpdateClient()
		{
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
		}
		
	}

}