
using OpenMMO;
using OpenMMO.UI;
using OpenMMO.Network;
using UnityEngine;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIRoot
	// ===================================================================================
	public abstract partial class UIRoot
	{

		protected OpenMMO.Network.NetworkManager _networkManager = null;
		protected OpenMMO.Network.NetworkAuthenticator _networkAuthenticator = null;
		
		// -------------------------------------------------------------------------------
		// networkManager
		// -------------------------------------------------------------------------------
		protected OpenMMO.Network.NetworkManager networkManager
		{
			get
			{
                if (_networkManager == null)
                {
                    _networkManager = FindObjectOfType<OpenMMO.Network.NetworkManager>();
                    if (!_networkManager) Debug.LogError("UIRoot could not find a NetworkManager in the Scene...");
                }
				return _networkManager;
			}
		}
        public virtual bool CanClick()
        {
            return networkManager.CanClick();
        }
		
		// -------------------------------------------------------------------------------
		// networkAuthenticator
		// -------------------------------------------------------------------------------
		protected OpenMMO.Network.NetworkAuthenticator networkAuthenticator
		{
			get
			{
                if (_networkAuthenticator == null)
                {
                    _networkAuthenticator = FindObjectOfType<OpenMMO.Network.NetworkAuthenticator>();
                    if (!_networkManager) Debug.LogError("UIRoot could not find a NetworkAuthenticator in the Scene...");
                }
				return _networkAuthenticator;
			}
		}
		
	}

}

// =======================================================================================