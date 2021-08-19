//BY DX4D

using UnityEngine;
using UnityEngine.UI;
using System;

namespace OpenMMO.UI
{
    [DisallowMultipleComponent]
    public partial class UIPopupCountdown : UIPopup
    {
        [Header("BUTTONS")]
        [SerializeField] Button cancelButton;

        //SINGLETON
        public static UIPopupCountdown singleton;

        //ACTIONS
        protected Action countdownAction;
        protected Action cancelAction;

        //ON VALIDATE
        //private void OnValidate()
        //{
        //    if (cancelButton) cancelButton.onClick.SetListener(() => { OnClickCancel(); });
        //}

        //AWAKE
        protected override void Awake()
        {
            singleton = this;
            base.Awake();
        }

        //INIT
        public void Init(string _description, float _duration = 5, Action _countdownAction = null, Action _cancelAction = null, bool fade = true)
        {
            if (!String.IsNullOrWhiteSpace(_description)) _description += "\n";
            _description += "Logging out in " + _duration + " seconds...";
            countdownAction = _countdownAction;
            cancelAction = _cancelAction;
            Show(_description, fade);
            Invoke(nameof(ExecuteAction), _duration);
        }
        //EXECUTE ACTION
        void ExecuteAction() { countdownAction?.Invoke(); }

        // B U T T O N  C L I C K  H A N D L E R S

        //ON CLICK CANCEL
        public void OnClickCancel()
        {
            CancelCountdown();
            CancelAction();
            Hide();
        }
        //CANCEL ACTION
        void CancelAction() { cancelAction?.Invoke(); }
        //CANCEL COUNTDOWN
        void CancelCountdown()
        {
            CancelInvoke(nameof(countdownAction));
        }

        // S H U T D O W N

        //CANCEL INVOCATION ON DISABLE/DESTROY
        void OnDisable() { SafeDispose(); }
        void OnDestroy() { SafeDispose(); }
        //SAFE DISPOSE
        void SafeDispose()
        {
            CancelInvoke(nameof(countdownAction));
            CancelInvoke(nameof(cancelAction));
        }
    }
}
