//MIRROR UPDATE - VERSION v13 to v42.2.8 BY DX4D
using Mirror;

namespace OpenMMO.Network
{

    // -----------------------------------------------------------------------------------
    // ClientMessageRequest
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public partial interface <c>ClientRequest</c> inherits from Mirror.NetworkMessage.
    /// Containts the message sent from client to server
    /// </summary>
    public partial interface ClientRequest : NetworkMessage
    {
        bool success { get; set; }
        string text { get; set; }
    }
}