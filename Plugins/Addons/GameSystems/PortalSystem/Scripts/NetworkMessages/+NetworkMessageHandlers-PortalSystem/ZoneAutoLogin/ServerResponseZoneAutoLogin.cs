//BY FHIZ
//MODIFIED BY DX4D

namespace OpenMMO.Network
{
	/// <summary>Player Auto Login Response Message</summary>
    /// <remarks>Sent from Server to Client</remarks>
    public partial struct ServerResponseZoneAutoLogin : ServerResponse
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