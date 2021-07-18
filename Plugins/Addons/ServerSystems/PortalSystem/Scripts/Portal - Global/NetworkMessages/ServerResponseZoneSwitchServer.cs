//BY FHIZ
//MODIFIED BY DX4D

namespace OpenMMO.Network
{
   	/// <summary>Player Switch Server Response Message</summary>
    /// <remarks>Sent from Server to Client</remarks>
    public partial struct ServerResponseZoneSwitchServer : ServerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

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
}