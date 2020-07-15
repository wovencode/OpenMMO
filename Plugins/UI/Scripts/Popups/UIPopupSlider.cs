//by Fhiz
using OpenMMO;
using OpenMMO.UI;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	/// <summary>
    /// This popup provides a slider that is used to input a numeric value. The process can
	/// be confirmed and cancelled. Confirmation passes the sliders input value. This class
	/// is universal and can be used everywhere you want to prompt the user for numeric input.
    /// </summary>
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
		
		/// <summary>
    	/// Awake sets the singleton (as this popup is unique) and calls base.Awake
    	/// </summary>
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		/// <summary>
    	/// Initializes the popup by setting its texts, button actions and max slider value.
    	/// </summary>
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
		
		/// <summary>
    	/// Called when the minus button is pressed. This changes the slider.
    	/// </summary>
		public void onClickMinus()
		{
			slider.value--;
			onSliderChange();
		}
		
		/// <summary>
    	/// Called when the plus button is pressed. This changes the slider.
    	/// </summary>
		public void onClickPlus()
		{
			slider.value++;
			onSliderChange();
		}
		
		/// <summary>
    	/// Called when the slider changed to update the slider text.
    	/// </summary>
		public void onSliderChange()
		{
			sliderValueText.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
		}
		
		/// <summary>
    	/// Called when the Confirm button is pressed. This always closes the popup.
    	/// </summary>
		public override void onClickConfirm()
		{
			if (confirmAction != null)
				confirmAction(Convert.ToInt32(slider.value));

			Close();
		}
		
		/// <summary>
    	/// Called when the cancel button is pressed. This always closes the popup.
    	/// </summary>
		public override void onClickCancel()
		{
			if (cancelAction != null)
				cancelAction();

			Close();
		}
		
	}

}