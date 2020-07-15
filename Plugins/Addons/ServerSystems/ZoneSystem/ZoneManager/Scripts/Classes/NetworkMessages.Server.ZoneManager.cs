//by Fhiz
using System.Collections.Generic;
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{

   	/// <summary>
    /// Player Switch Server Response Message
    /// </summary>
    /// <remarks>
    /// Sent from Server to Client
    /// </remarks>
    public partial class ServerMessageResponsePlayerSwitchServer : ServerMessageResponse
	{
		public string playername;
		public string anchorname;
		public string zonename;
		public int token;
	}
	
	/// <summary>
    /// Player Auto Login Response Message
    /// </summary>
    /// <remarks>
    /// Sent from Server to Client
    /// </remarks>
    public partial class ServerMessageResponsePlayerAutoLogin : ServerMessageResponse {}
	
	/// <summary>
    /// Auto Authentication Response Message
    /// </summary>
    /// <remarks>
    /// Sent from Server to Client
    /// </remarks>
    public partial class ServerMessageResponseAutoAuth : ServerMessageResponse {}
	
}