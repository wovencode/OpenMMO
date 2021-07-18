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
        internal protected void RegisterUserAccountMessageHandlersToClient()
        {
        	
            // ---- User Messages
            // @Server -> @Client
            NetworkClient.RegisterHandler<ServerResponseUserLogin>(OnServerResponseUserLogin);
            NetworkClient.RegisterHandler<ServerResponseUserRegister>(OnServerMessageResponseUserRegister);
            NetworkClient.RegisterHandler<ServerResponseUserDelete>(OnServerMessageResponseUserDelete);
            NetworkClient.RegisterHandler<ServerResponseUserChangePassword>(OnServerMessageResponseUserChangePassword);
            NetworkClient.RegisterHandler<ServerResponseUserConfirm>(OnServerResponseUserConfirm);
            NetworkClient.RegisterHandler<ServerResponseUserPlayerPreviews>(OnServerResponseUserPlayerPreviews);
        }
    }
}