
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
	public partial class NetworkManager
	{
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(Awake))]
		void Awake_ScriptableTemplates()
		{
			
			// -- preload scriptable objects only if we are server & preloader available
#if _SERVER && !_CLIENT
			PreloadScriptableTemplates();
#endif
	
		}

		// -------------------------------------------------------------------------------
		// PreloadScriptableTemplates
		// -------------------------------------------------------------------------------
		protected void PreloadScriptableTemplates()
    	{
       		PreloaderComponent preloader = GetComponent<PreloaderComponent>();
       		if (preloader)
        		preloader.PreloadTemplates();
    	}

		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================