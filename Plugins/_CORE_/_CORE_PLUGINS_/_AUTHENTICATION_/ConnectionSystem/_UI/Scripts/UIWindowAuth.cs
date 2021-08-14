//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.UI;
using System.Linq;

using OpenMMO.Network;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //Text Mesh Pro

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
        
        [Header("CONNECTION TIMEOUT")]
        [Tooltip("When the timeout is reached, how long will the message show before closing the client?")]
        public float timeoutNotificationDuration = 4f;
        [Header("AUTO CONNECT")]
        public Toggle autoConnectToggle;
        //[SerializeField] bool autoConnect = true;
		protected byte autoConnectTimer = 0;
		
		[Header("Dropdown")]
		public Dropdown serverDropdown;
		
		[Header("Buttons")]
		public Button connectButton;
        [Header("TEXT LABELS")]
        [Tooltip("Text Mesh Pro - Defaults to regular Text if null")]
        public TMP_Text connectButtonTextMesh;
        [Tooltip("Default Unity Text - Ignored if TextMesh is assigned")]
        public Text connectButtonText;
		
		[Header("System Texts")]
		public UIWindowAuth_Lang systemTexts;
		
		public static UIWindowAuth singleton;


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
			autoConnectTimer = 0;
			LoadServerList(true);
            LoadAutoConnect();
			base.Show();
		}
		
		// -------------------------------------------------------------------------------
		// Hide
		// -------------------------------------------------------------------------------
		public override void Hide()
		{
			CancelInvoke();

            //REMEMBER SERVER
            if (rememberServer)
            {
                PlayerPrefs.SetString(Defines.Connect.LastServer, serverDropdown.captionText.text);
            }
			base.Hide();	
		}

        //THROTTLED UPDATE
        float autoConnectTimerProgress = 0f;
		protected override void ThrottledUpdate()
		{
			//LOAD SERVER LIST
			LoadServerList();

            //VALIDATE COMPONENTS
            if (networkAuthenticator == null || networkManager == null)
            {
                Debug.LogWarning("<<<ISSUE>>> [CONNECT] The Component Connections for this UI Element have not been assigned.");
                return;
            }

            //TODO: It seems like this gets invoked every frame - fix that
            //INVOKE CONNECTION TIMEOUT TIMER
            if (networkAuthenticator.connectTimeout > 0)
            {
                Debug.Log("[CONNECT] Launching Connect Timeout Handler...");
                Invoke(nameof(Timeout), networkAuthenticator.connectTimeout);
            }

            //CHECK IF AUTOCONNECT IS ENABLED
            if (autoConnectToggle)
            {
                if (!autoConnectToggle.isOn)
                {
                    UpdateAutoConnectCountdownText(false); //UPDATE COUNTDOWN TEXT
                    StopConnecting();
                    return; //DO NOT AUTOCONNECT
                }
                else
                {
                    StartConnecting();
                }
            }

            //RESET EXPIRED AUTO CONNECT TIMER
            if (autoConnectTimer == 0) { autoConnectTimer = networkAuthenticator.connectDelay; }

            //UPDATE AUTO CONNECT TIMER AND DISPLAY
			if (networkManager.IsConnecting())
            {
                //TICK AUTO CONNECT TIMER
                autoConnectTimerProgress += updateInterval;
                if (autoConnectTimerProgress >= 1f)
                {
                    autoConnectTimerProgress -= 1f;
                    autoConnectTimer--;
                }
                //UPDATE TEXT LABELS
                UpdateAutoConnectCountdownText();
			}
        }
        //STOP CONNECTING
        void StopConnecting()
        {
            //if (!networkManager.IsConnecting()) { return; } //NOT CONNECTING
            networkAuthenticator.CancelAutoAuthentication(); //CANCEL AUTHENTICATION
        }
        //START CONNECTING
        void StartConnecting()
        {
            //if (networkManager.IsConnecting()) { return; } //ALREADY CONNECTING
            networkAuthenticator.StartAutoAuthentication(autoConnectTimer); //START AUTHENTICATION
        }
        //UPDATE COUNTDOWN TEXT
        void UpdateAutoConnectCountdownText(bool _autoConnect = true)
        {
            if (connectButtonTextMesh)
            {
                connectButtonTextMesh.text = systemTexts.clientConnect;
                connectButtonTextMesh.text += (_autoConnect ? " (" + autoConnectTimer.ToString() + ")" : "");
            }
            else if (connectButtonText)
            {
                connectButtonText.text = systemTexts.clientConnect;
                connectButtonText.text += (_autoConnect ? " (" + autoConnectTimer.ToString() + ")" : "");
            }
        }
		
		// -------------------------------------------------------------------------------
		// Timeout
		// -------------------------------------------------------------------------------
		protected void Timeout()
		{
            CancelInvoke();
            UIPopupCountdown.singleton.Init(systemTexts.connectTimeout, timeoutNotificationDuration, OnClickQuit);
			//UIPopupConfirm.singleton.Init(systemTexts.connectTimeout, OnClickQuit); //REMOVED DX4D
		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// onClickQuit
		// -------------------------------------------------------------------------------
		protected void OnClickQuit()
        {
			CancelInvoke();
            NetworkManager.singleton.Quit();
            //networkManager.Quit(); //REMOVED DX4D
		}
		
		// -------------------------------------------------------------------------------
		// OnClickConnect
		// -------------------------------------------------------------------------------
		public void OnClickConnect()
		{
			CancelInvoke();
			networkAuthenticator.ClientAuthenticate();
		}
		
		// -------------------------------------------------------------------------------
		// OnDropdownChange
		// -------------------------------------------------------------------------------
		public void OnDropdownChange()
		{
			if (rememberServer)
				PlayerPrefs.SetString(Defines.Connect.LastServer, serverDropdown.captionText.text);
			
            networkManager.networkAddress = ServerConfigTemplate.singleton.serverList[serverDropdown.value].ip;
		}
		
		// -------------------------------------------------------------------------------
		// OnToggleChange
		// -------------------------------------------------------------------------------
		public void OnToggleChange()
        {
            SaveAutoConnect();
		}
        //SAVE AUTOCONNECT
        protected void SaveAutoConnect()
        {
            PlayerPrefs.SetInt(Defines.Connect.AutoConnect, autoConnectToggle.isOn ? 1 : 0);
        }
        //LOAD AUTOCONNECT
        protected void LoadAutoConnect()
        {
            if (autoConnectToggle)
            {
                autoConnectToggle.isOn = (PlayerPrefs.GetInt(Defines.Connect.AutoConnect) == 1);
            }
        }
		
		// =============================== LOAD SERVER LIST ==============================
		

		// -------------------------------------------------------------------------------
		// LoadServers
		// -------------------------------------------------------------------------------
		protected void LoadServerList(bool forced=false)
		{
			if (loaded || ServerConfigTemplate.singleton == null)
				return;
			
			serverDropdown.options.Clear();
			
			foreach (ServerInfoTemplate template in ServerConfigTemplate.singleton.serverList)
			{
				if (template.visible)
					serverDropdown.options.Add(new Dropdown.OptionData(template.title));
			}
			
			if (rememberServer && PlayerPrefs.HasKey(Defines.Connect.LastServer))
			{
				string lastServer = PlayerPrefs.GetString(Defines.Connect.LastServer, "");
				
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