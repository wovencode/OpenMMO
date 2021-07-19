//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
    	//TODO: FIX SUMMARY
    	// -------------------------------------------------------------------------------
		// OnStartServer
		// @Server
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public Event <c>OnStartServer</c>.
        /// Triggered when the server starts.
        /// Registers all the server's event handlers.
        /// Runs on server.
        /// </summary>
        internal protected void RegisterPlayerCharacterMessageHandlersToServer()
        {
            // ---- Player
            NetworkServer.RegisterHandler<ClientRequestPlayerLogin>(OnClientRequestPlayerLogin);
            NetworkServer.RegisterHandler<ClientRequestPlayerRegister>(OnClientRequestPlayerRegister);
            NetworkServer.RegisterHandler<ClientRequestPlayerDelete>(OnClientRequestPlayerDelete);
        }   
    }
}