
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
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestPlayerLogin
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial Class <c>ClientMessageRequestPlayerLogin</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent player login request containing username and playername.
    /// </summary>
	public partial class ClientMessageRequestPlayerAutoLogin : ClientMessageRequest
	{
		public string username;
		public string playername;
	}
	
	// -------------------------------------------------------------------------------
	
}