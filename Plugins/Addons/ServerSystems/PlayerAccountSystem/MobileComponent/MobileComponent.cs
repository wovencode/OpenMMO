//by Fhiz
//MODIFIED BY DX4D
using UnityEngine;
using UnityEngine.AI;
using Mirror;

namespace OpenMMO {
	
	/// <summary>
	/// [abstract][partial] MobileComponent is the base class for all movable Entities (Players, NPCs, Monsters, Pets etc.)
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

        // H O O K A B L E  M E T H O D S

        /// <summary> [server] Throttled update that runs server-side.
        /// Hook this method using [DevExtMethods(nameof(UpdateServer))]</summary>
        [Server] protected override void UpdateServer() { this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); } //HOOK
		
        /// <summary> [client] Throttled update that runs client-side.
        /// Hook this method using [DevExtMethods(nameof(UpdateClient))] </summary>
        [Client] protected override void UpdateClient() { this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); } //HOOK
	}

}