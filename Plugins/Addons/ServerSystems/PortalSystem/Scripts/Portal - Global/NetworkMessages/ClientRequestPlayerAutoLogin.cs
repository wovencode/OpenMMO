//BY FHIZ
//MODIFIED BY DX4D

namespace OpenMMO.Network
{
	/// <summary>Auto Login Player Request Message</summary>
    /// <remarks>Sent from Client to Server</remarks>
	public partial struct ClientRequestZoneAutoLogin : ClientRequest
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        public string username;
		public string playername;
		public int token;
	}
}