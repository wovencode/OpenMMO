
using Wovencode;
using Wovencode.Network;
using Wovencode.Database;
using Wovencode.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Mirror;

namespace Wovencode.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
	public partial class NetworkManager
	{
				
		[Header("GUI Scene")]
		public UnityScene guiScene;

		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		[DevExtMethods("AwakePriority")]
		void AwakePriority_UI()
		{
			
			// -- load the UI scene only if we are client & UI available
#if _CLIENT
			LoadUIAdditive();
#endif
		}
		
		// -------------------------------------------------------------------------------
		// LoadUIAdditive
		// -------------------------------------------------------------------------------
		protected void LoadUIAdditive()
		{
			if (guiScene != null)
				SceneManager.LoadScene(guiScene, LoadSceneMode.Additive);
		}
	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================