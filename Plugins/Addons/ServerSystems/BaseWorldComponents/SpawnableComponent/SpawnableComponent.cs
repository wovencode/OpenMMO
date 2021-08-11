//by Fhiz
//MODIFIED BY DX4D
#define USE_HOOKS //Toggle hooks on/off

using UnityEngine;
using UnityEngine.AI;
using Mirror;

namespace OpenMMO {
	
	/// <summary>
	/// [abstract][partial] SpawnableComponent is the base class for all spawnable Entities (Players, NPCs, Monsters, Pets, Props etc.)
	/// </summary>
	[System.Serializable]
	public abstract partial class SpawnableComponent : LevelableComponent
    {
        [Tooltip("Used to categorize templates into groups (commonly used by Items, Skills, Buffs etc.)")]
        [SerializeField] string _category = "";
        public string category { get { return _category; } }

        // H O O K A B L E  C L I E N T  M E T H O D S
        //START CLIENT
        /// <summary> [Client] The Start method initializes the component and prepares its basic properties.
#if USE_HOOKS
        /// Hook this method using [Hook(nameof(StartClient))]
#endif
        /// </summary>
        protected override void StartClient()
    	{
#if USE_HOOKS
            Hook.HookMethod<SpawnableComponent>(this, nameof(StartClient)); //HOOK
#endif
		}
        //UPDATE CLIENT
        /// <summary> [Client] Throttled update that runs client-side.
#if USE_HOOKS
        /// Hook this method using [Hook(nameof(UpdateClient))]
#endif
        /// Hook this method using [DevExtMethods(nameof(UpdateClient))]
        /// </summary>
        [Client] protected override void UpdateClient()
        {
#if USE_HOOKS
            Hook.HookMethod<SpawnableComponent>(this, nameof(UpdateClient)); //HOOK
#endif
            this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
        }


        // H O O K A B L E  S E R V E R  M E T H O D S
        //START SERVER
        /// <summary> [Server] The Start methods initializes the component and prepares its basic properties.
#if USE_HOOKS
        /// Hook this method using [Hook(nameof(StartServer))]
#endif
        /// </summary>
        protected override void StartServer()
    	{
#if USE_HOOKS
            Hook.HookMethod<SpawnableComponent>(this, nameof(StartServer)); //HOOK
#endif
		}
        //UPDATE SERVER
        /// <summary> [server] Throttled update that runs server-side.
#if USE_HOOKS
        /// Hook this method using [Hook(nameof(UpdateServer))]
#endif
        /// Hook this method using [DevExtMethods(nameof(UpdateServer))]
        /// </summary>
        [Server] protected override void UpdateServer()
        {
#if USE_HOOKS
            Hook.HookMethod<SpawnableComponent>(this, nameof(UpdateServer)); //HOOK
#endif
            this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK
        }
	}

}