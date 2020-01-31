
using Wovencode;
using Wovencode.Network;
using Wovencode.UI;
using Wovencode.DebugManager;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Wovencode.UI
{

	// ===================================================================================
	// UIWindowRegisterUser
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowRegisterUser : UIRoot
	{
	
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField userpassInput;
		public InputField usermailInput;
		
		[Header("Buttons")]
		public Button registerButton;
		public Button backButton;
		
		// -------------------------------------------------------------------------------
		// Show
		// -------------------------------------------------------------------------------
		public override void Show()
		{
			usernameInput.text = "";
			userpassInput.text = "";
			usermailInput.text = "";
			base.Show();
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (!Tools.IsAllowedName(usernameInput.text) || !Tools.IsAllowedPassword(userpassInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			registerButton.interactable = networkManager.CanRegisterUser(usernameInput.text, userpassInput.text);
			registerButton.onClick.SetListener(() => { OnClickRegister(); });
			
			backButton.onClick.SetListener(() => { OnClickBack(); });

		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickRegister
		// -------------------------------------------------------------------------------
		public void OnClickRegister()
		{
			networkManager.TryRegisterUser(usernameInput.text, userpassInput.text, usermailInput.text);
		}
		
		// -------------------------------------------------------------------------------
		// OnClickBack
		// -------------------------------------------------------------------------------
		public void OnClickBack()
		{
			UIWindowMain.singleton.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================