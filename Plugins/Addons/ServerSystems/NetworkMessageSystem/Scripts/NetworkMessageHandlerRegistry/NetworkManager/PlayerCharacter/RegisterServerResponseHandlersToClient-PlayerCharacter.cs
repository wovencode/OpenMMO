//BY FHIZ

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        //TODO: FIX SUMMARY
        // -------------------------------------------------------------------------------
        // OnStartClient
        // @Client
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Pubilc override event <c>OnStartClient</c>.
        /// Triggered when the client starts.
        /// Occurs on client.
        /// Registers all the user request and server response handlers.
        /// </summary>
        internal protected void RegisterPlayerCharacterMessageHandlersToClient()
        {
            // ---- Player Messages
            // @Server -> @Client
            NetworkClient.RegisterHandler<ServerResponsePlayerLogin>(OnServerResponsePlayerLogin);
            NetworkClient.RegisterHandler<ServerResponsePlayerRegister>(OnServerResponsePlayerRegister);
            NetworkClient.RegisterHandler<ServerResponsePlayerDelete>(OnServerResponsePlayerDelete);
        }
    }
}