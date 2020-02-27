
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
		public int token;
	}
	
	// -----------------------------------------------------------------------------------
    // ServerMessageResponsePlayerAutoLogin
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    public partial class ServerMessageResponsePlayerAutoLogin : ServerMessageResponse
	{
		
	}
	
	// ================================= MESSAGES AUTH ===================================

    // -----------------------------------------------------------------------------------
    // ServerMessageResponseAutoAuth
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    public partial class ServerMessageResponseAutoAuth : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	
}