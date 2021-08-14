//BY FHIZ
//MODIFIED BY DX4D

#region N E T W O R K  M E S S A G E S
// ------------------------------

#region S E R V E R  R E S P O N S E  M E S S A G E S - HANDLED ON CLIENT
// @Server -> @Client
namespace OpenMMO.Network.Response
{
    //LOGIN
    /// <summary>
    /// Public Partial class <c>ServerMessageResponsePlayerLogin</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial struct PlayerLoginResponse : ServerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
    //REGISTER
    /// <summary>
    /// Public Partial class <c>ServerMessageResponsePlayerRegister</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial struct PlayerRegisterResponse : ServerRegisterPlayerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }

        public string _playername;
        public string playername { get { return _playername; } set { _playername = value; } }
    }
    //DELETE
    /// <summary>
    /// Public Partial class <c>ServerMessageResponsePlayerDelete</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial struct PlayerDeleteResponse : ServerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
}
#endregion //SERVER RESPONSES

#region C L I E N T  R E Q U E S T  M E S S A G E S - HANDLED ON SERVER
// @Client -> @Server
namespace OpenMMO.Network.Request
{
    //LOGIN
    /// <summary>
    /// Public Partial struct <c>ClientRequestPlayerLogin</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent player login request containing username and playername.
    /// </summary>
    public partial struct PlayerLoginRequest : ClientLoginPlayerRequest
    {
        public NetworkAction action => NetworkAction.PlayerLogin;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _username;
        public string username { get { return _username; } set { _username = value; } }

        internal string _playername;
        public string playername { get { return _playername; } set { _playername = value; } }
    }
    //REGISTER
    /// <summary>
    /// Public Partial struct <c>ClientRequestPlayerRegister</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent player register request containing username, playername and prefabname.
    /// </summary>
    public partial struct PlayerRegisterRequest : ClientRegisterPlayerRequest
    {
        public NetworkAction action => NetworkAction.PlayerRegister;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _username;
        public string username { get { return _username; } set { _username = value; } }

        internal string _playername;
        public string playername { get { return _playername; } set { _playername = value; } }

        internal string _prefabname;
        public string prefabname { get { return _prefabname; } set { _prefabname = value; } }
    }
    //DELETE
    /// <summary>
    /// Public Partial struct <c>ClientRequestPlayerDelete</c> inherits from <c>ClientMessageRequest</c>
    /// Sent from Client to Server
    /// Client sent player register request containing username and playername
    /// </summary>
    public partial struct PlayerDeleteRequest : ClientDeletePlayerRequest
    {
        public NetworkAction action => NetworkAction.PlayerDelete;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal string _username;
        public string username { get { return _username; } set { _username = value; } }

        internal string _playername;
        public string playername { get { return _playername; } set { _playername = value; } }
    }
}
#endregion //CLIENT REQUESTS

#endregion //NETWORK MESSAGES
