
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIWindowLoginUser
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowLoginUser : UIRoot
	{
		
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField userpassInput;
		
		[Header("Buttons")]
		public Button loginButton;
		public Button backButton;
		
		[Header("Settings")]
		public bool rememberCredentials;
		[Range(0,9)] public float maxDelayDuration = 1;
		
		public static UIWindowLoginUser singleton;
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		void Start()
		{
			if (!rememberCredentials) return;
			
			if (PlayerPrefs.HasKey(Constants.PlayerPrefsUserName))
				usernameInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsUserName, "");
			else
				usernameInput.text = "";
				
			if (PlayerPrefs.HasKey(Constants.PlayerPrefsPassword))
				userpassInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsPassword, "");
			else
				userpassInput.text = "";
				
		}
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
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
			
			loginButton.interactable = networkManager.CanLoginUser(usernameInput.text, userpassInput.text);
			loginButton.onClick.SetListener(() => { OnClickLogin(); });
			
			backButton.onClick.SetListener(() => { OnClickBack(); });

		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickLogin
		// -------------------------------------------------------------------------------
		public void OnClickLogin()
		{
		
			if (rememberCredentials)
			{
				PlayerPrefs.SetString(Constants.PlayerPrefsUserName, usernameInput.text);
				PlayerPrefs.SetString(Constants.PlayerPrefsPassword, userpassInput.text);
			}
			
			Invoke(nameof(OnExecuteLogin), UnityEngine.Random.Range(maxDelayDuration/4, maxDelayDuration));
			
			loginButton.interactable 	= false;
			usernameInput.interactable 	= false;
			userpassInput.interactable 	= false;

		}
		
		// -------------------------------------------------------------------------------
		// OnExecuteLogin
		// -------------------------------------------------------------------------------
		protected void OnExecuteLogin()
		{
		
			networkManager.TryLoginUser(usernameInput.text, userpassInput.text);
			
			loginButton.interactable 	= true;
			usernameInput.interactable 	= true;
			userpassInput.interactable 	= true;
			
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