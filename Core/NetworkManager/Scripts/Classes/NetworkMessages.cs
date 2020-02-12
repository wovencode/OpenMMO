
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{

	// -----------------------------------------------------------------------------------
	// ErrorMsg
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
    /// <summary>
    /// Public partial class <c>ErrorMsg</c> that inherits from Mirror.MessageBase
    /// Sent from Server to Client
    /// The error message sent contains text and a boolean dictating whether the cient will disconnect or not
    /// </summary>
	public partial class ErrorMsg : MessageBase
	{
   		public string text;
   		public bool causesDisconnect;
	}
	
	// -------------------------------------------------------------------------------
	
}