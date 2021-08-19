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
		[Header("AVATAR NAME")]
        [Tooltip("This will be overridden when the TextMesh pro field below is set")]
        [SerializeField] Text nameText;
        [Tooltip("Setting this overrides the regular text field above")]
		[SerializeField] TMP_Text nameTextMesh;
        
		[Header("BUTTONS")]
		[SerializeField] Button buttonSelect;
		
		[Header("ICONS")]
		[SerializeField] Sprite unselectedImage;
		[SerializeField] Sprite selectedImage;
		public Image imageSelected;
		
		protected int _index;
		protected bool selected;
		
		// -------------------------------------------------------------------------------
		// Init
		// -------------------------------------------------------------------------------
		public void Init(UIButtonGroup _buttonGroup, int index, string name, bool _selected=false)
		{
			
			selected = _selected;
			
			base.Init(_buttonGroup);
			
			_index = index;

            //NAME TEXT
            if (nameTextMesh != null) nameTextMesh.text = name; //TEXT MESH PRO
            else if (nameText != null) nameText.text = name; //TEXT
            else Debug.LogWarning(name + " did not have a text box component attached...");
			
			if (selected) imageSelected.sprite = selectedImage;
			else imageSelected.sprite = unselectedImage;
		}
		
		// -------------------------------------------------------------------------------
		// OnPressed
		// -------------------------------------------------------------------------------
		public override void OnPressed(bool deselect=false)
		{
		
			if (selected)
			{
				selected = false;
				
				if (imageSelected)
					imageSelected.sprite = unselectedImage;
				
			}
			else if (!deselect)
			{
				selected = true;
				
				if (imageSelected)
					imageSelected.sprite = selectedImage;
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