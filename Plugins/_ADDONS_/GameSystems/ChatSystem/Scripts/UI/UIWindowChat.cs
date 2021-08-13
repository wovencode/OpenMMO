//IMPROVEMENTS BY DX4D
using System;
using UnityEngine;
using UnityEngine.UI;
using OpenMMO.UI;

namespace OpenMMO.Chat
{

	// ===================================================================================
	// UIWindowChat
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowChat : UIRoot
	{
		
		[Header("Chat Log Panel")]
		public GameObject chatLog;
		
		[Header("Chat Slot Prefab")]
		public UIChatSlot chatEntryPrefab;
		
		[Header("Chat Content View")]
		public Transform contentViewport;
		public ScrollRect scrollRect;
		
		[Header("Chat Buttons")]
		public Button toggleChatLogButton;
		public Image toggleChatLogButtonImage;
		public Button sendButton;
		
		[Header("Chat Input Field")]
		public InputField sendInputField;
		
		[Header("Chat Channel Buttons")]
		public Button publicChannelButton;
		public Button privateChannelButton;
		public Button guildChannelButton;
		public Button partyChannelButton;
		public Button infoChannelButton;
		
		[Header("Chat Channel Ids")]
		public string channelIdPublic 	= "public";
		public string channelIdPrivate 	= "private";
		public string channelIdGuild 	= "guild";
		public string channelIdParty 	= "party";
		public string channelIdInfo		= "info";
		
		[Header("Chat UI Icons")]
		public Sprite maximizedImage;
		public Sprite minimizedImage;
		
		[Header("Chat Send Keys")]
		public KeyCode[] sendKeys = {KeyCode.Return, KeyCode.KeypadEnter};
		
		public int maxMessages = 100;
		
		public static UIWindowChat singleton;
		
		protected string channelId = "public";
		
		protected bool inputActive;
		
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
		//public override void Show()
		//{
		//	base.Show();
		//}
		
		// -------------------------------------------------------------------------------
		// Update
		// -------------------------------------------------------------------------------
		protected override void Update()
		{
			
			// -- check for 'Enter' pressed while Input has focus
			
			foreach (KeyCode enterKey in sendKeys)
				if (Input.GetKeyDown(enterKey) && inputActive)
					OnClickSendMessage();
			
			base.Update();
		
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
        {
            if (!networkManager || networkManager.state != Network.NetworkState.Game)
                Hide();
            else
                Show();

            sendButton.interactable = !String.IsNullOrWhiteSpace(sendInputField.text);
			sendButton.onClick.SetListener(() 				=> { OnClickSendMessage(); });
			
			toggleChatLogButton.onClick.SetListener(() 		=> { OnClickToggleMinimized(); });
			
			publicChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelPublic(); });
			privateChannelButton.onClick.SetListener(() 	=> { OnClickSwitchChannelPrivate(); });
			guildChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelGuild(); });
			partyChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelParty(); });
			infoChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelInfo(); });
			
		}

        // =============================== BUTTON HANDLERS ===============================

        //ONCLICK SEND MESSAGE
        public void OnClickSendMessage()
        {
            if (String.IsNullOrWhiteSpace(sendInputField.text))
            {
                inputActive = false;
                return;
            }

            ChatManager.singleton.ClientChatSend(channelId, sendInputField.text);

            sendInputField.text = String.Empty;

            inputActive = true;
        }
        
        //ONCLICK TOGGLE MINIMIZED
        public void OnClickToggleMinimized()
        {
            ToggleMinimized();
        }

        // =============================== SHOW/HIDE FUNCTIONS ===============================

        //TOGGLE MINIMIZED
        private void ToggleMinimized()
        {
            if (chatLog.activeInHierarchy) { Minimize(); }
            else { Restore(); }

            inputActive = false;
        }
        //MINIMIZE
        private void Minimize()
        {
            chatLog.SetActive(false);
            toggleChatLogButtonImage.sprite = minimizedImage;
        }
        //RESTORE
        private void Restore()
        {
            chatLog.SetActive(true);
            toggleChatLogButtonImage.sprite = maximizedImage;
        }
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelPublic
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelPublic()
		{	
			channelId = channelIdPublic;
			sendButton.interactable = true;
		}
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelPrivate
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelPrivate()
		{	
			channelId = channelIdPrivate;
			sendButton.interactable = true;
		}
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelGuild
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelGuild()
		{	
			channelId = channelIdGuild;
			sendButton.interactable = true;
		}
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelParty
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelParty()
		{	
			channelId = channelIdParty;
			sendButton.interactable = true;
		}
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelInfo
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelInfo()
		{	
			channelId = channelIdInfo;
			sendButton.interactable = false;
		}
		
		// -------------------------------------------------------------------------------
		// OnInputFieldChange
		// -------------------------------------------------------------------------------
		public void OnInputFieldChange()
		{
			inputActive = true;
		}	
		
		// =============================== UPDATE HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnReceiveChatMessage
		// -------------------------------------------------------------------------------
		public void OnReceiveChatMessage(ChatMessage message)
		{
			
			if (contentViewport.childCount >= maxMessages)
			{
				for (int i = 0; i < maxMessages / 2; ++i)
                	Destroy(contentViewport.GetChild(i).gameObject);
			}
			
			GameObject go = Instantiate(chatEntryPrefab.gameObject, contentViewport.transform, true);
			
			go.GetComponent<UIChatSlot>().Init(message);
		
			Canvas.ForceUpdateCanvases();
       	 	scrollRect.verticalNormalizedPosition = 0;
        
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================