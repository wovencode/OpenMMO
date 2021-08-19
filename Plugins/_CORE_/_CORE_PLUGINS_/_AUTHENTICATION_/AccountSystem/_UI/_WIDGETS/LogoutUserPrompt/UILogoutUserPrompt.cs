//BY DX4D

using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{
    [DisallowMultipleComponent]
    public partial class UILogoutUserPrompt : UIRoot
    {
        [Header("BUTTONS")]
        public Button yesButton;
        public Button noButton;

        [Header("LOGOUT TIMER")]
        [SerializeField] [Range(0f, 120f)] float logoutTimer = 10f;

        [Header("PROMPT")]
        public string popupDescription = "";

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
            UIPopupCountdown.singleton.Init(popupDescription, logoutTimer, OnClickConfirmLogout, OnClickCancelLogout);
            //UIPopupPrompt.singleton.Init(popupDescription, OnClickConfirmLogout, OnClickCancelLogout);
        }

        //ON CLICK CONFIRM LOGOUT
        public void OnClickConfirmLogout()
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

        //ON CLICK CANCEL LOGOUT
        public void OnClickCancelLogout()
        {
            Hide();
            UIAccountMenu.singleton.Show();
        }
    }
}
