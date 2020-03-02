//by Fhiz
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;

using System;
using System.Collections.Generic;

namespace OpenMMO.Network
{

    /// <summary>
	///
	/// </summary>
	public partial class NetworkManager
	{
		
		/// <summary>
		/// Hooks into awake of the NetworkManager to automatically preload templates (= scriptable objects) only if we are the server & a preloader is available.
		/// </summary>
		[DevExtMethods(nameof(Awake))]
		void Awake_ScriptableTemplates()
		{
#if _SERVER && !_CLIENT
			PreloadScriptableTemplates();
#endif
		}

		/// <summary>
		/// Executes the actual preload process if the required component is available.
		/// </summary>
		protected void PreloadScriptableTemplates()
    	{
       		PreloaderComponent preloader = GetComponent<PreloaderComponent>();
       		if (preloader)
        		preloader.PreloadTemplates();
    	}

	}

}