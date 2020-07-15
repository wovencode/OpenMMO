//by Fhiz
using OpenMMO;
using OpenMMO.Debugging;
using OpenMMO.Areas;
using UnityEngine;
using Mirror;

namespace OpenMMO.Areas
{
    
   	/// <summary>
   	/// Attach this component to a area portal to load/unload the scene when entering/exiting it.
   	/// </summary>
	[DisallowMultipleComponent]
    public partial class AreaPortal: NetworkBehaviour
    {
    
    	[Header("Sub Scene")]
        [Tooltip("Assign the sub-scene(s) to load or unload for this area")]
        public UnityScene subScene;
       
       	/// <summary>
   		/// Calls AwakeLate with a small delay to let the NetworkManager awake first.
   		/// </summary>
        public void Awake()
        {
            Invoke(nameof(AwakeLate), 0.1f);
        }
        
        /// <summary>
   		/// Registers the portal to the Area Manager.
   		/// </summary>
        public void AwakeLate()
        {
            AreaManager.singleton.RegisterAreaPortal(subScene);
        }
		
		/// <summary>
   		/// Unregisters the portal from the Area Manager again when this object is destroyed.
   		/// </summary>
        public void OnDestroy()
        {
        	if (AreaManager.singleton)
            	AreaManager.singleton.UnRegisterAreaPortal(subScene);
        }

		/// <summary>
   		/// Additively loads the scene on enter (further checks are done in Area Manager)
   		/// </summary>
        [Server]
        void OnTriggerEnter(Collider co)
        {
            NetworkIdentity ni = co.gameObject.GetComponentInParent<NetworkIdentity>();
            AreaManager.singleton.LoadScenesAdditive(ni, subScene);
        }
        
		/// <summary>
   		/// Additively unloads the scene on exit (further checks are done in Area Manager
   		/// </summary>
        [Server]
        void OnTriggerExit(Collider co)
        {
            NetworkIdentity ni = co.gameObject.GetComponentInParent<NetworkIdentity>();
           	AreaManager.singleton.UnloadScenesAdditive(ni, subScene);
        }
        
    }
    
}
