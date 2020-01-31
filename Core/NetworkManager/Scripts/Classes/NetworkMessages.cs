
using Wovencode.Network;
using Wovencode;
using Mirror;

namespace Wovencode.Network
{

	// -----------------------------------------------------------------------------------
	// ErrorMsg
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ErrorMsg : MessageBase
	{
   		public string text;
   		public bool causesDisconnect;
	}
	
	// -------------------------------------------------------------------------------
	
}