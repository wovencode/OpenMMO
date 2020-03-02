// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
//
// 
//
// =======================================================================================

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UITooltipHandler
	// ===================================================================================
	public partial class UITooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{

		[TextArea(1, 40)]
		public string tooltipText = "";
		
		protected bool _tooltipActive;
		
		// -------------------------------------------------------------------------------
		// Init
		// -------------------------------------------------------------------------------
		protected void ShowToolTip()
		{
			HideTooltip();
			UITooltip.singleton.Show(tooltipText);
			UITooltip.singleton.transform.SetParent(transform.root, true);
			UITooltip.singleton.transform.SetAsLastSibling();
			UITooltip.singleton.transform.position = Input.mousePosition;
			_tooltipActive = true;
		}
		
		// -------------------------------------------------------------------------------
		// HideTooltip
		// -------------------------------------------------------------------------------
		protected void HideTooltip()
		{
			_tooltipActive = false;
			CancelInvoke(nameof(ShowToolTip));
			UITooltip.singleton.Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnPointerEnter
		// -------------------------------------------------------------------------------
		public void OnPointerEnter(PointerEventData d)
		{
			ShowToolTip();
		}
		
		// -------------------------------------------------------------------------------
		// OnPointerExit
		// -------------------------------------------------------------------------------
		public void OnPointerExit(PointerEventData d)
		{
			if (_tooltipActive)
				HideTooltip();
		}
		
		// -------------------------------------------------------------------------------
		// Update
		// constantly update the tooltip text (as long as its visible) because values might
		// change
		// -------------------------------------------------------------------------------
		protected void Update()
		{
			if (!_tooltipActive) return;
			UITooltip.singleton.UpdateTooltip(tooltipText);
			UITooltip.singleton.transform.position = Input.mousePosition;
		}
		
		// -------------------------------------------------------------------------------
		// OnDisable
		// -------------------------------------------------------------------------------
		protected void OnDisable()
		{
			if (_tooltipActive)
				HideTooltip();
		}
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
		protected void OnDestroy()
		{
			if (_tooltipActive)
				HideTooltip();
		}
		
	}
	
	// -----------------------------------------------------------------------------------
	
}

// =======================================================================================