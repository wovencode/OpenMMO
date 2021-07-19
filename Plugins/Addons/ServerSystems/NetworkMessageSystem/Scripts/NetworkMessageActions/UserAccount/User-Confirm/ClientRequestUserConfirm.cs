//BY FHIZ

namespace OpenMMO.Network
{
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