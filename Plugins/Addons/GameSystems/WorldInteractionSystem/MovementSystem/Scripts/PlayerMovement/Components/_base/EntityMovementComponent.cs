//by Fhiz
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Partial Entity Movement Component is the base component for all entities and in charge of controlling movement.
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(MobileComponent))]
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class EntityMovementComponent : SyncableComponent
	{
		
		[Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;
        
		// -- Component Cache
		protected MobileComponent entityComponent;
		//public EntityEnergyComponent energyComponent; // TODO: add later to check "death" in states!
		
		[SyncVar] protected int state;
		
		/// <summary>
		/// Start caches components and does basic initialization.
		/// </summary>
		protected override void Start()
    	{
    	
    		agent = GetComponent<NavMeshAgent>();
    		animator = GetComponent<Animator>();
    		entityComponent = GetComponent<MobileComponent>();
    		
        	base.Start();
		}
		
		/// <summary>
		/// Server-side, throttled update.
		/// </summary>
		[Server]
		protected override void UpdateServer()
		{

			foreach(KeyValuePair<int, StateTemplate> stateTemplate in StateTemplate.data)
			{	
   				if (stateTemplate.Value.GetIsActive(entityComponent))
   				{
   					state = stateTemplate.Key;
   					return;
   				}
			}
			
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK
			
		}
		
		/// <summary>
		/// Client-side, throttled update.
		/// </summary>
		[Client]
		protected override void UpdateClient()
		{
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
		}
		
		/// <summary>
		/// Client-side, late-update responsible to update the animation state.
		/// </summary>
		[Client]
		protected override void LateUpdateClient()
		{

			if (state == 0)
				return;

			foreach(KeyValuePair<int, StateTemplate> stateTemplate in StateTemplate.data)	// each state
			{	
			
				string stateName = stateTemplate.Value.name;
				
				foreach (Animator anim in GetComponentsInChildren<Animator>())  			// each animator
				{
					if (anim.runtimeAnimatorController != null)  							// not null?
					{
						if (anim.parameters.Any(x => x.name == stateName)) 					// has param?
						{
	
							if (stateTemplate.Value.hash == state)							// is current state?
							{
								anim.SetBool(stateName, true);
							}
							else
							{
								anim.SetBool(stateName, false);								// otherwise deactivate
							}
						}
					}
				}
					
			}
						
			this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient)); //HOOK

		}
		
		/// <summary>
		/// Client-side, fixed update. Not used yet.
		/// </summary>
		[Client]
		protected override void FixedUpdateClient()
		{
			
		}
		
	}

}