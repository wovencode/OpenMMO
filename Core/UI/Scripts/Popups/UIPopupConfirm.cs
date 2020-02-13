// =======================================================================================
// UIPopupConfirm
// by Weaver (Fhiz)
// MIT licensed
//
// This simple popup displays a short message to your user and asks for a button click in
// order to confirm and hide the popup. You can add an action to the button as well, but
// the popup hides even if there is no action associated. This class is universal and can
// be used to display small pieces of information to your user, that require attention and
// confirmation.
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
	// UIPopupConfirm
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIPopupConfirm : UIPopup
	{

		public static UIPopupConfirm singleton;
		
		protected Action confirmAction;
		
		[SerializeField] protected Button confirmButton;
		
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
		public void Init(string _description, Action _confirmAction=null, string _confirmText="")
		{
			
			base.Init();
			
			confirmAction = _confirmAction;
			
			if (confirmButton)
				confirmButton.onClick.SetListener(() => { onClickConfirm(); });
				
			if (confirmButton && confirmButton.GetComponent<Text>() != null && !String.IsNullOrWhiteSpace(_confirmText))
				confirmButton.GetComponent<Text>().text = _confirmText;
			
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
		
	}

}

// =======================================================================================