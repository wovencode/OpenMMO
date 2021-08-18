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
    /// This popup offers the user with choices: Confirm or Cancel. Both can trigger a unique
	/// action so you can use it for A/B decisions as well. This class is universal and can be
	/// used anywhere you require the user to make a Yes/No decision.
    /// </summary>
	[DisallowMultipleComponent]
	public partial class UIPopupPrompt : UIPopup
	{

		public static UIPopupPrompt singleton;
		
		protected Action confirmAction;
		protected Action cancelAction;
		
		[SerializeField] protected Button confirmButton;
		[SerializeField] protected Text confirmText;
        [SerializeField] protected TMPro.TMP_Text confirmTextMesh;

        [SerializeField] protected Button cancelButton;
		[SerializeField] protected Text cancelText;
        [SerializeField] protected TMPro.TMP_Text cancelTextMesh;

        /// <summary>
        /// Awake sets the singleton (as this popup is unique) and calls base.Awake
        /// </summary>
        protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		/// <summary>
    	/// Initializes the popup by setting its texts and button actions.
    	/// </summary>
		public void Init(string _description, Action _confirmAction, Action _cancelAction=null, string _confirmText="", string _cancelText="")
		{
			base.Init();
			
            InitializeConfirm(_confirmAction, _confirmText); //INIT CONFIRM
            InitializeCancel(_cancelAction, _cancelText); //INIT CANCEL

			Show(_description);
		}
        //INIT CONFIRM
        void InitializeConfirm(Action _confirmAction, string _confirmText)
        {
            InitializeConfirmAction(_confirmAction);
            InitializeConfirmButton(_confirmText);
        }
        void InitializeConfirmAction(Action _confirmAction)
        {
            confirmAction = _confirmAction;
        }
        void InitializeConfirmButton(string _confirmText)
        {
            if (confirmButton)
            {
                confirmButton.onClick.SetListener(() => { onClickConfirm(); }); //ON CLICK LISTENER

                if (!String.IsNullOrWhiteSpace(_confirmText))
                {
                    if (confirmTextMesh != null) //TEXTMESH PRO
                    {
                        confirmTextMesh.text = _confirmText;
                    }
                    else if (confirmText != null) //REGULAR TEXT
                    {
                        confirmText.text = _confirmText;
                    }
                }
            }
        }
        //INIT CANCEL
        void InitializeCancel(Action _cancelAction, string _cancelText)
        {
            InitializeCancelAction(_cancelAction);
            InitializeCancelButton(_cancelText);
        }
        void InitializeCancelAction(Action _cancelAction)
        {
            cancelAction = _cancelAction;
        }
        void InitializeCancelButton(string _cancelText)
        {
            if (cancelButton)
            {
                cancelButton.onClick.SetListener(() => { onClickCancel(); }); //ON CLICK LISTENER

                if (!String.IsNullOrWhiteSpace(_cancelText))
                {
                    if (cancelTextMesh != null) //TEXTMESH PRO
                    {
                        cancelTextMesh.text = _cancelText;
                    }
                    else if (cancelText != null) //REGULAR TEXT
                    {
                        cancelText.text = _cancelText;
                    }
                }
            }
        }

        /// <summary>
        /// Called when the Confirm button is pressed.
        /// </summary>
        public override void onClickConfirm()
		{
			if (confirmAction != null)
				confirmAction();

			Close();
		}
		
		/// <summary>
    	/// Called when the Cancel button is pressed.
    	/// </summary>
		public override void onClickCancel()
		{
			if (cancelAction != null)
				cancelAction();

			Close();
		}

	}

}