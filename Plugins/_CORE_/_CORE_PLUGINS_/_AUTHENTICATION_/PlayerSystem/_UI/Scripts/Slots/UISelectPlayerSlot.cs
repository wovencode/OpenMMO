//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OpenMMO.UI
{
	
	// ===================================================================================
	// UISelectPlayerSlot
	// ===================================================================================
	public partial class UISelectPlayerSlot : UIButton
	{
        [Header("PREFAB FOLDER")]
        [SerializeField] string playerPrefabFolderName = "PLAYER";
		[Header("ICONS")]
		[SerializeField] Sprite uncheckedImage;
		[SerializeField] Sprite checkedImage;
		
		[Header("TEXTBOX LINKS")]
        [Tooltip("This will be overridden when the TextMesh pro field below is set")]
        [SerializeField] Text nameText;
        [Tooltip("Setting this overrides the regular text field above")]
		[SerializeField] TMP_Text nameTextMesh;
        
		//[Header("PREFAB NAME (Race/Class/Etc)")]
        //[Tooltip("This will be overridden when the TextMesh pro field below is set")]
        //[SerializeField] Text prefabText;
        //[Tooltip("Setting this overrides the regular text field above")]
		//[SerializeField] TMP_Text prefabTextMesh;

		[Header("BUTTON LINKS")]
		[SerializeField] Button buttonSelect;
		
		[Header("IMAGE LINKS")]
        [SerializeField] Image characterIcon;
		[SerializeField] Image checkboxIcon;

		protected int _index;
		protected bool selected;
		
		// -------------------------------------------------------------------------------
		// Init
		// -------------------------------------------------------------------------------
		public void Init(UIButtonGroup _buttonGroup, int index, string _playername, string _prefabname, bool _selected=false)
		{
			
			selected = _selected;
			
			base.Init(_buttonGroup);
			
			_index = index;

            //NAME TEXT
            if (nameTextMesh != null) nameTextMesh.text = _playername; //TEXT MESH PRO
            else if (nameText != null) nameText.text = _playername; //TEXT
            else Debug.LogWarning(_playername + " did not have a name text box component attached...");

            //PREFAB TEXT
            //if (prefabTextMesh != null) prefabTextMesh.text = _prefabname; //TEXT MESH PRO
            //else if (prefabText != null) prefabText.text = _prefabname; //TEXT
            //else Debug.LogWarning(_playername + " did not have a prefab text box component attached...");

            //PREFAB ICON
            //if (string.IsNullOrWhiteSpace(_prefabname)) { _prefabname = _playername; }
            if (!string.IsNullOrWhiteSpace(_prefabname))
            {
                string _prefabPath = playerPrefabFolderName + "/" + _prefabname;
                Debug.Log(_prefabPath);

                GameObject _prefab = Resources.Load(_prefabPath, typeof(GameObject)) as GameObject; //INSTANTIATE PREFAB
                if (_prefab)
                {
                    //SET CHARACTER ICON
                    CharacterDescription _description = _prefab.GetComponent<CharacterDescription>();
                    if (_description)
                    {
                        characterIcon.sprite = _description.icon; //SET ICON
                    }

                    //SPAWN CHARACTER PREVIEW MODEL
                    /*
                    GameObject spawnedCharacterPrefab = Instantiate(_prefab); //INSTANTIATE PREFAB
                    if (spawnedCharacterPrefab)
                    {
                        spawnedCharacterPrefab.name = spawnedCharacterPrefab.name.TrimEnd(" (Clone)".ToCharArray());
                        //TODO: SHOW CHARACTER PREVIEW
                    }*/
                }
                else Debug.Log("[CLIENT] - Prefab " + _prefabname + " could not be loaded for " + _playername);
            }
            else Debug.Log("[CLIENT] - Prefab name was blank " + _prefabname + " - Icon could not be loaded for " + _playername);

            //UPDATE CHECKBOX
            if (selected) checkboxIcon.sprite = checkedImage;
			else checkboxIcon.sprite = uncheckedImage;
		}
		
		// -------------------------------------------------------------------------------
		// OnPressed
		// -------------------------------------------------------------------------------
		public override void OnPressed(bool deselect=false)
		{
		
			if (selected)
			{
				selected = false;
				
				if (checkboxIcon)
					checkboxIcon.sprite = uncheckedImage;
				
			}
			else if (!deselect)
			{
				selected = true;
				
				if (checkboxIcon)
					checkboxIcon.sprite = checkedImage;
			}
			
			base.OnPressed(deselect);
			
		}
		
		// -------------------------------------------------------------------------------
		// Index
		// -------------------------------------------------------------------------------
		public int Index
		{
			get { return (selected) ? _index : -1; }
		}
				
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================