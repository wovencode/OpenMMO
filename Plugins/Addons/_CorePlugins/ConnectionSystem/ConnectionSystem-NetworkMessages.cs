//BY DX4D

#region N E T W O R K  M E S S A G E S
// ------------------------------

#region SERVER RESPONSE MESSAGES - HANDLED ON CLIENT
// @Server -> @Client
namespace OpenMMO.Network.Response
{
    /// <summary><para>
    /// Public Partial class <c>ServerMessageResponseAuth</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </para></summary>
    public partial struct Auth : ServerResponse
    {
        public NetworkAction action => NetworkAction.Authenticate;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
}
#endregion //SERVER RESPONSE

#region CLIENT REQUEST MESSAGES - HANDLED ON SERVER
// @Client -> @Server
namespace OpenMMO.Network.Request
{
    /// <summary><para>
    /// Public partial struct <c>ClientRequestAuth</c> inherits from <c>ClientRequest</c>. 
    /// Is used to authorize the client.
    /// Contains authorization request sent from client to server.
    /// </para></summary>
    public partial struct Auth : ClientConnectRequest
    {
        public NetworkAction action => NetworkAction.Authenticate;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _clientVersion;
        public string clientVersion { get { return _clientVersion; } set { _clientVersion = value; } }
    }
}
#endregion //CLIENT REQUESTS

#endregion //NETWORK MESSAGES
