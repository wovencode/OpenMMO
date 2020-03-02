
using UnityEngine;
using UnityEngine.UI;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIButton
	// ===================================================================================
	public partial class UIButton : MonoBehaviour
	{
		
		public Button button;
		[Range(0,9)] public float delayDuration = 1;
		
		protected UIButtonGroup buttonGroup;
		protected bool _interactable;
		
		// -------------------------------------------------------------------------------
		public virtual void Init(UIButtonGroup _buttonGroup = null)
		{
			
			button.onClick.RemoveAllListeners();
			
			if (delayDuration <= 0 && _buttonGroup == null)
				return;
			
			buttonGroup = _buttonGroup;
			_interactable = button.interactable;
			
			if (buttonGroup)
				buttonGroup.Add(this);
			
			button.onClick.AddListener(() =>
			{
				if (buttonGroup)
					buttonGroup.OnPressed(this);
				else
					OnPressed();
			});
		
		}
		
		// -------------------------------------------------------------------------------
		public virtual void OnPressed(bool deselect=false)
		{
			button.interactable = false;
			Invoke(nameof(EnableAgain), delayDuration);
		}
		
		// -------------------------------------------------------------------------------
		public void EnableAgain()
		{
			button.interactable = _interactable;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================