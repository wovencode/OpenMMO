//BY FHIZ
//MODIFIED BY DX4D

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
		
		[Header("WINDOW LINKS")]
		public UIWindowPlayerSelect selectWindow;
		
		[Header("SLOT LINKS")]
		public UISelectPlayerSlot slotPrefab;

		[Header("BUTTON GROUP LINKS")]
		public UIButtonGroup buttonGroup;
		public Transform contentHolder;
		
		[Header("INPUT FIELD LINKS")]
		public InputField playernameInput;
		
		[Header("BUTTON LINKS")]
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

            //this.InvokeInstanceDevExtMethods(nameof(ThrottledUpdate)); //HOOK //REMOVED - DX4D
            // -- Available Players
            UpdatePlayerPrefabs(); //REFRESH PLAYER LIST
            UpdatePlayerIndex(); //ADDED - DX4D
            
            // -- Buttons
            createButton.interactable = (index > -1 && networkManager.CanRegisterPlayer(playernameInput.text) );
			createButton.onClick.SetListener(() => { OnClickCreate(); });
			
			backButton.onClick.SetListener(() => { OnClickBack(); });

		}

        /// <summary>
        /// Updates the available player prefabs to reflect changes.
        /// </summary>
        protected void UpdatePlayerPrefabs(bool forced = false)
        {
            if (contentHolder.childCount > 0 && !forced) return; //LIST IS ALREADY POPULATED?

            if (forced) //CLEAR EXISTING LIST - (ONLY WHEN FORCED - ALREADY EMPTY OTHERWISE)
            {
                for (int i = 0; i < contentHolder.childCount; i++)
                {
                    GameObject.Destroy(contentHolder.GetChild(i).gameObject);
                }
            }

            int _index = 0; //TEMP INDEX

            //POPULATE THE LIST
            foreach (GameObject player in networkManager.playerPrefabs)
            {
                GameObject go = GameObject.Instantiate(slotPrefab.gameObject);
                go.transform.SetParent(contentHolder.transform, false);

                //TODO: The second player.name should be player.prefabname - the current solution works for now
                //go.GetComponent<UISelectPlayerSlot>().Init(buttonGroup, _index, player.name, player.name, (_index == 0) ? true : false); //REMOVED DX4D
                go.GetComponent<UISelectPlayerSlot>().Init(buttonGroup, _index, player.name, player.name, (_index == index)); //ADDED DX4D
                _index++;
            }

            UpdatePlayerIndex(); //ADDED DX4D
            //index = 0; //REMOVED DX4D
        }

        // -------------------------------------------------------------------------------
        // UpdatePlayerIndex
        // -------------------------------------------------------------------------------
        protected void UpdatePlayerIndex()
		{
            Debug.Log("[CLIENT PLAYER CREATE] - Determining selected slot...");
			foreach (UIButton button in buttonGroup.buttons)
			{
				int _index = button.GetComponent<UISelectPlayerSlot>().GetIndex;
				if (_index > -1)
				{
                    index = _index;
                    Debug.Log("[CLIENT PLAYER CREATE] - Slot #" + index + " was selected!");
                    return;
				}
			}
			
            Debug.Log("[CLIENT PLAYER CREATE] - No selected slot!");
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