
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIWindowPlayerSelect
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowPlayerSelect : UIRoot
	{
		
		[Header("Windows")]
		public UIWindowMain mainWindow;
		public UIWindowPlayerCreate createWindow;
		 
		[Header("Prefab")]
		public UISelectPlayerSlot slotPrefab;
		public UIButtonGroup buttonGroup;
		
		[Header("Content")]
		public Text textMaxPlayers;
		public Transform contentViewport;
		
		[Header("Buttons")]
		public Button createButton;
		public Button selectButton;
		public Button deleteButton;
		public Button backButton;
		
		[Header("System Texts")]
		public string popupDescription = "Do you really want to delete this character?";
		public string popupLogin		= "Logging in, please wait...";
		
		public static UIWindowPlayerSelect singleton;
		
		protected int index = -1;
		
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
			UpdatePlayerPreviews(true);
			base.Show();
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			// -- Max Players Test
			textMaxPlayers.text = networkManager.playerPreviews.Count.ToString() + " / " + networkManager.maxPlayers.ToString();
			
			// -- Available Players
			UpdatePlayerPreviews();
			UpdatePlayerIndex();
			
			// -- Buttons
			
			createButton.interactable = networkManager.playerPreviews.Count < networkManager.maxPlayers;
			createButton.onClick.SetListener(() => { OnClickCreate(); });
		
			selectButton.interactable = (index != -1);
			selectButton.onClick.SetListener(() => { OnClickSelect(); });
		
			deleteButton.interactable = (index != -1);
			deleteButton.onClick.SetListener(() => { OnClickDelete(); });
					
			backButton.onClick.SetListener(() => { OnClickBack(); });
		
		}
		
		// -------------------------------------------------------------------------------
		// UpdatePlayerPreviews
		// -------------------------------------------------------------------------------
		public void UpdatePlayerPreviews(bool forced=false)
		{
			
			if (!forced && contentViewport.childCount > 0)
				return;
				
			for (int i = 0; i < contentViewport.childCount; i++)
				GameObject.Destroy(contentViewport.GetChild(i).gameObject);
			
			int _index = 0;
			buttonGroup.buttons.Clear();
			
			foreach (PlayerPreview player in networkManager.playerPreviews)
			{
			
				GameObject go = GameObject.Instantiate(slotPrefab.gameObject);
				go.transform.SetParent(contentViewport.transform, false);

				go.GetComponent<UISelectPlayerSlot>().Init(buttonGroup, _index, player.name, (_index == 0) ? true : false);
				_index++;
			}
			
			index = 0;
		
		}
		
		// -------------------------------------------------------------------------------
		// UpdatePlayerIndex
		// -------------------------------------------------------------------------------
		protected void UpdatePlayerIndex()
		{
			
			foreach (UIButton button in buttonGroup.buttons)
			{
				int _index = button.GetComponent<UISelectPlayerSlot>().Index;
				if (_index != -1)
				{
					index = _index;
					return;
				}
			}
			
			index = -1;
			
		}
				
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickSelect
		// -------------------------------------------------------------------------------
		public void OnClickSelect()
		{
			
			if (UIPopupNotify.singleton)
				UIPopupNotify.singleton.Init(popupLogin, 10f, false);
        	
        	// -- issue login
			networkManager.TryLoginPlayer(networkManager.playerPreviews[index].name);
			
			Hide();
		}
				
		// -------------------------------------------------------------------------------
		// OnClickDelete
		// -------------------------------------------------------------------------------
		public void OnClickDelete()
		{	
			UIPopupPrompt.singleton.Init(popupDescription, OnClickConfirmDelete, OnClickCancelDelete);
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickCreate
		// -------------------------------------------------------------------------------
		public void OnClickCreate()
		{	
			createWindow.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickBack
		// -------------------------------------------------------------------------------
		public void OnClickBack()
		{	
			mainWindow.Show();
			Hide();
		}
		
		// ========================== EVENT CONFIRM HANDLERS =============================
		
		// -------------------------------------------------------------------------------
		// OnClickConfirmDelete
		// -------------------------------------------------------------------------------
		public void OnClickConfirmDelete()
		{
			networkManager.TryDeletePlayer(networkManager.playerPreviews[index].name);
			Show();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickCancelDelete
		// -------------------------------------------------------------------------------
		public void OnClickCancelDelete()
		{
			Show();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================