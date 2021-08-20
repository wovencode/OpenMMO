//BY DX4D

using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{
    [DisallowMultipleComponent]
    public partial class UIQuitGamePrompt : UIRoot
    {
        [Header("BUTTONS")]
        public Button yesButton;
        public Button noButton;

        [Header("LOGOUT TIMER")]
        [SerializeField] [Range(0f, 120f)] float logoutTimer = 10f;

        [Header("PROMPT")]
        public string popupDescription = "Quitting game in ";

        // -------------------------------------------------------------------------------
        // ThrottledUpdate
        // -------------------------------------------------------------------------------
        protected override void ThrottledUpdate()
        {
            yesButton.onClick.SetListener(() => { OnClickConfirm(); });
            noButton.onClick.SetListener(() => { OnClickCancel(); });
        }

        // C O N F I R M

        //ON CLICK CONFIRM
        protected void OnClickConfirm()
        {
            Hide();
            UIPopupCountdown.singleton.Init(popupDescription, logoutTimer, OnClickConfirmQuit, OnClickCancelQuit);
            //UIPopupPrompt.singleton.Init(popupDescription, OnClickConfirmLogout, OnClickCancelLogout);
        }

        //ON CLICK CONFIRM QUIT
        public void OnClickConfirmQuit()
        {
            Hide();
            QuitServer(networkManager);
        }
        //QUIT SERVER
        void QuitServer(Network.NetworkManager _manager)
        {
            _manager.Quit();
        }

        // C A N C E L

        //ON CLICK CANCEL
        public void OnClickCancel()
        {
            Hide();
            UIAccountMenu.singleton.Show();
        }

        //ON CLICK CANCEL QUIT
        public void OnClickCancelQuit()
        {
            Hide();
            UIAccountMenu.singleton.Show();
        }
    }
}
