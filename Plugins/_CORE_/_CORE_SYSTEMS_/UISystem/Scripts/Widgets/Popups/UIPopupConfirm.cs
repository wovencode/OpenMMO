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
    /// This simple popup displays a short message to your user and asks for a button click in
	/// order to confirm and hide the popup. You can add an action to the button as well, but
	/// the popup hides even if there is no action associated. This class is universal and can
	/// be used to display small pieces of information to your user, that require attention and
	/// confirmation.
    /// </summary>
	[DisallowMultipleComponent]
	public partial class UIPopupConfirm : UIPopup
	{

		public static UIPopupConfirm singleton;
		
		protected Action confirmAction;
		
		[SerializeField] protected Button confirmButton;
		
		/// <summary>
    	/// Awake sets the singleton (as this popup is unique) and calls base.Awake
    	/// </summary>
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		/// <summary>
    	/// Initializes the popup by setting the button actions and optional texts. Then shows the popup.
    	/// </summary>
		public void Init(string _description, Action _confirmAction=null, string _confirmText="")
		{
			
			base.Init();
			
			confirmAction = _confirmAction;
			
            //SET CONFIRM BUTTON LISTENER
			if (confirmButton) confirmButton.onClick.SetListener(() => { onClickConfirm(); }); //SET LISTENER
				
			if (confirmButton && confirmButton.GetComponent<Text>() != null && !String.IsNullOrWhiteSpace(_confirmText))
				confirmButton.GetComponent<Text>().text = _confirmText;
			
			Show(_description);
		
		}
		
		/// <summary>
    	/// Called when the Confirm button is pressed to execute the action that it is set to.
    	/// </summary>
		public override void onClickConfirm()
		{
			if (confirmAction != null)
				confirmAction();

			Close();
		}
		
	}

}