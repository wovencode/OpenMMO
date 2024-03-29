//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.UI;

using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

    /// <summary>
    /// Partial base class for many kinds of UIButtons. Supports Button Groups.
    /// </summary>
    public partial class UIButton : MonoBehaviour
    {
        [Header("CLICK DELAY")]
        [Range(0, 9)] public float clickDelayDuration = 1f;

        [Header("BUTTON LINKS")]
        public Button button;

        [Header("BUTTON GROUP LINKS")]
        protected UIButtonGroup buttonGroup;

        //INTERACTABLE
        protected bool _interactable;

        /// <summary>
        /// Initializes the button and adds support for Button Group (if any).
        /// </summary>
        public virtual void Init(UIButtonGroup _buttonGroup = null)
        {
            button.onClick.RemoveAllListeners();

            if (clickDelayDuration <= 0 && _buttonGroup == null) return;

            buttonGroup = _buttonGroup;
            _interactable = button.interactable;

            if (buttonGroup) buttonGroup.Add(this);

            button.onClick.AddListener(() =>
            {
                if (buttonGroup) buttonGroup.OnPressed(this);
                else OnPressed();
            });
        }

        /// <summary>
        /// Called when the button is pressed to temporarily disable it in order to prevent click-spamming.
        /// </summary>
        public virtual void OnPressed(bool deselect = false)
        {
            button.interactable = false;
            Invoke(nameof(EnableAgain), clickDelayDuration);
        }

        /// <summary>
        /// Re-enables the button again after a certain amount of time.
        /// </summary>
        public void EnableAgain()
        {
            button.interactable = _interactable;
        }
    }
}
