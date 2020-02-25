using OpenMMO;
using OpenMMO.Debugging;
using OpenMMO.Areas;
using OpenMMO.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace OpenMMO.Areas
{

    // ===================================================================================
	// AreaManager
	// ===================================================================================
	[DisallowMultipleComponent]
    public partial class AreaManager : MonoBehaviour
    {
		
		[Header("Options")]
		public static bool active = true;
		
        [Header("Debug Helper")]
		public DebugHelper debug;
        
        protected static List<UnityScene> subScenes = new List<UnityScene>();
        
        protected OpenMMO.Network.NetworkManager networkManager;
        
        // -------------------------------------------------------------------------------
    	// Awake
    	// -------------------------------------------------------------------------------
		void Awake()
    	{
    		debug = new DebugHelper();
			debug.Init();
			
			if (!GetIsActive)
				return;
				
			networkManager = FindObjectOfType<OpenMMO.Network.NetworkManager>();
			
			networkManager.eventListeners.OnStartServer.AddListener(OnStartServer);
			networkManager.eventListeners.OnStopServer.AddListener(OnStopServer);
			networkManager.eventListeners.OnStopClient.AddListener(OnStopClient);
		}
		
        // -------------------------------------------------------------------------------
    	// OnDestroy
    	// -------------------------------------------------------------------------------
        void OnDestroy()
        {
        	
        	if (networkManager == null)
        		return;
        	
        	networkManager.eventListeners.OnStartServer.RemoveListener(OnStartServer);
			networkManager.eventListeners.OnStopServer.RemoveListener(OnStopServer);
			networkManager.eventListeners.OnStopClient.RemoveListener(OnStopClient);
        	networkManager = null;
        }
        
        // ================================== PUBLIC =====================================
        
        // -------------------------------------------------------------------------------
    	// GetIsActive
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public static bool GetIsActive
        {
        	get
        	{
				if (ProjectConfigTemplate.singleton.GetNetworkType == NetworkType.HostAndPlay)
					return active;
				return false;
        	}
        }
        
   		// ================================ SCENE ANCHORS ================================
    
    	// -------------------------------------------------------------------------------
    	// RegisterAreaAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public static void RegisterAreaAnchor(UnityScene subScene)
        {
       
        	if (!GetIsActive || subScene == null || String.IsNullOrWhiteSpace(subScene.SceneName))
        		return;
        		
            subScenes.Add(subScene);
            DebugManager.LogFormat(nameof(AreaManager), nameof(RegisterAreaAnchor), subScene.SceneName); //DEBUG
        }

        // -------------------------------------------------------------------------------
    	// UnRegisterAreaAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public static void UnRegisterAreaAnchor(UnityScene subScene)
        {
        	
        	if (!GetIsActive)
        		return;
        	
			for (int i = 0; i < subScenes.Count; i++)
			{
				if (subScenes[i] == null|| String.IsNullOrWhiteSpace(subScenes[i].SceneName))
					continue;
	
				if (subScenes[i].SceneName == subScene.SceneName)
				{
					subScenes.RemoveAt(i);
					DebugManager.LogFormat(nameof(AreaManager), nameof(UnRegisterAreaAnchor), subScene.SceneName); //DEBUG
				}
			}
			
        }
        
    	// -------------------------------------------------------------------------------
    	// LoadSceneAdditive
    	// @Server
    	// -------------------------------------------------------------------------------
    	public static void LoadSceneAdditive(NetworkIdentity ni, string SceneName)
    	{
    	
    		if (!GetIsActive)
        		return;
        	
        	 NetworkServer.SendToClientOfPlayer(ni, new SceneMessage { sceneName = SceneName, sceneOperation = SceneOperation.LoadAdditive });
        	
    	}
    	
    	// -------------------------------------------------------------------------------
    	// UnloadSceneAdditive
    	//  @Server
    	// -------------------------------------------------------------------------------
    	public static void UnloadSceneAdditive(NetworkIdentity ni, string SceneName)
    	{
    	
    		if (!GetIsActive)
        		return;
        	
        	NetworkServer.SendToClientOfPlayer(ni, new SceneMessage { sceneName = SceneName, sceneOperation = SceneOperation.UnloadAdditive });
        	
    	}
    	
        // ================================ PUBLIC EVENTS ================================
        
		// -------------------------------------------------------------------------------
		// OnStartServer
		// From: @NetworkManager
		// -------------------------------------------------------------------------------
        public void OnStartServer()
        {
            StartCoroutine(LoadSubScenes());
        }
        
		// -------------------------------------------------------------------------------
		// OnStopServer
		// From: @NetworkManager
		// -------------------------------------------------------------------------------
        public void OnStopServer()
        {
            StartCoroutine(UnloadScenes());
        }
        
		// -------------------------------------------------------------------------------
		// OnStopClient
		// From: @NetworkManager
		// -------------------------------------------------------------------------------
        public void OnStopClient()
        {
            StartCoroutine(UnloadScenes());
        }
		
		// ================================== PROTECTED ==================================
		
		// -------------------------------------------------------------------------------
		// LoadSubScenes
		// -------------------------------------------------------------------------------
        IEnumerator LoadSubScenes()
        {

			debug.LogFormat(this.name, nameof(LoadSubScenes), subScenes.Count.ToString()); //DEBUG
			
            foreach (UnityScene subScene in subScenes)
            {
            	
            	if (subScene == null || String.IsNullOrWhiteSpace(subScene.SceneName))
            		continue;
            	
                yield return SceneManager.LoadSceneAsync(subScene.SceneName, LoadSceneMode.Additive);
                
                debug.LogFormat(subScene.SceneName, "Loaded"); //DEBUG
            }
            
        }
		
		// -------------------------------------------------------------------------------
		// UnloadScenes
		// -------------------------------------------------------------------------------
        IEnumerator UnloadScenes()
        {
            
			debug.LogFormat(this.name, nameof(UnloadScenes), subScenes.Count.ToString()); //DEBUG
			
            foreach (UnityScene subScene in subScenes)
            {
            
            	if (subScene == null || String.IsNullOrWhiteSpace(subScene.SceneName))
            		continue;
            		
                if (SceneManager.GetSceneByName(subScene.SceneName).IsValid())
                {
                    yield return SceneManager.UnloadSceneAsync(subScene.SceneName);
                    debug.LogFormat(subScene.SceneName, "Unloaded"); //DEBUG
                }
			}
			
            yield return Resources.UnloadUnusedAssets();
        }
        
        // -------------------------------------------------------------------------------
        
    }
    
    // ===================================================================================
    
}
