/* //DEPRECIATED
//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{
    [DisallowMultipleComponent]
    public partial class UIWindowAccountOptionsMenu : UIRoot
    {

        [Header("LOGIN PROMPTS")]
        public UILoginUserPrompt loginWindow;
        public UIRegisterUserPrompt registerWindow;

        [Header("LOGIN BUTTONS")]
        public Button loginButton;
        public Button registerButton;

        [Header("ACCOUNT OPTIONS PROMPTS")]
        public UIWindowChangePasswordUser changePasswordWindow;
        public UIWindowDeleteUser deleteWindow;

        [Header("ACCOUNT OPTIONS BUTTONS")]
        public Button changePasswordButton;
        public Button deleteButton;
        //public Button serverButton; //DEPRECIATED

        [Header("QUIT BUTTON")]
        public Button quitButton;

        public static UIWindowAccountOptionsMenu singleton;

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
            UpdateLoginButton();
            UpdateRegisterButton();
            UpdateChangePasswordButton();
            UpdateDeleteButton();
            UpdateQuitButton();
            //DEPRECIATED
			//serverButton.interactable = networkManager.CanStartServer();
			//serverButton.onClick.SetListener(() => { OnClickStartServer(); });
		    
        }

        //UPDATE LOGIN BUTTON
        void UpdateLoginButton()
        {
            loginButton.interactable = networkManager.CanClick();
            loginButton.onClick.SetListener(() => { OnClickLogin(); });
        }
        //ON CLICK LOGIN
        public void OnClickLogin()
        {
            LoginUser();
            Hide();
        }
        //LOGIN USER
        void LoginUser()
        {
            ShowWindow(loginWindow);
        }

        //UPDATE REGISTER BUTTON
        void UpdateRegisterButton()
        {
            registerButton.interactable = networkManager.CanClick();
            registerButton.onClick.SetListener(() => { OnClickRegister(); });
        }
        //ON CLICK REGISTER
        public void OnClickRegister()
        {
            RegisterUser();
            Hide();
        }
        //REGISTER USER
        void RegisterUser()
        {
            ShowWindow(registerWindow);
        }

        // A C C O U N T  M E N U

        //UPDATE CHANGE PASSWORD BUTTONS
        void UpdateChangePasswordButton()
        {
            changePasswordButton.interactable = networkManager.CanClick();
            changePasswordButton.onClick.SetListener(() => { OnClickChangePassword(); });
        }
        //ON CLICK CHANGE PASSWORD
        public void OnClickChangePassword()
        {
            ChangePassword();
            Hide();
        }
        //CHANGE PASSWORD
        void ChangePassword()
        {
            ShowWindow(changePasswordWindow);
        }

        //UPDATE DELETE BUTTON
        void UpdateDeleteButton()
        {
            deleteButton.interactable = networkManager.CanClick();
            deleteButton.onClick.SetListener(() => { OnClickDeleteUser(); });
        }
        //ON CLICK DELETE USER
        public void OnClickDeleteUser()
        {
            DeleteUser();
            Hide();
        }
        //DELETE USER
        void DeleteUser()
        {
            ShowWindow(deleteWindow);
        }

        // Q U I T  B U T T O N

        //UPDATE QUIT BUTTON
        void UpdateQuitButton()
        {
            quitButton.onClick.SetListener(() => { OnClickQuit(); });
        }

        // S Y S T E M  F U N C T I O N S
        //SHOW WINDOW
        void ShowWindow(UIBase _window)
        {
            _window.Show();
        }

        //LOGOUT
        void Logout(Network.NetworkManager _manager)
        {
            _manager.TryLogoutUser();
        }

        //START SERVER
        public void OnClickStartServer()
        {
            StartServer(networkManager);
            Hide();
        }
        void StartServer(Network.NetworkManager _manager)
        {
            _manager.TryStartServer();
        }

        //QUIT SERVER
        public void OnClickQuit()
        {
            QuitServer(networkManager);
            Hide();
        }
        void QuitServer(Network.NetworkManager _manager)
        {
            _manager.Quit();
        }
        

    }
}
*/