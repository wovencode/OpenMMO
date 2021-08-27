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
        [Tooltip("The prefabs to display are contained in RESOURCES/PLAYER by default")]
        [SerializeField] string resourcesSubFolder = "PLAYER";
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

        //INDEX
        protected int index;
        public int GetIndex
        {
            get { return (selected) ? index : -1; }
        }

        //SELECTED
        protected bool _selected;
        protected bool selected => _selected;
        void Select() { _selected = true; Debug.Log("[UI PLAYER SLOT] Selected slot #" + index); }
        void Deselect() { _selected = false; Debug.Log("[UI PLAYER SLOT] Deselected slot #" + index); }

        // -------------------------------------------------------------------------------
        // Init
        // -------------------------------------------------------------------------------
        public void Init(UIButtonGroup _buttonGroup, int _index, string _playername, string _prefabname, bool _selected = false)
        {
            //selected = _selected; //REMOVED DX4D
            if (_selected) Select(); //ADDED DX4D
            else Deselect(); //ADDED DX4D

            base.Init(_buttonGroup);

            index = _index; //SET INDEX
            
            //NAME TEXT
            if (nameTextMesh != null) nameTextMesh.text = _playername; //TEXT MESH PRO
            else if (nameText != null) nameText.text = _playername; //TEXT
            else Debug.LogWarning("[UI PLAYER SLOT]" + _playername + " did not have a name text box component attached...");

            //PREFAB TEXT
            //if (prefabTextMesh != null) prefabTextMesh.text = _prefabname; //TEXT MESH PRO
            //else if (prefabText != null) prefabText.text = _prefabname; //TEXT
            //else Debug.LogWarning(_playername + " did not have a prefab text box component attached...");

            //PREFAB ICON
            //if (string.IsNullOrWhiteSpace(_prefabname)) { _prefabname = _playername; }
            if (!string.IsNullOrWhiteSpace(_prefabname))
            {
                string _prefabPath = resourcesSubFolder + "/" + _prefabname;
                //Debug.Log(_prefabPath); //DEBUG

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
                    /* //TODO
                    GameObject spawnedCharacterPrefab = Instantiate(_prefab); //INSTANTIATE PREFAB
                    if (spawnedCharacterPrefab)
                    {
                        spawnedCharacterPrefab.name = spawnedCharacterPrefab.name.TrimEnd(" (Clone)".ToCharArray());
                        //TODO: SHOW CHARACTER PREVIEW
                    }*/
                }
                else Debug.Log("[UI PLAYER SLOT] - Prefab " + _prefabname + " could not be loaded for " + _playername);
            }
            else Debug.Log("[UI PLAYER SLOT] - Prefab name was blank " + _prefabname + " - Icon could not be loaded for " + _playername);

            //UPDATE CHECKBOX
            if (selected) checkboxIcon.sprite = checkedImage;
            else checkboxIcon.sprite = uncheckedImage;
        }

        //ON PRESSED
        public override void OnPressed(bool deselect = false)
        {
            //TOGGLE SELECTION
            if (selected)
            {
                Deselect(); //ADDED DX4D
                            //selected = false; //REMOVED DX4D

                if (checkboxIcon) checkboxIcon.sprite = uncheckedImage; //UNCHECK
            }
            else if (!deselect)
            {
                Select(); //ADDED DX4D
                          //selected = true; //REMOVED DX4D

                if (checkboxIcon) checkboxIcon.sprite = checkedImage; //CHECK
            }

            base.OnPressed(deselect);
        }
    }
}
