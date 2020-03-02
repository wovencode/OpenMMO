// =======================================================================================
// UIPopupSlider
// by Weaver (Fhiz)
// MIT licensed
//
// This popup provides a slider that is used to input a numeric value. The process can
// be confirmed and cancelled. Confirmation passes the sliders input value. This class
// is universal and can be used everywhere you want to prompt the user for numeric input.
//
// =======================================================================================

using OpenMMO;
using OpenMMO.UI;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIPopupInput
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIPopupSlider : UIPopup
	{

		public static UIPopupSlider singleton;
		
		protected Action<long> confirmAction;
		protected Action cancelAction;
		
		[SerializeField] protected Slider 	slider;
		[SerializeField] protected Text 	sliderValueText;
		[SerializeField] protected Button 	confirmButton;
		[SerializeField] protected Button 	cancelButton;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		// -------------------------------------------------------------------------------
		// Init
		// -------------------------------------------------------------------------------
		public void Init(string _description, Action<long> _confirmAction, Action _cancelAction=null, long _maxValue=100, string _confirmText="", string _cancelText="")
		{
			
			base.Init();
			
			confirmAction 	= _confirmAction;
			cancelAction 	= _cancelAction;
			
			if (confirmButton)
				confirmButton.onClick.SetListener(() => { onClickConfirm(); });
				
			if (confirmButton && confirmButton.GetComponent<Text>() != null && !String.IsNullOrWhiteSpace(_confirmText))
				confirmButton.GetComponent<Text>().text = _confirmText;
				
			if (cancelButton)
				cancelButton.onClick.SetListener(() => { onClickCancel(); });
			
			if (cancelButton && cancelButton.GetComponent<Text>() != null && !String.IsNullOrWhiteSpace(_cancelText))
				cancelButton.GetComponent<Text>().text = _cancelText;
			
			slider.value 			= 0;
			slider.maxValue 		= _maxValue;
					
			onSliderChange();
			
			Show(_description);
		
		}
		
		// -------------------------------------------------------------------------------
		public void onClickMinus()
		{
			slider.value--;
			onSliderChange();
		}
		
		// -------------------------------------------------------------------------------
		public void onClickPlus()
		{
			slider.value++;
			onSliderChange();
		}
		
		// -------------------------------------------------------------------------------
		public void onSliderChange()
		{
			sliderValueText.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
		}
		
		// -------------------------------------------------------------------------------
		// onClickConfirm
		// -------------------------------------------------------------------------------
		public override void onClickConfirm()
		{
			if (confirmAction != null)
				confirmAction(Convert.ToInt32(slider.value));

			Close();
		}
		
		// -------------------------------------------------------------------------------
		// onClickCancel
		// -------------------------------------------------------------------------------
		public override void onClickCancel()
		{
			if (cancelAction != null)
				cancelAction();

			Close();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================