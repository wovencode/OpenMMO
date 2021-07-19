//BY FHIZ
//MIRROR UPDATE - VERSION v13 to v42.2.8 BY DX4D

namespace OpenMMO.Network
{
    // -----------------------------------------------------------------------------------
    // ClientRequestConnect
    // Unauthorized -> results in Authorization
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// <para>
    /// Public partial struct <c>ClientRequestConnect</c> inherits from <c>ClientRequest</c>. 
    /// Is used to authorize the client.
    /// Contains authorization request sent from client to server.
    /// </para>
    /// </summary>
    public partial struct ClientRequestConnect : ClientRequest
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string clientVersion;
	}
}