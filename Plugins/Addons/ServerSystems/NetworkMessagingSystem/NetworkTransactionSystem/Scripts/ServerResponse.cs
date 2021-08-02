//UPDATED MIRROR VERSION v13 to v42.2.8 BY DX4D
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
    /// Public partial Class <see cref="ServerResponse"/> 
    /// inherits from <see cref="NetworkTransaction"/>.
    /// Sent from Server to Client.
    /// Server Message response containing boolean dictating success, a text message, and a boolean dictating wether the message causes disconnection.
    /// </summary>
    public interface ServerResponse : NetworkTransaction
    {
        bool success { get; set; }
        string text { get; set; }
        bool causesDisconnect { get; set; }
    }
}