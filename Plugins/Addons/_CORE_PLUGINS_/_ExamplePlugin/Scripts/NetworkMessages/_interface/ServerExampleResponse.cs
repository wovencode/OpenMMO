//BY DX4D

namespace OpenMMO.Network
{
    // @Client -> @Server
    /// <summary><para>An example Server Response interface - 
    /// This exposes variables in the message by passing a lightweight 
    /// interface around instead of a full network message.</para>
    /// <para>NOTE: <see cref="ServerResponse"/> can be used 
    /// if you only need a basic message with no extra data fields.</para></summary>
    public partial interface ServerExampleResponse : ServerResponse
    {
        string exampleMessage { get; }
    }
}
