
using Wovencode;
using Wovencode.UI;
using Wovencode.Network;
using UnityEngine;

namespace Wovencode.UI
{

	// ===================================================================================
	// UIRoot
	// ===================================================================================
	public abstract partial class UIRoot
	{

		protected Wovencode.Network.NetworkManager _networkManager = null;
		protected Wovencode.Network.NetworkAuthenticator _networkAuthenticator = null;
		
		// -------------------------------------------------------------------------------
		// networkManager
		// -------------------------------------------------------------------------------
		protected Wovencode.Network.NetworkManager networkManager
		{
			get
			{
				if (_networkManager == null)
					_networkManager = FindObjectOfType<Wovencode.Network.NetworkManager>();
				return _networkManager;
			}
		}
		
		// -------------------------------------------------------------------------------
		// networkAuthenticator
		// -------------------------------------------------------------------------------
		protected Wovencode.Network.NetworkAuthenticator networkAuthenticator
		{
			get
			{
				if (_networkAuthenticator == null)
					_networkAuthenticator = FindObjectOfType<Wovencode.Network.NetworkAuthenticator>();
				return _networkAuthenticator;
			}
		}
		
	}

}

// =======================================================================================