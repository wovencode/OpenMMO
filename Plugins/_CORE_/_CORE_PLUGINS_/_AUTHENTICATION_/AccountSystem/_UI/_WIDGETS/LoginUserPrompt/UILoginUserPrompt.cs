//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OpenMMO.UI
{
    [DisallowMultipleComponent]
    public partial class UILoginUserPrompt : UIRoot
    {
        //STATUS BAR
        [Header("STATUS BAR")]
        [Tooltip("Assigning this will override the regular text field")]
        [SerializeField] TMP_Text statusTextMesh;
        [Tooltip("If the TextMesh Pro field is assigned, this is ignored")]
        [SerializeField] Text statusText;

        //USERNAME FIELD
        [Header("USERNAME FIELDS")]
        [Tooltip("Assigning this will override the regular input field")]
        [SerializeField] TMP_InputField usernameInputTextMesh;
        [Tooltip("If the TextMesh Pro field is assigned, this is ignored")]
        [SerializeField] InputField usernameInputText;

        //PASSWORD FIELD
        [Header("PASSWORD FIELDS")]
        [Tooltip("Assigning this will override the regular input field")]
        [SerializeField] TMP_InputField passwordInputTextMesh;
        [Tooltip("If the TextMesh Pro field is assigned, this is ignored")]
        [SerializeField] InputField passwordInputText;

        //BUTTONS
        [Header("BUTTONS")]
        [SerializeField] Button loginButton;
        [SerializeField] Button backButton;

        //OPTIONS
        [Header("OPTIONS")]
        [SerializeField] bool rememberLoginCredentials;
        [Range(0, 9)] public float maxDelayDuration = 1;

        //TEXT MESH PRO
        bool textMeshProStatusBar => (statusTextMesh != null);
        bool textMeshProUsername => (usernameInputTextMesh != null);
        bool textMeshProPassword => (passwordInputTextMesh != null);
        
        //SINGLETON
        public static UILoginUserPrompt singleton;

        // I N I T I A L I Z E

        //SHOW
        public override void Show()
        {
            Logout(networkManager); //LOGOUT WHEN THIS PROMPT SHOWS
            base.Show();
        }
        //LOGOUT
        void Logout(Network.NetworkManager _manager)
        {
            _manager.TryLogoutUser();
        }
        //AWAKE
        protected override void Awake()
        {
            singleton = this;
            base.Awake();
        }
        //START
        void Start()
        {
            LoadSavedCredentials(); //LOAD FROM PLAYERPREFS
        }
        //LOAD SAVED CREDENTIALS
        void LoadSavedCredentials()
        {
            if (!rememberLoginCredentials) return;

            //USERNAME
            if (textMeshProUsername) //TEXT MESH PRO
            {
                if (PlayerPrefs.HasKey(Defines.Login.UserName))
                {
                    usernameInputTextMesh.text = PlayerPrefs.GetString(Defines.Login.UserName, "");
                }
                else
                {
                    usernameInputTextMesh.text = "";
                }
            }
            else //REGULAR TEXT
            {
                if (PlayerPrefs.HasKey(Defines.Login.UserName))
                {
                    usernameInputText.text = PlayerPrefs.GetString(Defines.Login.UserName, "");
                }
                else
                {
                    usernameInputText.text = "";
                }
            }

            //PASSWORD
            if (textMeshProPassword) //TEXT MESH PRO
            {
                if (PlayerPrefs.HasKey(Defines.Login.Password))
                {
                    passwordInputTextMesh.text = PlayerPrefs.GetString(Defines.Login.Password, "");
                }
                else
                {
                    passwordInputTextMesh.text = "";
                }
            }
            else //REGULAR TEXT
            {
                if (PlayerPrefs.HasKey(Defines.Login.Password))
                {
                    passwordInputText.text = PlayerPrefs.GetString(Defines.Login.Password, "");
                }
                else
                {
                    passwordInputText.text = "";
                }
            }
        }

        // U P D A T E

        //THROTTLED UPDATE
        protected override void ThrottledUpdate()
        {
            ValidateInputFields(); //IS VALID USERNAME/PASSWORD?

            loginButton.onClick.SetListener(() => { OnClickLogin(); }); //LOGIN
            backButton.onClick.SetListener(() => { OnClickBack(); }); //BACK
        }

        // V A L I D A T E

        //VALIDATE INPUTS
        void ValidateInputFields()
        {
            //GET CREDENTIALS
            string _usernameInput = (textMeshProUsername) ? usernameInputTextMesh.text : usernameInputText.text;
            string _passwordInput = (textMeshProPassword) ? passwordInputTextMesh.text : passwordInputText.text;

            //VALIDATE LOGIN CREDENTIALS
            ValidateLoginCredentials(_usernameInput, _passwordInput);

            //ENABLE LOGIN BUTTON
            loginButton.interactable = networkManager.CanLoginUser(_usernameInput, _passwordInput);
        }
        //VALIDATE LOGIN CREDENTIALS
        void ValidateLoginCredentials(string _username, string _password)
        {
            string _statusMessage = "";

            //VALID USERNAME?
            if (!Tools.IsAllowedName(_username))
            {
                _statusMessage += "|Invalid Username|";
            }
            //VALID PASSWORD?
            if (!Tools.IsAllowedPassword(_password))
            {
                _statusMessage += "|Invalid Password|";
            }

            //CHANGE STATUS MESSAGE
            if (textMeshProStatusBar && statusTextMesh.text != _statusMessage)
            {
                statusTextMesh.text = _statusMessage; //TEXTMESH PRO
            }
            else if (statusText.text != _statusMessage)
            {
                statusText.text = _statusMessage; //TEXT
            }
        }

        // B U T T O N  C L I C K  E V E N T S

        //ON CLICK BACK BUTTON
        public void OnClickBack()
        {
            //UIWindowAccountOptionsMenu.singleton.Show();
            UIAccountMenu.singleton.Show();
            Hide();
        }
        //ON CLICK LOGIN BUTTON EVENT
        public void OnClickLogin()
        {
            //SAVE CREDENTIALS TO PLAYERPREFS
            if (rememberLoginCredentials)
            {
                //GET CREDENTIALS
                string _usernameInput = (textMeshProUsername) ? usernameInputTextMesh.text : usernameInputText.text;
                string _passwordInput = (textMeshProPassword) ? passwordInputTextMesh.text : passwordInputText.text;

                //SAVE CREDENTIALS
                PlayerPrefs.SetString(Defines.Login.UserName, _usernameInput);
                PlayerPrefs.SetString(Defines.Login.Password, _passwordInput);
            }

            //INVOKE ON EXECUTE LOGIN EVENT
            Invoke(nameof(OnExecuteLogin), UnityEngine.Random.Range(maxDelayDuration / 4, maxDelayDuration));

            //DISABLE LOGIN BUTTON
            loginButton.interactable = false;
            //DISABLE USERNAME FIELDS
            if (textMeshProUsername) { usernameInputTextMesh.interactable = false; }
            else { usernameInputText.interactable = false; }
            //DISABLE PASSWORD FIELDS
            if (textMeshProPassword) { passwordInputTextMesh.interactable = false; }
            else { passwordInputText.interactable = false; }
        }

        // L O G I N  E V E N T S

        //ON EXECUTE LOGIN EVENT
        protected void OnExecuteLogin()
        {
            //GET CREDENTIALS
            string _usernameInput = (textMeshProUsername) ? usernameInputTextMesh.text : usernameInputText.text;
            string _passwordInput = (textMeshProPassword) ? passwordInputTextMesh.text : passwordInputText.text;

            //ATTEMPT LOGIN
            AttemptLogin(_usernameInput, _passwordInput);

            //ENABLE BUTTONS
            EnableButtons();
        }
        //ATTEMPT USER LOGIN
        void AttemptLogin(string _username, string _password)
        {
            networkManager.TryLoginUser(_username, _password);
        }
        //ENABLE BUTTONS
        void EnableButtons()
        {
            //ENABLE LOGIN BUTTON
            loginButton.interactable = true;
            //ENABLE USERNAME FIELD
            if (textMeshProUsername) { usernameInputTextMesh.interactable = true; }
            else { usernameInputText.interactable = true; }
            //ENABLE USERNAME FIELD
            if (textMeshProPassword) { passwordInputTextMesh.interactable = true; }
            else { passwordInputText.interactable = true; }
        }
    }
}
