
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequest
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequest : MessageBase
	{
		public bool success;
		public string text;
	}
	
	// ================================= MESSAGES AUTH ===================================
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestAuth
	// Unauthorized -> results in Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestAuth : ClientMessageRequest
	{
		public string clientVersion;
	}
	
	// ================================= MESSAGES USER ===================================
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestUserLogin
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestUserLogin : ClientMessageRequest
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestUserRegister
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestUserRegister : ClientMessageRequest
	{
		public string username;
		public string password;
		public string email;
		public string deviceid;
	}
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestUserDelete
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestUserDelete : ClientMessageRequest
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestUserChangePassword
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestUserChangePassword : ClientMessageRequest
	{
		public string username;
		public string oldPassword;
		public string newPassword;
	}
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestUserConfirm
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestUserConfirm : ClientMessageRequest
	{
		public string username;
		public string password;
	}
	
	// ================================ MESSAGES PLAYER ==================================
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestPlayerLogin
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestPlayerLogin : ClientMessageRequest
	{
		public string username;
		public string playername;
	}
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestPlayerRegister
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestPlayerRegister : ClientMessageRequest
	{
		public string username;
		public string playername;
		public string prefabname; // represents the character class
	}
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestPlayerDelete
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestPlayerDelete : ClientMessageRequest
	{
		public string username;
		public string playername;
	}
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequestPlayerSwitchServer
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ClientMessageRequestPlayerSwitchServer : ClientMessageRequest
	{
		public string username;
		public string playername;
		public int zoneIndex;
		public int token;
	}
	
	// -------------------------------------------------------------------------------
	
}