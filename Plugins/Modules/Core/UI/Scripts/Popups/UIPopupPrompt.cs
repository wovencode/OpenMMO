// =======================================================================================
// UIPopupPrompt
// by Weaver (Fhiz)
// MIT licensed
//
// This popup offers the user with choices: Confirm or Cancel. Both can trigger a unique
// action so you can use it for A/B decisions as well. This class is universal and can be
// used anywhere you require the user to make a Yes/No decision.
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
	// UIPopupPrompt
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIPopupPrompt : UIPopup
	{

		public static UIPopupPrompt singleton;
		
		protected Action confirmAction;
		protected Action cancelAction;
		
		[SerializeField] protected Button confirmButton;
		[SerializeField] protected Button cancelButton;
		
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
		public void Init(string _description, Action _confirmAction, Action _cancelAction=null, string _confirmText="", string _cancelText="")
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
				
			Show(_description);
			
		}
		
		// -------------------------------------------------------------------------------
		// onClickConfirm
		// -------------------------------------------------------------------------------
		public override void onClickConfirm()
		{
			if (confirmAction != null)
				confirmAction();

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