//by Fhiz
using OpenMMO.Network;
using OpenMMO;
using Mirror;

namespace OpenMMO.Network
{

   
    /// <summary>
    /// Switch Server Request Message
    /// </summary>
    /// <remarks>
    /// Sent from Client to Server
    /// </remarks>
    public partial class ClientMessageRequestPlayerSwitchServer : ClientMessageRequest
	{
		public string playername;
		public string zonename;
		public string anchorname;
		public int token;
	}
	
	/// <summary>
    /// Auto Login Player Request Message
    /// </summary>
    /// <remarks>
    /// Sent from Client to Server
    /// </remarks>
	public partial class ClientMessageRequestPlayerAutoLogin : ClientMessageRequest
	{
		public string username;
		public string playername;
		public int token;
	}
	
	/// <summary>
    /// Auto Authentication Request Message
    /// </summary>
    /// <remarks>
    /// Sent from Client to Server
    /// </remarks>
    public partial class ClientMessageRequestAutoAuth : ClientMessageRequest
	{
		public string clientVersion;
	}

}