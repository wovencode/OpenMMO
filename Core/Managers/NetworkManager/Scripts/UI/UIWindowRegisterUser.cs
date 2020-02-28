
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using OpenMMO.Debugging;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
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
		
		public static UIWindowRegisterUser singleton;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
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
            usernameInput.text = Tools.TrimExcessWhitespace(usernameInput.text, true); //TRIM ALL WHITESPACE
			
			if (!Tools.IsAllowedName(usernameInput.text))
				statusText.text = "ENTER A VALID USERNAME";
            else if (!Tools.IsAllowedPassword(userpassInput.text))
				statusText.text = "ENTER A VALID PASSWORD";
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
			
			UIWindowLoginUser.singleton.Show();
			Hide();
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