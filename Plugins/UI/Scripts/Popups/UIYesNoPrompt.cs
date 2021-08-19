//BY DX4D

using UnityEngine;
using System;
using UnityEngine.UI;

namespace OpenMMO.UI
{
    [DisallowMultipleComponent]
    public abstract class UIYesNoPrompt : UIPopup
    {
        [Header("CONFIRM BUTTON")]
        [SerializeField] protected Button confirmButton;
        [SerializeField] protected Text confirmButtonText;
        [SerializeField] protected TMPro.TMP_Text confirmButtonTextMesh;
        protected Action confirmButtonAction;

        [Header("CANCEL BUTTON")]
        [SerializeField] protected Button cancelButton;
        [SerializeField] protected Text cancelButtonText;
        [SerializeField] protected TMPro.TMP_Text cancelButtonTextMesh;
        protected Action cancelButtonAction;

        //INIT
        public void Init(string _description, Action _confirmAction, Action _cancelAction = null, string _confirmText = "YES", string _cancelText = "NO")
        {
            base.Init();

            InitializeConfirm(_confirmAction, _confirmText); //INIT CONFIRM
            InitializeCancel(_cancelAction, _cancelText); //INIT CANCEL

            Show(_description);
        }


        // C O N F I R M

        //INIT CONFIRM
        void InitializeConfirm(Action _confirmAction, string _confirmText)
        {
            InitializeConfirmAction(_confirmAction);
            InitializeConfirmButton(_confirmText);
        }
        //INIT CONFIRM ACTION
        void InitializeConfirmAction(Action _confirmAction)
        {
            confirmButtonAction = _confirmAction;
        }
        //INIT CONFIRM BUTTON TEXT
        void InitializeConfirmButton(string _confirmText)
        {
            if (confirmButton)
            {
                confirmButton.onClick.SetListener(() => { onClickConfirm(); }); //ON CLICK LISTENER

                if (!String.IsNullOrWhiteSpace(_confirmText))
                {
                    if (confirmButtonTextMesh != null) //TEXTMESH PRO
                    {
                        confirmButtonTextMesh.text = _confirmText;
                    }
                    else if (confirmButtonText != null) //REGULAR TEXT
                    {
                        confirmButtonText.text = _confirmText;
                    }
                }
            }
        }
        //ON CLICK CONFIRM
        public override void onClickConfirm()
        {
            confirmButtonAction?.Invoke(); //INVOKE CONFIRM ACTION
            Close(); //CLOSE PROMPT
        }


        // C A N C E L

        //INIT CANCEL
        void InitializeCancel(Action _cancelButtonAction, string _cancelButtonText)
        {
            InitializeCancelAction(_cancelButtonAction);
            InitializeCancelButton(_cancelButtonText);
        }
        //INIT CANCEL ACTION
        void InitializeCancelAction(Action _cancelButtonAction)
        {
            cancelButtonAction = _cancelButtonAction;
        }
        //INIT CANCEL BUTTON TEXT
        void InitializeCancelButton(string _cancelButtonText)
        {
            if (cancelButton)
            {
                cancelButton.onClick.SetListener(() => { onClickCancel(); }); //ON CLICK LISTENER

                if (!String.IsNullOrWhiteSpace(_cancelButtonText))
                {
                    if (cancelButtonTextMesh != null) //TEXTMESH PRO
                    {
                        cancelButtonTextMesh.text = _cancelButtonText;
                    }
                    else if (cancelButtonText != null) //REGULAR TEXT
                    {
                        cancelButtonText.text = _cancelButtonText;
                    }
                }
            }
        }
        //ON CLICK CANCEL
        public override void onClickCancel()
        {
            cancelButtonAction?.Invoke(); //INVOKE CANCEL ACTION
            Close(); //CLOSE PROMPT
        }
    }
}
