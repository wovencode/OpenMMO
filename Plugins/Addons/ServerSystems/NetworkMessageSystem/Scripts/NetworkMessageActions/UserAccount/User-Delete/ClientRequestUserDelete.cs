//BY FHIZ

namespace OpenMMO.Network
{
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