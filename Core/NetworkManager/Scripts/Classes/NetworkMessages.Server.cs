
using System.Collections.Generic;
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{

	// -----------------------------------------------------------------------------------
	// ServerMessageResponse
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
    /// <summary>
    /// Public partial Class <c>ServerMessageResponse</c> inherits from Mirror.MessageBase.
    /// Sent from Server to Client.
    /// Server Message response containing boolean dictating success, a text message, and a boolean dictating wether the message causes disconnection.
    /// </summary>
	public partial class ServerMessageResponse : MessageBase
	{
		public bool success;
		public string text;
		public bool causesDisconnect;
	}

    // ================================= MESSAGES AUTH ===================================

    // -----------------------------------------------------------------------------------
    // ServerMessageResponseAuth
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseAuth</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial class ServerMessageResponseAuth : ServerMessageResponse
	{
		
	}

    // ================================== MESSAGES USER ==================================

    // -----------------------------------------------------------------------------------
    // ServerMessageResponseUserLogin
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserLogin</c> inherits <c>ServerMessageResponseUserPlayerPreviews</c>.
    /// Sent from Server to Client.
    /// Based on ServerMessageResponseUserPlayerPreviews. Contains only inherited functionality.
    /// </summary>
    public partial class ServerMessageResponseUserLogin : ServerMessageResponseUserPlayerPreviews
	{
	
		// based on: ServerMessageResponseUserPlayerPreviews
		
	}

    // -----------------------------------------------------------------------------------
    // ServerMessageResponseUserRegister
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserRegister</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial class ServerMessageResponseUserRegister : ServerMessageResponse
	{
		
	}

    // -----------------------------------------------------------------------------------
    // ServerMessageResponseUserDelete
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserDelete</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial class ServerMessageResponseUserDelete : ServerMessageResponse
	{
		
	}

    // -----------------------------------------------------------------------------------
    // ServerMessageResponseUserChangePassword
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserChangePassword</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial class ServerMessageResponseUserChangePassword : ServerMessageResponse
	{
		
	}

    // -----------------------------------------------------------------------------------
    // ServerMessageResponseUserConfirm
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserConfirm</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial class ServerMessageResponseUserConfirm : ServerMessageResponse
	{
		
	}

    // -----------------------------------------------------------------------------------
    // ServerMessageResponseUserPlayerPreviews
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserPlayerPreviews</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// Contains an array of <c>PlayerPreview</c>(s) and the max player number.
    /// Also contains a <c>LoadPlayerPreviews</c> method.
    /// </summary>
    public partial class ServerMessageResponseUserPlayerPreviews : ServerMessageResponse
	{
	
		public PlayerPreview[] players;
		public int maxPlayers;

        // -------------------------------------------------------------------------------
        // LoadPlayerPreviews
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public method <c>LoadPlayerPreviews</c> located inside <c>ServerMessageResponseUserPlayerPreviews</c>.
        /// Loads an array of players  previews from a list of players
        /// </summary>
        /// <param name="_players"></param>
        public void LoadPlayerPreviews(List<PlayerPreview> _players)
		{
			players = new PlayerPreview[_players.Count];
			players = _players.ToArray();
		}
		
	}

    // ================================ MESSAGES PLAYER ==================================

    // -----------------------------------------------------------------------------------
    // ServerMessageResponsePlayerLogin
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponsePlayerLogin</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial class ServerMessageResponsePlayerLogin : ServerMessageResponse
	{
		
	}

    // -----------------------------------------------------------------------------------
    // ServerMessageResponsePlayerRegister
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponsePlayerRegister</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial class ServerMessageResponsePlayerRegister : ServerMessageResponse
	{
		public string playername;
	}

    // -----------------------------------------------------------------------------------
    // ServerMessageResponsePlayerDelete
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponsePlayerDelete</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial class ServerMessageResponsePlayerDelete : ServerMessageResponse
	{

	}

	// -------------------------------------------------------------------------------
	
}