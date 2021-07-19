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
        public override void OnStartServer()
        {

            // ---- User
            RegisterUserAccountMessageHandlersToServer();
            //NetworkServer.RegisterHandler<ClientRequestUserLogin>(OnClientRequestUserLogin);
            //NetworkServer.RegisterHandler<ClientRequestUserLogout>(OnClientRequestUserLogout);
            //NetworkServer.RegisterHandler<ClientRequestUserRegister>(OnClientRequestUserRegister);
            //NetworkServer.RegisterHandler<ClientRequestUserDelete>(OnClientRequestUserDelete);
            //NetworkServer.RegisterHandler<ClientRequestUserChangePassword>(OnClientRequestUserChangePassword);
            //NetworkServer.RegisterHandler<ClientRequestUserConfirm>(OnClientRequestUserConfirm);

            // ---- Player
            RegisterPlayerCharacterMessageHandlersToServer();
            //NetworkServer.RegisterHandler<ClientRequestPlayerLogin>(OnClientRequestPlayerLogin);
            //NetworkServer.RegisterHandler<ClientRequestPlayerRegister>(OnClientRequestPlayerRegister);
            //NetworkServer.RegisterHandler<ClientRequestPlayerDelete>(OnClientRequestPlayerDelete);

			this.InvokeInstanceDevExtMethods(nameof(OnStartServer)); //HOOK
        	eventListeners.OnStartServer.Invoke(); //EVENT
        }   
    }
}