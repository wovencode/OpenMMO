//BY FHIZ
//MODIFIED BY DX4D

using System.Collections;
using System;
using OpenMMO;
using OpenMMO.UI;
using OpenMMO.Network;

using UnityEngine;
using UnityEngine.UI;
using TMPro; //TEXTMESH PRO

namespace OpenMMO.UI
{

    /// <summary>
    /// The base class all popup windows are derived from. It features a UIWindowBackground
    /// that hides all other UI elements underneath it (so the user can only interact with
    /// the popup while it is shown).
    /// Popup classes are partial but feature no DevExtension hooks as this impacts performance
    /// Instead, developers are encouraged to add their own components to the same UI element.
    /// </summary>
    public abstract partial class UIPopup : UIBase
    {

        [Header("COMPONENT LINKS")]
        [SerializeField] protected Animator animator;
        [Header("TRIGGER NAMES")]
        [SerializeField] protected string showTriggerName = "show";
        [SerializeField] protected string closeTriggerName = "close";

        [Header("DESCRIPTION")]
        [SerializeField] protected Text descriptionText;
        [SerializeField] protected TMP_Text descriptionTextMesh;

        [SerializeField] [ReadOnly] bool _showing = false;
        protected bool showing => _showing;
        protected bool _fadeIn = true;

        /// <summary>
        /// Calls base.Awake
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Shows the popup with the given text and optional fades in the black background (or shows that one immediately as well)
        /// </summary>
        public virtual void Show(string _text, bool fade = true)
        {
            if (descriptionTextMesh != null) { descriptionTextMesh.text = _text; }//TEXT MESH PRO
            else if (descriptionText != null) { descriptionText.text = _text; } //REGULAR TEXT
            else { Debug.LogWarning("UI Element " + name + " does not have a description text box assigned"); } //NO TEXT

            base.Show();
            Init(fade);
        }

        /// <summary>
        /// Initializes the window by setting the animation trigger and fading/showing the background.
        /// </summary>
        protected void Init(bool fade = true)
        {
            _showing = true; //SET SHOWING BOOL

            if (root.activeSelf) animator.SetTrigger(showTriggerName);

            if (UIBackgroundLayer.singleton != null)
            {

                _fadeIn = fade;

                if (fade) UIBackgroundLayer.singleton.FadeIn();
                else UIBackgroundLayer.singleton.BlackIn();
            }
        }

        /// <summary>
        /// Closes and hides the popup.
        /// </summary>
        public override void Hide()
        {
            Close();
            base.Hide();
        }

        /// <summary>
        /// Called when the Confirm button is clicked, always hides the popup again.
        /// </summary>
        public virtual void onClickConfirm()
        {
            Close();
            base.Hide();
        }

        /// <summary>
        /// Called when the Cancel button is clicked, always hides the popup again.
        /// </summary>
        public virtual void onClickCancel()
        {
            Close();
            base.Hide();
        }

        /// <summary>
        /// Closes the window by fading/hiding the black background and setting the animation trigger.
        /// </summary>
        public void Close()
        {
            _showing = false; //SET SHOWING BOOL

            if (UIBackgroundLayer.singleton != null)
            {
                if (_fadeIn) UIBackgroundLayer.singleton.FadeOut();
                else UIBackgroundLayer.singleton.BlackOut();
            }

            if (root.activeSelf) animator.SetTrigger(closeTriggerName);
        }
    }
}
