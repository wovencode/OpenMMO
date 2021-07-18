//BY FHIZ

namespace OpenMMO.Network
{
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
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string password;
	}
}