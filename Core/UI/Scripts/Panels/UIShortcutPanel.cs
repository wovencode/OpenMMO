using UnityEngine;
using OpenMMO;
using OpenMMO.UI;
using OpenMMO.Network;

namespace OpenMMO.UI
{
	
	// ===================================================================================
	// UIShortcutPanel
	// ===================================================================================
	public partial class UIShortcutPanel : UIRoot
	{

		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
		
			if (!networkManager || networkManager.state != NetworkState.Game)
				Hide();
			else
				Show();
			
		}
		
		// -------------------------------------------------------------------------------
		
    }

}
