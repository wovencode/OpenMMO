//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        // Direction: @Client -> @Server
        // Execution: @Server
        /// <summary>
        /// Event <c>OnClientRequest</c>.
        /// Event called when a <c>ClientRequest</c> is received on the server.
        /// Occurs on the server.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientRequest(NetworkConnection conn, ClientRequest msg)
        {
            //TODO: Add default client request handler that uses NetworkActions
            // do nothing (this message is never called directly)
        }
    }
}