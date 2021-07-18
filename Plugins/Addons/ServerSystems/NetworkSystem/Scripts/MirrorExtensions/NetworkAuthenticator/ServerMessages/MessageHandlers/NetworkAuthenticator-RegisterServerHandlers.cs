//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        // @Server
        /// <summary>
        /// Public ovverride event <c>OnStartServer</c>.
        /// Triggered on server start.
        /// Event occurs on the server.
        /// Sets up the user authentication event.
        /// </summary>
        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<ClientRequestConnect>(OnClientRequestConnect, false);
            
        	this.InvokeInstanceDevExtMethods(nameof(OnStartServer)); //HOOK
        }
    }
}