
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIWindowChangePasswordUser
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowChangePasswordUser : UIRoot
	{
	
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField oldUserPassInput;
		public InputField newUserPassInput;
		
		[Header("Buttons")]
		public Button changeButton;
		public Button backButton;
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (!Tools.IsAllowedName(usernameInput.text) || !Tools.IsAllowedPassword(oldUserPassInput.text) || !Tools.IsAllowedPassword(newUserPassInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			changeButton.interactable = networkManager.CanChangePasswordUser(usernameInput.text, oldUserPassInput.text, newUserPassInput.text);
			changeButton.onClick.SetListener(() => { OnClickChangePassword(); });
		
			backButton.onClick.SetListener(() => { OnClickBack(); });

		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickChangePassword
		// -------------------------------------------------------------------------------
		protected void OnClickChangePassword()
		{
			networkManager.TryChangePasswordUser(usernameInput.text, oldUserPassInput.text, newUserPassInput.text);
			
			Tools.PlayerPrefsSetString(Constants.PlayerPrefsPassword, newUserPassInput.text, oldUserPassInput.text);
			
			usernameInput.text = "";
			oldUserPassInput.text = "";
			newUserPassInput.text = "";
			
		}
		
		// -------------------------------------------------------------------------------
		// OnClickBack
		// -------------------------------------------------------------------------------
		public void OnClickBack()
		{
			Hide();
			UIWindowMain.singleton.Show();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================