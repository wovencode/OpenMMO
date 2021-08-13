//BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    // @Client -> @Server
    /// <summary>
    /// Public partial interface <c>ClientRequest</c> inherits from Mirror.NetworkMessage.
    /// Containts the message sent from client to server
    /// </summary>
    public partial interface ClientDeleteUserRequest : ClientRequest
    {
        string username { get; set; }
        string password { get; set; }
    }
}
