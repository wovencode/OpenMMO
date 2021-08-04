//BY DX4D
using Mirror;

namespace OpenMMO.Network
{

    /// <summary><see cref="NetworkTransaction"/> - 
    /// A Network Message that triggers a <see cref="NetworkAction"/>
    /// <para>Implemented by <see cref="ClientRequest"/> and <see cref="ServerResponse"/></para>
    /// </summary>
    public partial interface NetworkTransaction : NetworkMessage
    {
        NetworkAction action { get; }
        //bool success { get; set; }
        //string text { get; set; }
    }
}