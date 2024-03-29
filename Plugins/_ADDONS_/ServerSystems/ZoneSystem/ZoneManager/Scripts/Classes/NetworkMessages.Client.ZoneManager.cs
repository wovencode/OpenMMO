//by Fhiz
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{

   
    /// <summary>
    /// Switch Server Request Message
    /// </summary>
    /// <remarks>
    /// Sent from Client to Server
    /// </remarks>
    public partial struct ClientRequestPlayerSwitchServer : ClientRequest
    {
        public NetworkAction action => NetworkAction.PlayerSwitchServer;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string playername;
		public string zonename;
		public string anchorname;
		public int token;
	}
	
	/// <summary>
    /// Auto Login Player Request Message
    /// </summary>
    /// <remarks>
    /// Sent from Client to Server
    /// </remarks>
	public partial struct ClientRequestPlayerAutoLogin : ClientRequest
    {
        public NetworkAction action => NetworkAction.PlayerAutoLogin;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string playername;
		public int token;
	}
	
	/// <summary>
    /// Auto Authentication Request Message
    /// </summary>
    /// <remarks>
    /// Sent from Client to Server
    /// </remarks>
    public partial struct ClientRequestAutoAuth : ClientRequest
    {
        public NetworkAction action => NetworkAction.PlayerAutoAuthenticate;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string clientVersion;
	}

}