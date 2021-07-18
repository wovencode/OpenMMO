//BY FHIZ

namespace OpenMMO.Network
{
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
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string playername;
	}
}