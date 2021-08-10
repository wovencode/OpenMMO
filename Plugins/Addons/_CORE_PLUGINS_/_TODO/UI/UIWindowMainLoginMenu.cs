//BY FHIZ

using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

    // ===================================================================================
    // UIWindowMainLoginMenu
    // ===================================================================================
    [DisallowMultipleComponent]
    public partial class UIWindowMainLoginMenu : UIRoot
    {

        [Header("Windows")]
        public UIWindowLoginUser loginWindow;
        public UIWindowRegisterUser registerWindow;
        public UIWindowChangePasswordUser changePasswordWindow;
        public UIWindowDeleteUser deleteWindow;

        [Header("Buttons")]
        public Button loginButton;
        public Button registerButton;
        public Button changePasswordButton;
        public Button deleteButton;
        //public Button serverButton; //DEPRECIATED
        public Button quitButton;

        public static UIWindowMainLoginMenu singleton;

        // -------------------------------------------------------------------------------
        // Awake
        // -------------------------------------------------------------------------------
        protected override void Awake()
        {
            singleton = this;
            base.Awake();
        }

        // -------------------------------------------------------------------------------
        // Show
        // -------------------------------------------------------------------------------
        public override void Show()
        {
            Logout(networkManager);
            base.Show();
        }

        // -------------------------------------------------------------------------------
        // ThrottledUpdate
        // -------------------------------------------------------------------------------
        protected override void ThrottledUpdate()
        {

            loginButton.interactable = networkManager.CanClick();
            loginButton.onClick.SetListener(() => { OnClickLogin(); });

            registerButton.interactable = networkManager.CanClick();
            registerButton.onClick.SetListener(() => { OnClickRegister(); });

            changePasswordButton.interactable = networkManager.CanClick();
            changePasswordButton.onClick.SetListener(() => { OnClickChangePassword(); });

            deleteButton.interactable = networkManager.CanClick();
            deleteButton.onClick.SetListener(() => { OnClickDeleteUser(); });

            /* //DEPRECIATED
			serverButton.interactable = networkManager.CanStartServer();
			serverButton.onClick.SetListener(() => { OnClickStartServer(); });
		    */

            quitButton.onClick.SetListener(() => { OnClickQuit(); });

        }

        // S Y S T E M  F U N C T I O N S
        //SHOW WINDOW
        void ShowWindow(UIBase _window)
        {
            _window.Show();
        }
        //START SERVER
        void Logout(Network.NetworkManager _manager)
        {
            _manager.TryLogoutUser();
        }
        //START SERVER
        void StartServer(Network.NetworkManager _manager)
        {
            _manager.TryStartServer();
        }
        //QUIT SERVER
        void QuitServer(Network.NetworkManager _manager)
        {
            _manager.Quit();
        }

        // B U T T O N  H A N D L E R S
        //LOGIN
        public void OnClickLogin()
        {
            ShowWindow(loginWindow);
            Hide();
        }
        //REGISTER
        public void OnClickRegister()
        {
            ShowWindow(registerWindow);
            Hide();
        }
        //CHANGE PASSWORD
        public void OnClickChangePassword()
        {
            ShowWindow(changePasswordWindow);
            Hide();
        }
        //DELETE USER
        public void OnClickDeleteUser()
        {
            ShowWindow(deleteWindow);
            Hide();
        }
        public void OnClickStartServer()
        {
            StartServer(networkManager);
            Hide();
        }
        public void OnClickQuit()
        {
            QuitServer(networkManager);
            Hide();
        }
    }
}
