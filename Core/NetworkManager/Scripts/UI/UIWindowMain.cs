
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIWindowMain
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowMain : UIRoot
	{
			
		[Header("Windows")]
		public UIWindowLoginUser 			loginWindow;
		public UIWindowRegisterUser 		registerWindow;
		public UIWindowChangePasswordUser 	changePasswordWindow;
		public UIWindowDeleteUser 			deleteWindow;
		
		[Header("Buttons")]
		public Button loginButton;
		public Button registerButton;
		public Button changePasswordButton;
		public Button deleteButton;
		public Button serverButton;
		public Button quitButton;
		
		public static UIWindowMain singleton;
		
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
			networkManager.TryLogoutUser();
			base.Show();
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{

			loginButton.interactable = networkManager.CanClick();
			loginButton.onClick.SetListener(() => { OnClickLogin(); });
			
			registerButton.interactable = networkManager.CanClick();
			registerButton.onClick.SetListener(() => { OnClickRegister(); });
			
			changePasswordButton.interactable = networkManager.CanClick();
			changePasswordButton.onClick.SetListener(() => { OnClickChangePassword(); });
			
			deleteButton.interactable = networkManager.CanClick();
			deleteButton.onClick.SetListener(() => { OnClickDeleteUser(); });
		
			serverButton.interactable = networkManager.CanStartServer();
			serverButton.onClick.SetListener(() => { OnClickStartServer(); });
		
			quitButton.onClick.SetListener(() => { OnClickQuit(); });

		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickLogin
		// -------------------------------------------------------------------------------
		public void OnClickLogin()
		{
			loginWindow.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickRegister
		// -------------------------------------------------------------------------------
		public void OnClickRegister()
		{
			registerWindow.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickChangePassword
		// -------------------------------------------------------------------------------
		public void OnClickChangePassword()
		{
			changePasswordWindow.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickDeleteUser
		// -------------------------------------------------------------------------------
		public void OnClickDeleteUser()
		{
			deleteWindow.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickStartServer
		// -------------------------------------------------------------------------------
		public void OnClickStartServer()
		{
			networkManager.TryStartServer();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickQuit
		// -------------------------------------------------------------------------------
		public void OnClickQuit()
		{
			networkManager.Quit();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================