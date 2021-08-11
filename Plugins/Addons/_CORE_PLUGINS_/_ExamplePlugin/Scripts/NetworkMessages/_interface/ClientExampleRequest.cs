//BY DX4D

namespace OpenMMO.Network
{
    // @Client -> @Server
    /// <summary><para>An example Client Request interface - 
    /// This exposes variables in the message by passing a lightweight 
    /// interface around instead of a full network message.</para>
    /// <para>NOTE: <see cref="ClientRequest"/> can be used 
    /// if you only need a basic message with no extra data fields.</para></summary>
    public partial interface ClientExampleRequest : ClientRequest
    {
        string exampleMessage { get; }
    }
}
