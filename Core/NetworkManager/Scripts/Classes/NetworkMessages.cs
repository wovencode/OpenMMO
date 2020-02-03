
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
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