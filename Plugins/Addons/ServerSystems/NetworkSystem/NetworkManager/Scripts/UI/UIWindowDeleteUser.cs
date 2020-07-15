
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIWindowDeleteUser
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowDeleteUser : UIRoot
	{
	
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField userpassInput;
		
		[Header("Buttons")]
		public Button deleteButton;
		public Button backButton;
		
		[Header("System Texts")]
		public string popupDescription = "Do you really want to delete this account?";
		
		protected const string _userName = "UserName";
		protected const string _userPass = "UserPass";
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (!Tools.IsAllowedName(usernameInput.text) || !Tools.IsAllowedPassword(userpassInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			deleteButton.interactable = networkManager.CanDeleteUser(usernameInput.text, userpassInput.text);
			deleteButton.onClick.SetListener(() => { OnClickDelete(); });
			
			backButton.onClick.SetListener(() => { OnClickBack(); });
				
		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickDelete
		// -------------------------------------------------------------------------------
		protected void OnClickDelete()
		{
			UIPopupPrompt.singleton.Init(popupDescription, OnClickConfirmDelete, OnClickCancelDelete);
		}
		
		// -------------------------------------------------------------------------------
		// OnClickBack
		// -------------------------------------------------------------------------------
		public void OnClickBack()
		{
			Hide();
			UIWindowMain.singleton.Show();
		}
		
		// ================================ EVENT HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickConfirmDelete
		// -------------------------------------------------------------------------------
		public void OnClickConfirmDelete()
		{
			networkManager.TryDeleteUser(usernameInput.text, userpassInput.text);
			
			Tools.PlayerPrefsSetString(Constants.PlayerPrefsUserName, "", usernameInput.text);
			Tools.PlayerPrefsSetString(Constants.PlayerPrefsPassword, "", userpassInput.text);
			
			usernameInput.text = "";
			userpassInput.text = "";
			
		}
		
		// -------------------------------------------------------------------------------
		// OnClickCancelDelete
		// -------------------------------------------------------------------------------
		public void OnClickCancelDelete()
		{
			Hide();
			UIWindowMain.singleton.Show();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================