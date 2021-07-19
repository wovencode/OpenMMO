//BY FHIZ

namespace OpenMMO.Network
{
	// -----------------------------------------------------------------------------------
    // ClientRequestUserLogout
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    public partial struct ClientRequestUserLogout : ClientRequest
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }
    }
}