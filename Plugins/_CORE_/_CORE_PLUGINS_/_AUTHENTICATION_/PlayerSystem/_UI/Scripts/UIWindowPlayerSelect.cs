//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.UI;
using System.Linq;

using OpenMMO.Network;
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
        //DEFAULT VALUES
        const byte DEFAULT_DELETE_COOLDOWN = 7;
        public const byte MAX_DELETE_COOLDOWN = byte.MaxValue - 1;
		
		[Header("WINDOW LINKS")]
		//public UIWindowAccountOptionsMenu mainWindow;
		public UIAccountMenu mainWindow;
		public UIWindowPlayerCreate createWindow;
		 
		[Header("SLOT LINKS")]
		public UISelectPlayerSlot slotPrefab;
		
		[Header("TEXT FIELD LINKS")]
		public Text textMaxPlayers;
		
		[Header("BUTTON GROUP LINKS")]
		public UIButtonGroup buttonGroup;
		public Transform contentHolder;

		[Header("BUTTON LINKS")]
		public Button selectButton;
		public Button createButton;
		public Button deleteButton;
		public Button menuButton;
		
		[Header("SYSTEM TEXT MESSAGES")]
		public string DELETE_CONFIRM_MESSAGE = "Do you really want to delete this character?";
		public string LOGIN_MESSAGE		= "Logging in, please wait...";
        /// <summary>Prefixed by the duration remaining on the timer.</summary>
        [Tooltip("Prefixed by the duration remaining on the timer")]
		public string DELETE_TIMER_MESSAGE = " seconds must pass before the next character deletion.";


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
            TickTimer(); //INCREMENT THE ACTIVATION TIMER
			
			// -- Max Players Test
			textMaxPlayers.text = networkManager.playerPreviews.Count.ToString() + " / " + networkManager.maxPlayers.ToString();
			
			// -- Available Players
			UpdatePlayerPreviews();
			UpdateSelectedIndex(); //REMOVED DX4D

            // -- Buttons

            //SELECT
            selectButton.interactable = (index > -1);
			selectButton.onClick.SetListener(() => { OnClickSelect(); });
			//CREATE
			createButton.interactable = networkManager.playerPreviews.Count < networkManager.maxPlayers;
			createButton.onClick.SetListener(() => { OnClickCreate(); });
		    //DELETE
			deleteButton.interactable = (index > -1);
			deleteButton.onClick.SetListener(() => { OnClickDelete(); });
            //LOGOUT
			menuButton.onClick.SetListener(() => { OnClickBack(); });
		
		}
		
		// -------------------------------------------------------------------------------
		// UpdatePlayerPreviews
		// -------------------------------------------------------------------------------
		public void UpdatePlayerPreviews(bool forced=false)
        {
            if (contentHolder.childCount > 0 && !forced) return; //LIST IS ALREADY POPULATED?
            //if (!forced && contentHolder.childCount > 0) return;

            if (forced) //CLEAR EXISTING LIST - (ONLY WHEN FORCED - ALREADY EMPTY OTHERWISE)
            {
                for (int i = 0; i < contentHolder.childCount; i++)
                {
                    GameObject.Destroy(contentHolder.GetChild(i).gameObject);
                }
            }

            int _index = 0; //TEMP INDEX
            //for (int i = 0; i < contentHolder.childCount; i++)
            //{
            //    GameObject.Destroy(contentHolder.GetChild(i).gameObject);
            //}
			
			//int _index = 0;
			
            buttonGroup.buttons.Clear(); //CLEAR BUTTON GROUP

#if DEBUG && UNITY_EDITOR //DEBUG
            System.Text.StringBuilder debugLog = new System.Text.StringBuilder("[UI PLAYER SELECT] - Loading Character Slots...");
#endif

            foreach (PlayerPreview player in networkManager.playerPreviews)
			{
				GameObject go = GameObject.Instantiate(slotPrefab.gameObject);
				go.transform.SetParent(contentHolder.transform, false);

				//go.GetComponent<UISelectPlayerSlot>().Init(buttonGroup, _index, player.playername, player.prefabname, (_index == 0) ? true : false); //REMOVED DX4D
                go.GetComponent<UISelectPlayerSlot>().Init(buttonGroup, _index, player.playername, player.prefabname, (_index == index)); //ADDED DX4D

#if DEBUG && UNITY_EDITOR //DEBUG
                debugLog.AppendLine("Slot #" + _index + " > " + ((_index == index) ? " (selected) " : " ") + go.name);
#endif

                _index++; //INCREMENT INDEX
			}

#if DEBUG && UNITY_EDITOR //DEBUG
            Debug.Log(debugLog.ToString());
#endif

            UpdateSelectedIndex(); //ADDED DX4D
            //index = 0; //REMOVED DX4D
		}
        void UpdateSelectedIndex()
        {
            index = GetSelectedIndex(); //ADDED DX4D
        }
        //GET SELECTED INDEX
        int GetSelectedIndex()
        {
            foreach (UIButton button in buttonGroup.buttons)
            {
                int _index = button.GetComponent<UISelectPlayerSlot>().GetIndex;
                if (_index > -1)
                {
                    return _index;
                }
            }

            return -1;
        }
				
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickSelect
		// -------------------------------------------------------------------------------
		public void OnClickSelect()
		{
			
			if (UIPopupNotify.singleton)
				UIPopupNotify.singleton.Init(LOGIN_MESSAGE, 10f, false);
        	
        	// -- issue login
			networkManager.TryLoginPlayer(networkManager.playerPreviews[index].playername);
			
			Hide();
		}
        
        //DELETE COOLDOWN
        /// <summary>How many ticks until the next delete is possible. Datatype: byte max=255
        /// <para>This feature exists to prevent any shenanigans with "rerolls" to get better starting gear etc.</para>
        /// <para>It is also a workaround to a bug in Host and Play mode where clicking buttons triggers twice (client+server)</para>
        /// </summary>
        [Header("DELETE COOLDOWN")]
        [SerializeField][Range(byte.MinValue, byte.MaxValue)] byte deleteCooldownDelay = DEFAULT_DELETE_COOLDOWN;

        /// <summary>Set to Max by default so that the first click will never be blocked.</summary>
        byte lastDeleteTimer = DEFAULT_DELETE_COOLDOWN; //NOTE: Set to byte.Max - 1

        //ON CLICK DELETE
        bool deletedCharacter = false;
        public void OnClickDelete()
		{
            if (CanDelete) //TIMER ELAPSED //NOTE: (>= is used as a failsafe, == would have worked as well)
            {
                if (deletedCharacter) { deletedCharacter = false; return; } //SKIP THIS PASS

                deletedCharacter = true;
                lastDeleteTimer = 0; //RESET TIMER
                UIPopupPrompt.singleton.Init(DELETE_CONFIRM_MESSAGE, OnClickConfirmDelete, OnClickCancelDelete);
                Hide();
            }
            else
            {
                if (!deletedCharacter) return; //NO TIMER MESSAGE BEFORE DELETING ONCE

                UIPopupNotify.singleton.Init
                    ( (deleteCooldownDelay - lastDeleteTimer) + DELETE_TIMER_MESSAGE, 1.2f, true);
            }

            //NOTE: The Timer is Incremented in ThrottledUpdate
        }
        //CAN DELETE
        bool CanDelete => (lastDeleteTimer >= deleteCooldownDelay);

        //TICK TIMER
        /// <summary>Triggered every time the <see cref="UIWindowPlayerSelect"/> window does a throttled update.</summary>
        void TickTimer()
        {
            //BUFFER OVERFLOW TRAP - Stops the timer from going past the max value
            if (lastDeleteTimer >= byte.MaxValue) lastDeleteTimer = 0;

            lastDeleteTimer++; //INCREMENT TIMER
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

#if _SERVER && _CLIENT //HOST MODE
        bool deleting = false;
#endif
        public void OnClickConfirmDelete()
        {
			Show();

            //PREVENT "DOUBLE DELETE BUG" IN HOST MODE
#if _SERVER && _CLIENT //HOST MODE
            if (deleting)
            {
                deleting = false;
                //UIPopupNotify.singleton.Init("Skipping the deletion of slot #" + index);
                return;
            }
            deleting = true;
#endif

            //DELETE PLAYER
            networkManager.TryDeletePlayer(networkManager.playerPreviews[index].playername);
        }
		
		// -------------------------------------------------------------------------------
		// OnClickCancelDelete
		// -------------------------------------------------------------------------------
		public void OnClickCancelDelete()
        {
            //deleting = false; //REMOVED DX4D
            Show();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================