
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIWindowAuth
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowAuth : UIRoot
	{
		
		[Header("Settings")]
		public bool rememberServer;
		
		[Header("Dropdown")]
		public Dropdown serverDropdown;
		
		[Header("Buttons")]
		public Button connectButton;
		public Text connectButtonText;
		
		[Header("System Texts")]
		public UIWindowAuth_Lang systemTexts;
		
		public static UIWindowAuth singleton;
		
		protected int connectTimer = -1;
		protected bool loaded;
		
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
			loaded = false;
			connectTimer = -1;
			LoadServers(true);
			base.Show();
		}
		
		// -------------------------------------------------------------------------------
		// Hide
		// -------------------------------------------------------------------------------
		public override void Hide()
		{
			
			CancelInvoke();
			
			if (rememberServer)
				PlayerPrefs.SetString(Constants.PlayerPrefsLastServer, serverDropdown.captionText.text);
		
			base.Hide();
			
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			LoadServers();
			
			if (networkAuthenticator == null || networkManager == null)
				return;
				
			if (networkAuthenticator.connectTimeout > 0)
				Invoke(nameof(Timeout), networkAuthenticator.connectTimeout);
						
			if (connectTimer == -1)
				connectTimer = networkAuthenticator.connectDelay;
				
			if (networkManager.IsConnecting())
			{
				if (connectButtonText)
				{
					connectTimer--;
					connectButtonText.text = systemTexts.clientConnect + " (in " + connectTimer.ToString() + "s)";
				}
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// Timeout
		// -------------------------------------------------------------------------------
		protected void Timeout()
		{
			UIPopupConfirm.singleton.Init(systemTexts.serverOffline, OnClickQuit);
		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// onClickQuit
		// -------------------------------------------------------------------------------
		protected void OnClickQuit()
		{
			networkManager.Quit();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickConnect
		// -------------------------------------------------------------------------------
		public void OnClickConnect()
		{
			CancelInvoke();
			networkAuthenticator.ClientConnect();
		}
		
		// -------------------------------------------------------------------------------
		// OnDropdownChange
		// -------------------------------------------------------------------------------
		public void OnDropdownChange()
		{
			if (rememberServer)
				PlayerPrefs.SetString(Constants.PlayerPrefsLastServer, serverDropdown.captionText.text);
			
            networkManager.networkAddress = ServerConfigTemplate.singleton.serverList[serverDropdown.value].ip;
		}
		
		// ================================= LOAD SERVERS ================================
		
		// -------------------------------------------------------------------------------
		// LoadServers
		// -------------------------------------------------------------------------------
		protected void LoadServers(bool forced=false)
		{
			
			if (loaded || ServerConfigTemplate.singleton == null)
				return;
			
			serverDropdown.options.Clear();
			
			foreach (ServerInfoTemplate template in ServerConfigTemplate.singleton.serverList)
			{
				if (template.visible)
					serverDropdown.options.Add(new Dropdown.OptionData(template.title));
			}
			
			if (rememberServer && PlayerPrefs.HasKey(Constants.PlayerPrefsLastServer))
			{
				string lastServer = PlayerPrefs.GetString(Constants.PlayerPrefsLastServer, "");
				
				for (int i = 0; i < ServerConfigTemplate.singleton.serverList.Length; i++)
					if (ServerConfigTemplate.singleton.serverList[i].visible && ServerConfigTemplate.singleton.serverList[i].title == lastServer)
						serverDropdown.value = i;
			}
			else
				serverDropdown.value = 0;
			
			serverDropdown.captionText.text = ServerConfigTemplate.singleton.serverList[serverDropdown.value].title;
			
			networkManager.networkAddress = ServerConfigTemplate.singleton.serverList[serverDropdown.value].ip;
			
			loaded = true;
			
		}
		
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================