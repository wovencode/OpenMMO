
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{
	
	// -----------------------------------------------------------------------------------
	// ClientMessageRequest
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
    /// <summary>
    /// Public partial class <c>ClientMessageRequest</c> inherits from Mirror.MessageBase.
    /// Containts the message sent from client to server
    /// </summary>
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
    /// <summary>
    /// <para>
    /// Public partial class <c>ClientMessageRequestAuth</c> inherits from <c>ClientMessageRequest</c>. 
    /// Is used to authorize the client.
    /// Contains authorization request sent from client to server.
    /// </para>
    /// </summary>
    public partial class ClientMessageRequestAuth : ClientMessageRequest
	{
		public string clientVersion;
	}

    // ================================= MESSAGES USER ===================================

    // -----------------------------------------------------------------------------------
    // ClientMessageRequestUserLogin
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial Class <c>ClientMessageRequestUSerLogin</c> inherits from <c>ClientMessageRequest</c>. 
    /// Sent from Client to Server.
    /// Used to login the User. Contains username and password.
    /// </summary>
    public partial class ClientMessageRequestUserLogin : ClientMessageRequest
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
    // ClientMessageRequestUserLogout
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    public partial class ClientMessageRequestUserLogout : ClientMessageRequest
	{
	
	}
	
    // -----------------------------------------------------------------------------------
    // ClientMessageRequestUserRegister
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial Class <c>ClientMessageRequestUserRegister</c> inherits from <c>ClientMessageRequest</c>. 
    /// Sent from Client to Server.
    /// Client sent Registartion request Contains username, password, email and deviceid.
    /// </summary>
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
    /// <summary>
    /// Pubic Partial Class <c>ClientMessageRequestUserDelete</c> inherits from <c>ClientsMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent deletion request Contains username and password.
    /// </summary>
	public partial class ClientMessageRequestUserDelete : ClientMessageRequest
	{
		public string username;
		public string password;
	}

    // -----------------------------------------------------------------------------------
    // ClientMessageRequestUserChangePassword
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial Class <c>ClientMessageRequestUserChangePassword</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent password change request contains username, old password and new the new password
    /// </summary>
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
    /// <summary>
    /// Public Partial Class <c>ClientMessageRequestUserConfirm</c> inherites from <c>ClientMessageRequest</c>.
    /// Sent from client to server
    /// Client sent user confirmation request containing username and password.
    /// </summary>
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
    /// <summary>
    /// Public Partial Class <c>ClientMessageRequestPlayerLogin</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent player login request containing username and playername.
    /// </summary>
	public partial class ClientMessageRequestPlayerLogin : ClientMessageRequest
	{
		public string username;
		public string playername;
	}

    // -----------------------------------------------------------------------------------
    // ClientMessageRequestPlayerRegister
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial Class <c>ClientMessageRequestPlayerRegister</c> inherits from <c>ClientMessageRequest</c>.
    /// Sent from Client to Server.
    /// Client sent player register request containing username, playername and prefabname.
    /// </summary>
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
    /// <summary>
    /// Public Partial Class <c>ClientMessageRequestPlayerDelete</c> inherits from <c>ClientMessageRequest</c>
    /// Sent from Client to Server
    /// Client sent player register request containing username and playername
    /// </summary>
	public partial class ClientMessageRequestPlayerDelete : ClientMessageRequest
	{
		public string username;
		public string playername;
	}
	
	// -------------------------------------------------------------------------------
	
}