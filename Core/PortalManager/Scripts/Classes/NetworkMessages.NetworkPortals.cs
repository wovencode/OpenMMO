
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
    public partial class ServerMessageResponsePlayerSwitchServer : ServerMessageResponse
	{
		public string playername;
		public string anchorname;
		public string zonename;
	}
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestPlayerAutoLogin
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestPlayerAutoLogin : ClientMessageRequest
	{
		public string username;
		public string playername;
	}
	
	// -----------------------------------------------------------------------------------
    // ServerMessageResponsePlayerAutoLogin
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    public partial class ServerMessageResponsePlayerAutoLogin : ServerMessageResponse
	{
		
	}
	
	// -------------------------------------------------------------------------------
	
}