//BY DX4D

using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
    [CreateAssetMenu(menuName = FULL_MENU_PATH, order = 0)]
    public class AccountSystemPlugin : ScriptableNetworkPlugin
    {
        const string DESCRIPTION = "Account System";
        const string CREATE_PATH = "OpenMMO/Plugins/";
        
        #region P L U G I N  I N F O
        const string CREATE_MENU = DESCRIPTION + " - plugin";
        const string FULL_MENU_PATH = CREATE_PATH + CREATE_MENU;
        [Header("PLUGIN INFO")]
        [SerializeField] string _pluginName = DESCRIPTION;
        public override string PLUGIN_NAME => _pluginName;
        #endregion

        #region M E S S A G E  H A N D L E R S
        //CLIENT HANDLERS
        [Client]
        internal override void HandleServerMessageOnClient<T>(T msg)
        {
            if (msg is Response.UserPlayerPreviews) manager.OnServerResponseUserPlayerPreviews((ServerPlayerPreviewsResponse)msg);
            else if (msg is Response.UserLoginResponse) manager.OnServerResponseUserLogin((ServerLoginUserResponse)msg);
            //else if (msg is Response.UserLogout) manager.OnServerResponseUserLogout(msg);
            else if (msg is Response.UserRegisterResponse) manager.OnServerMessageResponseUserRegister(msg);
            else if (msg is Response.UserDeleteResponse) manager.OnServerMessageResponseUserDelete(msg);
            else if (msg is Response.UserChangePasswordResponse) manager.OnServerMessageResponseUserChangePassword(msg);
            else if (msg is Response.UserConfirmResponse) manager.OnServerResponseUserConfirm(msg);
        }
        [Server]
        internal override void HandleClientMessageOnServer<T>(NetworkConnection conn, T msg)
        {
            if (msg is Request.UserLoginRequest) manager.OnClientMessageRequestUserLogin(conn, (ClientLoginUserRequest)msg);
            else if (msg is Request.UserLogoutRequest) manager.OnClientMessageRequestUserLogout(conn, (ClientLogoutUserRequest)msg);
            else if (msg is Request.UserRegisterRequest) manager.OnClientMessageRequestUserRegister(conn, (ClientRegisterUserRequest)msg);
            else if (msg is Request.UserDeleteRequest) manager.OnClientMessageRequestUserDelete(conn, (ClientDeleteUserRequest)msg);
            else if (msg is Request.UserChangePasswordRequest) manager.OnClientMessageRequestUserChangePassword(conn, (ClientChangeUserPasswordRequest)msg);
            else if (msg is Request.UserConfirmRequest) manager.OnClientMessageRequestUserConfirm(conn, (ClientConfirmUserRequest)msg);
        }
        #endregion

        #region M E S S A G E  H A N D L E R  R E G I S T R Y
        //REGISTER CLIENT HANDLERS
        //[Client]
        internal override void RegisterClientMessageHandlers()
        {
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "LOGIN USER");
            NetworkClient.RegisterHandler<Response.UserLoginResponse>(HandleServerMessageOnClient, false);
            //Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "LOGOUT");
            //NetworkClient.RegisterHandler<Response.UserLogoutResponse>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "REGISTER USER");
            NetworkClient.RegisterHandler<Response.UserRegisterResponse>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "DELETE USER");
            NetworkClient.RegisterHandler<Response.UserDeleteResponse>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "CHANGE USER PASSWORD");
            NetworkClient.RegisterHandler<Response.UserChangePasswordResponse>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "CONFIRM USER");
            NetworkClient.RegisterHandler<Response.UserConfirmResponse>(HandleServerMessageOnClient, false);
        }
        //REGISTER SERVER HANDLERS
        //[Server]
        internal override void RegisterServerMessageHandlers()
        {
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "LOGIN USER");
            NetworkServer.RegisterHandler<Request.UserLoginRequest>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "LOGOUT USER");
            NetworkServer.RegisterHandler<Request.UserLogoutRequest>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "REGISTER USER");
            NetworkServer.RegisterHandler<Request.UserRegisterRequest>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "DELETE USER");
            NetworkServer.RegisterHandler<Request.UserDeleteRequest>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "CHANGE USER PASSWORD");
            NetworkServer.RegisterHandler<Request.UserChangePasswordRequest>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "CONFIRM USER");
            NetworkServer.RegisterHandler<Request.UserConfirmRequest>(HandleClientMessageOnServer, false);
        }
        #endregion //MESSAGE HANDLER REGISTRY
    }
}
