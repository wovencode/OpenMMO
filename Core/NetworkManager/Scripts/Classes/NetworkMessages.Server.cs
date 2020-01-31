
using System.Collections.Generic;
using Wovencode.Network;
using Wovencode;
using Mirror;

namespace Wovencode.Network
{

	// -----------------------------------------------------------------------------------
	// ServerMessageResponse
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
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
	public partial class ServerMessageResponseAuth : ServerMessageResponse
	{
		// do nothing (this message is never called directly)
	}

	// ================================== MESSAGES USER ==================================
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserLogin
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserLogin : ServerMessageResponseUserPlayerPreviews
	{
	
		// based on: ServerMessageResponseUserPlayerPreviews
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserRegister
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserRegister : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserDelete
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserDelete : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserChangePassword
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserChangePassword : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserConfirm
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserConfirm : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserPlayerPreviews
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserPlayerPreviews : ServerMessageResponse
	{
	
		public PlayerPreview[] players;
		public int maxPlayers;
		
		// -------------------------------------------------------------------------------
		// LoadPlayerPreviews
		// -------------------------------------------------------------------------------
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
	public partial class ServerMessageResponsePlayerLogin : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponsePlayerRegister
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponsePlayerRegister : ServerMessageResponse
	{

	}	
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponsePlayerDelete
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponsePlayerDelete : ServerMessageResponse
	{

	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponsePlayerSwitchServer
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponsePlayerSwitchServer : ServerMessageResponse
	{
		
	}
	
	// -------------------------------------------------------------------------------
	
}