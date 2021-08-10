//BY FHIZ
//MODIFIED BY DX4D

namespace OpenMMO.Network.Request
{
    //CONFIRM
    /// <summary>
    /// Public Partial struct <c>ClientRequestUserConfirm</c> inherites from <c>ClientMessageRequest</c>.
    /// Sent from client to server
    /// Client sent user confirmation request containing username and password.
    /// </summary>
    public partial struct UserConfirm : ClientConfirmUserRequest
    {
        public NetworkAction action => NetworkAction.UserConfirm;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _username;
        public string username { get { return _username; } set { _username = value; } }

        internal string _password;
        public string password { get { return _password; } set { _password = value; } }
    }
    //LOGIN
    /// <summary>
    /// Public Partial struct <c>ClientRequestUserLogin</c> inherits from <c>ClientMessageRequest</c>. 
    /// Sent from Client to Server.
    /// Used to login the User. Contains username and password.
    /// </summary>
    public partial struct UserLogin : ClientLoginUserRequest
    {
        public NetworkAction action => NetworkAction.UserLogin;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _username;
        public string username { get { return _username; } set { _username = value; } }

        internal string _password;
        public string password { get { return _password; } set { _password = value; } }
    }
    //LOGOUT
    public partial struct UserLogout : ClientLogoutUserRequest
    {
        public NetworkAction action => NetworkAction.UserLogout;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }
    }
    //REGISTER
    /// <summary>
    /// Public Partial struct <c>ClientRequestUserRegister</c> inherits from <c>ClientMessageRequest</c>. 
    /// Sent from Client to Server.
    /// Client sent Registartion request Contains username, password, email and deviceid.
    /// </summary>
    public partial struct UserRegister : ClientRegisterUserRequest
    {
        public NetworkAction action => NetworkAction.UserRegister;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _username;
        public string username { get { return _username; } set { _username = value; } }

        internal string _password;
        public string password { get { return _password; } set { _password = value; } }

        public string _email;
        public string email { get { return _email; } set { _email = value; } }

        public string _deviceid;
        public string deviceid { get { return _deviceid; } set { _deviceid = value; } }
    }
    //DELETE
    /// <summary>
    /// Pubic Partial struct <c>ClientRequestUserDelete</c> inherits from <c>ClientsMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent deletion request Contains username and password.
    /// </summary>
    public partial struct UserDelete : ClientDeleteUserRequest
    {
        public NetworkAction action => NetworkAction.UserDelete;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _username;
        public string username { get { return _username; } set { _username = value; } }

        internal string _password;
        public string password { get { return _password; } set { _password = value; } }
    }
    //CHANGE PASSWORD
    /// <summary>
    /// Public Partial struct <c>ClientRequestUserChangePassword</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent password change request contains username, old password and new the new password
    /// </summary>
    public partial struct UserChangePassword : ClientChangeUserPasswordRequest
    {
        public NetworkAction action => NetworkAction.UserChangePassword;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _username;
        public string username { get { return _username; } set { _username = value; } }

        internal string _oldPassword;
        public string oldPassword { get { return _oldPassword; } set { _oldPassword = value; } }
        
        internal string _newPassword;
        public string newPassword { get { return _newPassword; } set { _newPassword = value; } }
    }
}
