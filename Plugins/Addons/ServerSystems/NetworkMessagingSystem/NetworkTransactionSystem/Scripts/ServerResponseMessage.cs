//BY FHIZ
//MODIFIED BY DX4D
//UPDATED MIRROR VERSION v13 to v42.2.8 BY DX4D
using System.Collections.Generic;
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{
    // -----------------------------------------------------------------------------------
    // ServerResponseMessage
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// public partial struct <c>ServerResponseMessage</c> inherits <c>ServerResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial struct ServerResponseMessage : ServerResponse
    {
        public ServerResponseMessage(NetworkAction messageAction) : this()
        {
            this.action = messageAction;
        }

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