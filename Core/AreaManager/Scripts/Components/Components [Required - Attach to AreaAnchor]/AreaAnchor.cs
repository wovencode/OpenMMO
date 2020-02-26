using OpenMMO;
using OpenMMO.Debugging;
using OpenMMO.Areas;
using UnityEngine;
using Mirror;

namespace OpenMMO.Areas
{
    // This script is attached to a scene object called Zone that is on the Player layer and has:
    // - Sphere Collider with isTrigger = true
    // - Network Identity with Server Only checked
    // These OnTrigger events only run on the server and will only send a message to the player
    // that entered the Zone to load the subscene assigned to the subscene property.
    // ===================================================================================
	// AreaTrigger
	// ===================================================================================
	[DisallowMultipleComponent]
    public partial class AreaAnchor: NetworkBehaviour
    {
    
    	[Header("Sub Scene")]
        [Tooltip("Assign the sub-scene(s) to load or unload for this area")]
        public UnityScene subScene;
       
        // -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        public void Awake()
        {
            AreaManager.singleton.RegisterAreaAnchor(subScene);
        }
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
        public void OnDestroy()
        {
        	if (AreaManager.singleton)
            	AreaManager.singleton.UnRegisterAreaAnchor(subScene);
        }

		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Server
		// -------------------------------------------------------------------------------
        [Server]
        void OnTriggerEnter(Collider co)
        {
            NetworkIdentity ni = co.gameObject.GetComponentInParent<NetworkIdentity>();
            AreaManager.singleton.LoadScenesAdditive(ni, subScene);
        }
        
		// -------------------------------------------------------------------------------
		// OnTriggerExit
		// @Server
		// -------------------------------------------------------------------------------
        [Server]
        void OnTriggerExit(Collider co)
        {
            NetworkIdentity ni = co.gameObject.GetComponentInParent<NetworkIdentity>();
           	AreaManager.singleton.UnloadScenesAdditive(ni, subScene);
        }
        
        // -------------------------------------------------------------------------------
        
    }
    
    // ===================================================================================
    
}
