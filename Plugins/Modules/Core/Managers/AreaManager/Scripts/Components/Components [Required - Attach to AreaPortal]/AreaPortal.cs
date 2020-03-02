using OpenMMO;
using OpenMMO.Debugging;
using OpenMMO.Areas;
using UnityEngine;
using Mirror;

namespace OpenMMO.Areas
{
    
    // ===================================================================================
	// AreaPortal
	// ===================================================================================
	[DisallowMultipleComponent]
    public partial class AreaPortal: NetworkBehaviour
    {
    
    	[Header("Sub Scene")]
        [Tooltip("Assign the sub-scene(s) to load or unload for this area")]
        public UnityScene subScene;
       
        // -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        public void Awake()
        {
            Invoke(nameof(AwakeLate), 0.1f);
        }
        
        // -------------------------------------------------------------------------------
		// AwakeLate
		// -------------------------------------------------------------------------------
        public void AwakeLate()
        {
            AreaManager.singleton.RegisterAreaPortal(subScene);
        }
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
        public void OnDestroy()
        {
        	if (AreaManager.singleton)
            	AreaManager.singleton.UnRegisterAreaPortal(subScene);
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
