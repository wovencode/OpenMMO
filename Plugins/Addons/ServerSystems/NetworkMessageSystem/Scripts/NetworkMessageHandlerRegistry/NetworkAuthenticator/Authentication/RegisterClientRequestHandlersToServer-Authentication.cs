//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        //TODO: FIX SUMMARY
        // @Server
        /// <summary>
        /// Public ovverride event <c>OnStartServer</c>.
        /// Triggered on server start.
        /// Event occurs on the server.
        /// Sets up the user authentication event.
        /// </summary>
        internal protected void RegisterAuthenticationMessageHandlersToServer()
        {
            NetworkServer.RegisterHandler<ClientRequestConnect>(OnClientRequestConnect, false);
        }
    }
}