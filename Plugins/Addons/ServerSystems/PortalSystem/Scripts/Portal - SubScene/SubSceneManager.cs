//BY FHIZ
//MODIFIED BY DX4D

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

    /// <summary>
    /// Attach the SubSceneManager component to any game object inside a scene. It will register all additive scenes to itself automatically.
    /// </summary>
    [DisallowMultipleComponent]
    public partial class SubSceneManager : MonoBehaviour
    {
		
        [Header("Debug Helper")]
		public DebugHelper debug;
        
        protected List<UnityScene> subScenes = new List<UnityScene>();
        
        protected OpenMMO.Network.NetworkManager networkManager;
        
        public static SubSceneManager singleton;
       
       	/// <summary>
   		/// Sets reference to NetworkManager and adds event listeners to it.
   		/// </summary>
		void Awake()
    	{
    		
    		singleton = this;
    		
    		debug = new DebugHelper();
						
			if (!GetIsActive)
				return;
				
			networkManager = FindObjectOfType<OpenMMO.Network.NetworkManager>();
			
			networkManager.eventListeners.OnStartServer.AddListener(OnStartServer);
			networkManager.eventListeners.OnStopServer.AddListener(OnStopServer);
			networkManager.eventListeners.OnStopClient.AddListener(OnStopClient);
		}
		
        /// <summary>
   		/// Removes event listeners from NetworkManager again and clears up NetworkManager reference.
   		/// </summary>
        void OnDestroy()
        {
        	
        	if (networkManager == null)
        		return;
        	
        	networkManager.eventListeners.OnStartServer.RemoveListener(OnStartServer);
			networkManager.eventListeners.OnStopServer.RemoveListener(OnStopServer);
			networkManager.eventListeners.OnStopClient.RemoveListener(OnStopClient);
        	networkManager = null;
        }
        
        /// <summary>
   		/// Returns true if we are not in Host+Play mode, as additive areas only work in client/server environments.
   		/// </summary>
        public bool GetIsActive
        {
        	get
        	{
				return ServerConfigTemplate.singleton.GetNetworkType != NetworkType.HostAndPlay;
        	}
        }
        
   		// ================================ SUBSCENE PORTALS =================================

    	/// <summary>
   		/// Registers a new subscene portal to the subscene manager. Only the scenes of registered portals can be loaded/unloaded additively.
   		/// </summary>
        public void RegisterSubScenePortal(UnityScene subScene)
        {
       
        	if (!GetIsActive || subScene == null || String.IsNullOrWhiteSpace(subScene.SceneName) )
        		return;
        		
            subScenes.Add(subScene);
            DebugManager.LogFormat(nameof(SubSceneManager), nameof(RegisterSubScenePortal), subScene.SceneName); //DEBUG
        }

        /// <summary>
   		/// Unregisters a new subscene portal to the subscene manager. Only the scenes of registered portals can be loaded/unloaded additively.
   		/// </summary>
        public void UnRegisterSubScenePortal(UnityScene subScene)
        {
        	
        	if (!GetIsActive || subScene == null || String.IsNullOrWhiteSpace(subScene.SceneName) )
        		return;
        	
			for (int i = 0; i < subScenes.Count; i++)
			{
				if (subScenes[i] == null|| String.IsNullOrWhiteSpace(subScenes[i].SceneName))
					continue;
	
				if (subScenes[i].SceneName == subScene.SceneName)
				{
					subScenes.RemoveAt(i);
					DebugManager.LogFormat(nameof(SubSceneManager), nameof(UnRegisterSubScenePortal), subScene.SceneName); //DEBUG
				}
			}
			
        }
        
    	/// <summary>
   		/// Additively loads a scene on the client who entered the area portal.
   		/// </summary>
    	public void LoadScenesAdditive(NetworkIdentity ni, UnityScene scene)
    	{
    	
    		if (!GetIsActive || ni == null)
        		return;

            //NetworkServer.SendToClientOfPlayer(ni, new SceneMessage { sceneName = scene.SceneName, sceneOperation = SceneOperation.LoadAdditive }); //REMOVED - DX4D
            ni.connectionToClient.Send<SceneMessage>( new SceneMessage { sceneName = scene.SceneName, sceneOperation = SceneOperation.LoadAdditive }); //ADDED - DX4D

        }
    	
    	/// <summary>
   		/// Additively unloads a scene on the client who left the area portal.
   		/// </summary>	
    	public void UnloadScenesAdditive(NetworkIdentity ni, UnityScene scene)
    	{
    	
    		if (!GetIsActive || ni == null)
        		return;
        		
			//NetworkServer.SendToClientOfPlayer(ni, new SceneMessage { sceneName = scene.SceneName, sceneOperation = SceneOperation.UnloadAdditive }); //REMOVED - DX4D
            ni.connectionToClient.Send<SceneMessage>(new SceneMessage { sceneName = scene.SceneName, sceneOperation = SceneOperation.UnloadAdditive }); //ADDED - DX4D

        }
    	
        // ================================ PUBLIC EVENTS ================================
        
		/// <summary>
   		/// Loads all scenes additively when the server starts. Server-side only.
   		/// </summary>
   		/// <remarks>
   		/// Called from NetworkManager
   		/// </remarks>
        public void OnStartServer()
        {
            StartCoroutine(LoadSubScenes());
        }
        
		/// <summary>
   		/// Unloads all additively loaded scenes when the server stops.
   		/// </summary>
   		/// <remarks>
   		/// Called from NetworkManager
   		/// </remarks>
        public void OnStopServer()
        {
            StartCoroutine(UnloadScenes());
        }
        
		/// <summary>
   		/// Unloads all additively loaded scenes when the client stops.
   		/// </summary>
   		/// <remarks>
   		/// Called from NetworkManager
   		/// </remarks>
        public void OnStopClient()
        {
            StartCoroutine(UnloadScenes());
        }
		
		// ================================== PROTECTED ==================================
		
		/// <summary>
   		/// Additively loads all sub scenes to the current scene. Server-Side only. 
   		/// </summary>
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
		
		/// <summary>
   		/// Additively unloads all sub scenes to the current scene.
   		/// </summary>
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
        
    }
    
}