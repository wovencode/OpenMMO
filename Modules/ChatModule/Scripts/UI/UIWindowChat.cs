using System;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using OpenMMO.Chat;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIWindowChat
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowChat : UIRoot
	{
		
		[Header("Window")]
		public GameObject windowRoot;
		
		[Header("Prefab")]
		public UIChatSlot slotPrefab;
		
		[Header("Content")]
		public Transform contentViewport;
		public ScrollRect scrollRect;
		
		[Header("Buttons")]
		public Button toggleSizeButton;
		public Image toggleButtonImage;
		public Button sendButton;
		
		[Header("Send Input Field")]
		public InputField sendInputField;
		
		[Header("Channel Buttons")]
		public Button publicChannelButton;
		public Button privateChannelButton;
		public Button guildChannelButton;
		public Button partyChannelButton;
		public Button infoChannelButton;
		
		[Header("Channel Id")]
		public string channelIdPublic 	= "public";
		public string channelIdPrivate 	= "private";
		public string channelIdGuild 	= "guild";
		public string channelIdParty 	= "party";
		public string channelIdInfo		= "info";
		
		[Header("Used Images")]
		public Sprite maximizedImage;
		public Sprite minimizedImage;
		
		public int maxMessages = 100;
		
		public static UIWindowChat singleton;
		
		protected string channelId = "public";
		
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
			base.Show();
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
		
			if (!networkManager || networkManager.state != NetworkState.Game)
				Hide();
			else
				Show();
		
			sendButton.interactable = !String.IsNullOrWhiteSpace(sendInputField.text);
			sendButton.onClick.SetListener(() 				=> { OnClickSendMessage(); });
			
			toggleSizeButton.onClick.SetListener(() 		=> { OnClickToggleSize(); });
			
			publicChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelPublic(); });
			privateChannelButton.onClick.SetListener(() 	=> { OnClickSwitchChannelPrivate(); });
			guildChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelGuild(); });
			partyChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelParty(); });
			infoChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelInfo(); });
			
		}
						
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickSendMessage
		// -------------------------------------------------------------------------------
		public void OnClickSendMessage()
		{	
			
			if (String.IsNullOrWhiteSpace(sendInputField.text))
				return;
			
			ChatManager.singleton.ClientChatSend(channelId, sendInputField.text);
			
			sendInputField.text = String.Empty;
			
		}
				
		// -------------------------------------------------------------------------------
		// OnClickMinimize
		// -------------------------------------------------------------------------------
		public void OnClickToggleSize()
		{	
			if (windowRoot.activeInHierarchy)
			{
				windowRoot.SetActive(false);
				toggleButtonImage.sprite = minimizedImage;
			}
			else
			{
				windowRoot.SetActive(true);
				toggleButtonImage.sprite = maximizedImage;
			}
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
			
			GameObject go = Instantiate(slotPrefab.gameObject, contentViewport.transform, true);
			
			go.GetComponent<UIChatSlot>().Init(message);
		
			Canvas.ForceUpdateCanvases();
       	 	scrollRect.verticalNormalizedPosition = 0;
        
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================