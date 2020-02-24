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
        [Tooltip("Assign the sub-scene to load for this area")]
        public UnityScene subScene;
        
        // -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        public void Awake()
        {
            AreaManager.RegisterAreaAnchor(subScene);
        }
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
        public void OnDestroy()
        {
            AreaManager.UnRegisterAreaAnchor(subScene);
        }

		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Server
		// -------------------------------------------------------------------------------
        [Server]
        void OnTriggerEnter(Collider co)
        {
            
			DebugManager.LogFormat(this.name, nameof(OnTriggerEnter), subScene.SceneName); //DEBUG
			
            NetworkIdentity networkIdentity = co.gameObject.GetComponentInParent<NetworkIdentity>();
            NetworkServer.SendToClientOfPlayer(networkIdentity, new SceneMessage { sceneName = subScene.SceneName, sceneOperation = SceneOperation.LoadAdditive });
        }
        
		// -------------------------------------------------------------------------------
		// OnTriggerExit
		// @Server
		// -------------------------------------------------------------------------------
        [Server]
        void OnTriggerExit(Collider co)
        {
        
            DebugManager.LogFormat(this.name, nameof(OnTriggerExit), subScene.SceneName); //DEBUG

            NetworkIdentity networkIdentity = co.gameObject.GetComponentInParent<NetworkIdentity>();
            NetworkServer.SendToClientOfPlayer(networkIdentity, new SceneMessage { sceneName = subScene.SceneName, sceneOperation = SceneOperation.UnloadAdditive });
        }
        
        // -------------------------------------------------------------------------------
        
    }
    
    // ===================================================================================
    
}
