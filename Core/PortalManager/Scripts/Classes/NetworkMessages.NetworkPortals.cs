
using System.Collections.Generic;
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{
    // -----------------------------------------------------------------------------------
    // ServerMessageResponsePlayerSwitchServer
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponsePlayerSwitchServer</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial class ServerMessageResponsePlayerSwitchServer : ServerMessageResponse
	{
		public string playername;
		public string anchorname;
		public string zonename;
	}
	
	// -------------------------------------------------------------------------------
	
}