//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;
using UnityEngine.AI;
using Mirror;

namespace OpenMMO {
	
	/// <summary>
	/// [abstract][partial] MobileComponent is the base class for all movable Entities (Players, NPCs, Monsters, Pets etc.)
	/// </summary>
	[System.Serializable]
	public abstract partial class MobileComponent : SpawnableComponent
	{
		
		[Header("Components")]
		public NavMeshAgent agent;
        //public NetworkProximityChecker proxChecker; //REMOVED - DX4D
#pragma warning disable CS0109
        public new Collider collider;
#pragma warning restore CS0109
		
		[Header("Default Data")]
		public ArchetypeTemplate archeType;
		
		// -- Component Cache
		[HideInInspector] public EntityMovementComponent movementComponent;
		
		///// <summary> The Start methods initializes the component and prepares its basic properties. </summary>
		//protected override void Start() { base.Start(); }

        //ON VALIDATE - LOAD COMPONENTS
        private void OnValidate() { LoadComponents(); }

        //LOAD COMPONENTS
        void LoadComponents()
        {
    		//if (!proxChecker) proxChecker = GetComponent<NetworkProximityChecker>(); //REMOVED - DX4D
            if (!movementComponent) movementComponent = GetComponent<EntityMovementComponent>();
        }
        // H O O K A B L E  M E T H O D S
        protected override void StartServer()
        {
            base.StartServer();
            LoadComponents();
        }
        protected override void StartClient()
        {
            base.StartClient();
            LoadComponents();
        }
        /// <summary> [server] Throttled update that runs server-side.
        /// Hook this method using [DevExtMethods(nameof(UpdateServer))]</summary>
        [Server] protected override void UpdateServer() { this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); } //HOOK
		
        /// <summary> [client] Throttled update that runs client-side.
        /// Hook this method using [DevExtMethods(nameof(UpdateClient))] </summary>
        [Client] protected override void UpdateClient() { this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); } //HOOK
	}
}