
using UnityEngine;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;

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
        [Tooltip("Setting this overrides the regular text field below")]
		public TMP_Text nameTextMesh;
        [Tooltip("This will be overridden when the TextMesh pro field above is set")]
        public Text nameText;
        
		[Header("BUTTONS")]
		public Button buttonSelect;
		
		[Header("ICONS")]
		public Image imageSelected;
		public Sprite unselectedImage;
		public Sprite selectedImage;
		
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