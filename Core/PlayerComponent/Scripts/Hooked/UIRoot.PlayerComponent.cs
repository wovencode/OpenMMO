
using Wovencode;
using Wovencode.UI;
using UnityEngine;

namespace Wovencode.UI
{

	// ===================================================================================
	// UIRoot
	// ===================================================================================
	public abstract partial class UIRoot
	{

		protected GameObject _localPlayer = null;

		// -------------------------------------------------------------------------------
		// localPlayer
		// -------------------------------------------------------------------------------
		protected GameObject localPlayer
		{
			get
			{
				if (_localPlayer = null)
					_localPlayer = PlayerComponent.localPlayer;
				return _localPlayer;
			}
		
		}

	}

}

// =======================================================================================