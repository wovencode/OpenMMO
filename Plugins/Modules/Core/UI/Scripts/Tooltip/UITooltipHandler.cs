//by Fhiz
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{

	/// <summary>
    /// Attach this script to any game object to enable a tooltip when hovered over it. Only works on objects inside a Canvas.
    /// </summary>
	public partial class UITooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{

		[TextArea(1, 40)]
		public string tooltipText = "";
		
		protected bool _tooltipActive;
		
		/// <summary>
    	/// When called, shows the tooltip at mouse position.
    	/// </summary>
		protected void ShowToolTip()
		{
			HideTooltip();
			UITooltip.singleton.Show(tooltipText);
			UITooltip.singleton.transform.SetParent(transform.root, true);
			UITooltip.singleton.transform.SetAsLastSibling();
			UITooltip.singleton.transform.position = Input.mousePosition;
			_tooltipActive = true;
		}
		
		/// <summary>
    	/// When called, immediately hides the tooltip.
    	/// </summary>
		protected void HideTooltip()
		{
			_tooltipActive = false;
			CancelInvoke(nameof(ShowToolTip));
			UITooltip.singleton.Hide();
		}
		
		/// <summary>
    	/// Show the tooltip, when the mouse pointer enters the object.
    	/// </summary>
		public void OnPointerEnter(PointerEventData d)
		{
			ShowToolTip();
		}
		
		/// <summary>
    	/// Hide the tooltip again, when the mouse pointer leaves the object.
    	/// </summary>
		public void OnPointerExit(PointerEventData d)
		{
			if (_tooltipActive)
				HideTooltip();
		}
		
		/// <summary>
    	/// Constantly update the tooltip text (as long as its visible) because values might change.
    	/// </summary>
		protected void Update()
		{
			if (!_tooltipActive) return;
			UITooltip.singleton.UpdateTooltip(tooltipText);
			UITooltip.singleton.transform.position = Input.mousePosition;
		}
		
		/// <summary>
    	/// Hide the tooltip immediately when the object itself is disabled.
    	/// </summary>
		protected void OnDisable()
		{
			if (_tooltipActive)
				HideTooltip();
		}
		
		/// <summary>
    	/// Hide the tooltip immediately when the object itself is destroyed.
    	/// </summary>
		protected void OnDestroy()
		{
			if (_tooltipActive)
				HideTooltip();
		}
		
	}
	
}