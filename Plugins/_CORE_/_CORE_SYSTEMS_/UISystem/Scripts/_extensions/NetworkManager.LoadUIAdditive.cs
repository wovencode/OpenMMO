//by Fhiz
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

    /// <summary>
    /// This partial section of NetworkManager adds the GUI scene and loading to it.
    /// </summary>
	public partial class NetworkManager
	{
				
		[Header("GUI SCENE")]
        [Tooltip("")]
		public UnityScene guiScene;

		/// <summary>
    	/// Hooks into AwakePriority to additively load the UI scene.
    	/// </summary>
		[DevExtMethods(nameof(AwakePriority))]
		void AwakePriority_UI()
		{
#if _CLIENT
			// -- load the UI scene only if we are client & UI available
			LoadUIAdditive();
#endif
		}
		
		/// <summary>
    	/// This actually loads the UI scene.
    	/// </summary>
		protected void LoadUIAdditive()
		{
            if (guiScene != null)
            {
                Debug.Log("<b>[<color=blue>GUI CLIENT</color>] - </b>"
                    + "<b>Loading UI " + guiScene.SceneName + "...</b>");
                SceneManager.LoadScene(guiScene, LoadSceneMode.Additive);
                Debug.Log("<b>[<color=green>GUI CLIENT</color>] - </b>"
                    + "<b>Loaded UI " + guiScene.SceneName + "!</b>");
                return;
            }
            else if (guiScene == null || string.IsNullOrWhiteSpace(guiScene.SceneName))
            {
                Debug.Log("<b>>>>ISSUE<<< [<color=red>GUI CLIENT</color>]</b> - "
                    + " UI Not Loaded - GUI Scene was not assigned in " + this.name);
                return;
            }
		}
	}
}