//BY DX4D

using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{
    [RequireComponent(typeof(UIRoot))]
    public partial class UIShowWidgetButton : MonoBehaviour
    {
        //[Header("PARENT WIDGET LINK")]
        [HideInInspector][ReadOnly][SerializeField] UIRoot parentWidget;
        [HideInInspector][ReadOnly][SerializeField][Range(0.01f, 3f)] float updateInterval = 1f;
        float fInterval = 0.01f;

        //[Header("UI LINKS")]
        [SerializeField] UIRoot targetWidget;
        [SerializeField] Button linkedButton;
        

        //ON VALIDATE
        private void OnValidate()
        {
            if (!parentWidget)
            {
                parentWidget = GetComponent<UIRoot>();
                updateInterval = parentWidget.updateInterval;
            }
        }

        /// <summary>
        /// Update is called every frame. Throttled update is only called in certain intervals to lessen client load. Protected so that child class can still use it (it's sometimes required to gather user input).
        /// </summary>
        protected void Update()
        {
            if (Time.time > fInterval)
            {
                ThrottledUpdate();
                fInterval = Time.time + updateInterval;
            }
        }
        // -------------------------------------------------------------------------------
        // ThrottledUpdate
        // -------------------------------------------------------------------------------
        protected void ThrottledUpdate()
        {
            UpdateShowWidgetButton();
        }

        //UPDATE SHOW WIDGET BUTTON
        void UpdateShowWidgetButton()
        {
            linkedButton.interactable = parentWidget.CanClick();
            linkedButton.onClick.SetListener(() => { OnClickShowWidget(); });
        }
        //ON CLICK SHOW WIDGET
        public void OnClickShowWidget()
        {
            ShowWidget();
            parentWidget.Hide();
        }
        //SHOW WIDGET
        void ShowWidget()
        {
            ShowWindow(targetWidget);
        }

        // S Y S T E M  F U N C T I O N S
        //SHOW WINDOW
        void ShowWindow(UIBase _window)
        {
            _window.Show();
        }

        //LOGOUT
        //void Logout(Network.NetworkManager _manager)
        //{
        //    _manager.TryLogoutUser();
        //}
    }
}
