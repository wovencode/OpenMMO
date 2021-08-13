//by Fhiz
using System.Collections.Generic;
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{

   	/// <summary>
    /// Player Switch Server Response Message
    /// </summary>
    /// <remarks>
    /// Sent from Server to Client
    /// </remarks>
    public partial struct ServerResponsePlayerSwitchServer : ServerResponse
    {
        public NetworkAction action => NetworkAction.PlayerSwitchServer;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }

        public string playername;
		public string anchorname;
		public string zonename;
		public int token;
	}
	
	/// <summary>
    /// Player Auto Login Response Message
    /// </summary>
    /// <remarks>
    /// Sent from Server to Client
    /// </remarks>
    public partial struct ServerResponsePlayerAutoLogin : ServerResponse
    {
        public NetworkAction action => NetworkAction.PlayerAutoLogin;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
	
	/// <summary>
    /// Auto Authentication Response Message
    /// </summary>
    /// <remarks>
    /// Sent from Server to Client
    /// </remarks>
    public partial struct ServerResponseAutoAuth : ServerResponse
    {
        public NetworkAction action => NetworkAction.PlayerAutoAuthenticate;

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
	
}