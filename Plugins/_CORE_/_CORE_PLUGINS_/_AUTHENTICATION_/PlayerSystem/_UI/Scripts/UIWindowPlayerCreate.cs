
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIWindowPlayerCreate
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowPlayerCreate : UIRoot
	{
		
		[Header("Windows")]
		public UIWindowPlayerSelect selectWindow;
		
		[Header("Prefab")]
		public UISelectPlayerSlot slotPrefab;
		public UIButtonGroup buttonGroup;
		
		[Header("Content")]
		public Transform contentViewport;
		
		[Header("Input Fields")]
		public InputField playernameInput;
		
		[Header("Buttons")]
		public Button createButton;
		public Button backButton;
		
		protected int index = -1;
		
		public static UIWindowPlayerCreate singleton;
		
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
			playernameInput.text = "";
			base.Show();
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
        {
            playernameInput.text = Tools.TrimExcessWhitespace(playernameInput.text); //TRIM EXTRA WHITESPACE

            this.InvokeInstanceDevExtMethods(nameof(ThrottledUpdate)); //HOOK
			
			// -- Available Players
			UpdatePlayerIndex();
			
			// -- Buttons
			createButton.interactable = (index != -1 && networkManager.CanRegisterPlayer(playernameInput.text) );
			createButton.onClick.SetListener(() => { OnClickCreate(); });
			
			backButton.onClick.SetListener(() => { OnClickBack(); });
		
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
		// OnClickCreate
		// -------------------------------------------------------------------------------
		public void OnClickCreate()
		{	
		
			if (!networkManager.CanRegisterPlayer(playernameInput.text))
				return;
			
			string prefabName = networkManager.playerPrefabs[index].name;
			networkManager.TryRegisterPlayer(playernameInput.text, prefabName);
			selectWindow.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickBack
		// -------------------------------------------------------------------------------
		public void OnClickBack()
		{	
			selectWindow.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================