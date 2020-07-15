//by  Fhiz
using OpenMMO;
using OpenMMO.UI;
using UnityEngine;

namespace OpenMMO.UI
{

	/// <summary>
	/// This partial section of UIRoot adds a localPlayer object to it. It can be used to quickly fetch the local player without a GetComponent call everywhere.
	/// </summary>
	public abstract partial class UIRoot
	{

		protected GameObject _localPlayer = null;

		/// <summary>
		/// Returns the cached local player object or fetches it once and caches it.
		/// </summary>
		protected GameObject localPlayer
		{
			get
			{
				if (_localPlayer == null)
					_localPlayer = PlayerAccount.localPlayer;
				return _localPlayer;
			}
		
		}

	}

}