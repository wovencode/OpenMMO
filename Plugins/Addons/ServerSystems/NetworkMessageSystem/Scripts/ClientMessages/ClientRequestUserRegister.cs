//BY FHIZ

namespace OpenMMO.Network
{
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
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string password;
		public string email;
		public string deviceid;
	}
}