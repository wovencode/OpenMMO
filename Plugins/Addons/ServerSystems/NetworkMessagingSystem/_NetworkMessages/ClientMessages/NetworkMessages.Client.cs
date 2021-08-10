//MIRROR UPDATE - VERSION v13 to v42.2.8 BY DX4D

namespace OpenMMO.Network
{

    // -----------------------------------------------------------------------------------
    // ClientMessageRequest
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public partial struct <c>ClientMessageRequest</c> inherits from Mirror.MessageBase.
    /// Containts the message sent from client to server
    /// </summary>
    /*public partial interface ClientMessageRequest : NetworkMessage //DEPRECIATED - MIRROR UPDATE - MessageBase Replaced with NetworkMessage - DX4D
    {
        bool success { get; set; }
        string text { get; set; }
	}*/

    // ================================= MESSAGES AUTH ===================================
    /* //DEPRECIATED - DX4D
    // -----------------------------------------------------------------------------------
    // ClientMessageRequestAuth
    // Unauthorized -> results in Authorization
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// <para>
    /// Public partial struct <c>ClientRequestAuth</c> inherits from <c>ClientRequest</c>. 
    /// Is used to authorize the client.
    /// Contains authorization request sent from client to server.
    /// </para>
    /// </summary>
    public partial struct ClientRequestAuth : ClientRequest
    {
        public NetworkAction action => NetworkAction.Authenticate;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string clientVersion;
	}*/

    // ================================= MESSAGES USER ===================================

    // -----------------------------------------------------------------------------------
    // ClientMessageRequestUserLogin
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial struct <c>ClientRequestUserLogin</c> inherits from <c>ClientMessageRequest</c>. 
    /// Sent from Client to Server.
    /// Used to login the User. Contains username and password.
    /// </summary>
    public partial struct ClientRequestUserLogin : ClientRequest
    {
        public NetworkAction action => NetworkAction.UserLogin;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
    // ClientRequestUserLogout
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    public partial struct ClientRequestUserLogout : ClientRequest
    {
        public NetworkAction action => NetworkAction.UserLogout;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }
    }

    // -----------------------------------------------------------------------------------
    // ClientRequestUserRegister
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial struct <c>ClientRequestUserRegister</c> inherits from <c>ClientMessageRequest</c>. 
    /// Sent from Client to Server.
    /// Client sent Registartion request Contains username, password, email and deviceid.
    /// </summary>
    public partial struct ClientRequestUserRegister : ClientRequest
    {
        public NetworkAction action => NetworkAction.UserRegister;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string password;
		public string email;
		public string deviceid;
	}

    // -----------------------------------------------------------------------------------
    // ClientRequestUserDelete
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Pubic Partial struct <c>ClientRequestUserDelete</c> inherits from <c>ClientsMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent deletion request Contains username and password.
    /// </summary>
    public partial struct ClientRequestUserDelete : ClientRequest
    {
        public NetworkAction action => NetworkAction.UserDelete;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string password;
	}

    // -----------------------------------------------------------------------------------
    // ClientRequestUserChangePassword
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial struct <c>ClientRequestUserChangePassword</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent password change request contains username, old password and new the new password
    /// </summary>
    public partial struct ClientRequestUserChangePassword : ClientRequest
    {
        public NetworkAction action => NetworkAction.UserChangePassword;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string oldPassword;
		public string newPassword;
	}

    // -----------------------------------------------------------------------------------
    // ClientRequestUserConfirm
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial struct <c>ClientRequestUserConfirm</c> inherites from <c>ClientMessageRequest</c>.
    /// Sent from client to server
    /// Client sent user confirmation request containing username and password.
    /// </summary>
    public partial struct ClientRequestUserConfirm : ClientRequest
    {
        public NetworkAction action => NetworkAction.UserConfirm;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string password;
	}

    // ================================ MESSAGES PLAYER ==================================

    // -----------------------------------------------------------------------------------
    // ClientRequestPlayerLogin
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial struct <c>ClientRequestPlayerLogin</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent player login request containing username and playername.
    /// </summary>
    public partial struct ClientRequestPlayerLogin : ClientRequest
    {
        public NetworkAction action => NetworkAction.PlayerLogin;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string playername;
	}

    // -----------------------------------------------------------------------------------
    // ClientRequestPlayerRegister
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial struct <c>ClientRequestPlayerRegister</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent player register request containing username, playername and prefabname.
    /// </summary>
    public partial struct ClientRequestPlayerRegister : ClientRequest
    {
        public NetworkAction action => NetworkAction.PlayerRegister;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string playername;
		public string prefabname; // represents the character class
	}

    // -----------------------------------------------------------------------------------
    // ClientRequestPlayerDelete
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial struct <c>ClientRequestPlayerDelete</c> inherits from <c>ClientMessageRequest</c>
    /// Sent from Client to Server
    /// Client sent player register request containing username and playername
    /// </summary>
    public partial struct ClientRequestPlayerDelete : ClientRequest
    {
        public NetworkAction action => NetworkAction.PlayerDelete;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string playername;
	}
	
	// -------------------------------------------------------------------------------
	
}