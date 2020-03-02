// =======================================================================================
// UIPopupInput
// by Weaver (Fhiz)
// MIT licensed
// 
// This popup provides your user with an input field for text or numeric values. It offers
// a way to Confirm or Cancel the process and each decision can result in a unique action.
// This class is universal and can be used anywhere you want your user to input a name,
// text or number.
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
	public partial class UIPopupInput : UIPopup
	{

		public static UIPopupInput singleton;
		
		protected Action<string> confirmAction;
		protected Action cancelAction;
		
		[SerializeField] protected InputField 	inputField;
		[SerializeField] protected Button 		confirmButton;
		[SerializeField] protected Button 		cancelButton;
		
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
		public void Init(string _description, Action<string> _confirmAction, Action _cancelAction=null, string _confirmText="", string _cancelText="")
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
			
			onInputChange();
			
			Show(_description);
		
		}
		
		// -------------------------------------------------------------------------------
		// onInputChange
		// -------------------------------------------------------------------------------
		public void onInputChange()
		{
			confirmButton.interactable = !String.IsNullOrWhiteSpace(inputField.text);
		}
		
		// -------------------------------------------------------------------------------
		// onClickConfirm
		// -------------------------------------------------------------------------------
		public override void onClickConfirm()
		{
			if (confirmAction != null)
				confirmAction(inputField.text);

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