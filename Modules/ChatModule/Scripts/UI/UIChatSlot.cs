
using UnityEngine;
using UnityEngine.UI;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using OpenMMO.Chat;

namespace OpenMMO.UI
{
	
	// ===================================================================================
	// UIChatSlot
	// ===================================================================================
	public partial class UIChatSlot : UIButton
	{
		
		[Header("UI Elements")]
		public Text text;
		
		
		// -------------------------------------------------------------------------------
		// Init
		// -------------------------------------------------------------------------------
		public void Init(ChatMessage message)
		{
			text.text = message.GetMessage();
			
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================