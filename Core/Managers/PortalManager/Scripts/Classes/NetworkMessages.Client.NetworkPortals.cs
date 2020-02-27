
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{

    // -----------------------------------------------------------------------------------
    // ClientMessageRequestPlayerSwitchServer
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial Class <c>ClientMessageRequestPlayerSwitchServer</c> inherits from <c>ClientMessageRequest</c>
    /// Sent from Client to Server
    /// Client sent pplayer switch server request containing playername and zonename
    /// </summary>
    public partial class ClientMessageRequestPlayerSwitchServer : ClientMessageRequest
	{
		public string playername;
		public string zonename;
		public string anchorname;
		public int token;
	}
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestPlayerAutoLogin
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestPlayerAutoLogin : ClientMessageRequest
	{
		public string username;
		public string playername;
		public int token;
	}
	
	// ================================= MESSAGES AUTH ===================================

    // -----------------------------------------------------------------------------------
    // ClientMessageRequestAutoAuth
    // Unauthorized -> results in Authorization
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    public partial class ClientMessageRequestAutoAuth : ClientMessageRequest
	{
		public string clientVersion;
	}

	
	// -------------------------------------------------------------------------------
	
}