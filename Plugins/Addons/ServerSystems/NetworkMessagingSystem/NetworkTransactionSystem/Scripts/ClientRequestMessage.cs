//BY DX4D
using Mirror;

namespace OpenMMO.Network
{
    // -----------------------------------------------------------------------------------
    // ClientRequestMessage
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// public partial struct <c>ClientRequestMessage</c> inherits <c>ClientRequest</c>.
    /// Sent from Client to Server.
    /// </summary>
    public partial struct ClientRequestMessage : ClientRequest
    {
        public NetworkAction action { get; set; }
        public bool success { get; set; }
        public string text { get; set; }
    }
}