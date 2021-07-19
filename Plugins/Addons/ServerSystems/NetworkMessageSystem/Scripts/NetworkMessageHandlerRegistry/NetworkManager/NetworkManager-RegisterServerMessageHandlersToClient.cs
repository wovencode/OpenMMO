//BY FHIZ

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        //TODO: FIX SUMMARY
        // @Client
        /// <summary>
        /// Pubilc override event <c>OnStartClient</c>.
        /// Triggered when the client starts.
        /// Occurs on client.
        /// Registers all the user request and server response handlers.
        /// </summary>
        public override void OnStartClient()
        {

            // ---- User Messages
            // @Server -> @Client
            RegisterUserAccountMessageHandlersToClient();
            //NetworkClient.RegisterHandler<ServerResponseUserLogin>(OnServerResponseUserLogin);
            //NetworkClient.RegisterHandler<ServerResponseUserRegister>(OnServerMessageResponseUserRegister);
            //NetworkClient.RegisterHandler<ServerResponseUserDelete>(OnServerMessageResponseUserDelete);
            //NetworkClient.RegisterHandler<ServerResponseUserChangePassword>(OnServerMessageResponseUserChangePassword);
            //NetworkClient.RegisterHandler<ServerResponseUserConfirm>(OnServerResponseUserConfirm);
            //NetworkClient.RegisterHandler<ServerResponseUserPlayerPreviews>(OnServerResponseUserPlayerPreviews);

            // ---- Player Messages
            // @Server -> @Client
            RegisterPlayerCharacterMessageHandlersToClient();
            //NetworkClient.RegisterHandler<ServerResponsePlayerLogin>(OnServerResponsePlayerLogin);
            //NetworkClient.RegisterHandler<ServerResponsePlayerRegister>(OnServerResponsePlayerRegister);
            //NetworkClient.RegisterHandler<ServerResponsePlayerDelete>(OnServerResponsePlayerDelete);

            // --- Error Message
            // @Server -> @Client
            RegisterErrorMessageHandlersToClient();
            //NetworkClient.RegisterHandler<ServerResponseError>(OnServerResponseError);
            
            this.InvokeInstanceDevExtMethods(nameof(OnStartClient)); //HOOK
            eventListeners.OnStartClient.Invoke(); //EVENT

        }
    }
}