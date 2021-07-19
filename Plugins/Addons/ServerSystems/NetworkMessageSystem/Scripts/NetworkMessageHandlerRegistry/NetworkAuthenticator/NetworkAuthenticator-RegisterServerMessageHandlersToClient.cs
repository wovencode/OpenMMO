//BY FHIZ
//MODIFIED BY DX4D

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        // -------------------------------------------------------------------------------
        // @Client
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public override event <c>OnStartClient</c>.
        /// Event triggered on client start.
        /// This Event occurs on the client.
        /// The even registers the authentication event.
        /// </summary>
        public override void OnStartClient()
        {
            RegisterAuthenticationMessageHandlersToClient();
            //NetworkClient.RegisterHandler<ServerResponseConnect>(OnServerResponseConnect, false);  
            
            this.InvokeInstanceDevExtMethods(nameof(OnStartClient)); //HOOK
        }
    }
}