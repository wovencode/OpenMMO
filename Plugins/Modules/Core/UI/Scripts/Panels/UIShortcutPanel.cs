//by Fhiz
using UnityEngine;
using OpenMMO;
using OpenMMO.UI;
using OpenMMO.Network;

namespace OpenMMO.UI
{
	
	/// <summary>
    /// Attach this script to the UIShortcutPanel that holds shortcut buttons.
    /// </summary>
	public partial class UIShortcutPanel : UIRoot
	{

		/// <summary>
    	/// Throttled update shows/hides the panel depending on NetworkManager NetworkState.
    	/// </summary>
		protected override void ThrottledUpdate()
		{
		
			if (!networkManager || networkManager.state != NetworkState.Game)
				Hide();
			else
				Show();
			
		}
		
    }

}
