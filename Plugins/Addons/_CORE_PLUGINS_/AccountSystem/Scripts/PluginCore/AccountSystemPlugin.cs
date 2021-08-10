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
            if (msg is Response.UserLogin) manager.OnServerResponseUserLogin((ServerLoginUserResponse)msg);
            //if (msg is Response.UserLogout) manager.OnServerResponseUserLogout(msg);
            if (msg is Response.UserRegister) manager.OnServerMessageResponseUserRegister(msg);
            if (msg is Response.UserDelete) manager.OnServerMessageResponseUserDelete(msg);
            if (msg is Response.UserChangePassword) manager.OnServerMessageResponseUserChangePassword(msg);
            if (msg is Response.UserConfirm) manager.OnServerResponseUserConfirm(msg);
        }
        [Server]
        internal override void HandleClientMessageOnServer<T>(NetworkConnection conn, T msg)
        {
            if (msg is Request.UserLogin) manager.OnClientMessageRequestUserLogin(conn, (ClientLoginUserRequest)msg);
            if (msg is Request.UserLogout) manager.OnClientMessageRequestUserLogout(conn, (ClientLogoutUserRequest)msg);
            if (msg is Request.UserRegister) manager.OnClientMessageRequestUserRegister(conn, (ClientRegisterUserRequest)msg);
            if (msg is Request.UserDelete) manager.OnClientMessageRequestUserDelete(conn, (ClientDeleteUserRequest)msg);
            if (msg is Request.UserChangePassword) manager.OnClientMessageRequestUserChangePassword(conn, (ClientChangeUserPasswordRequest)msg);
            if (msg is Request.UserConfirm) manager.OnClientMessageRequestUserConfirm(conn, (ClientConfirmUserRequest)msg);
        }
        #endregion

        #region M E S S A G E  H A N D L E R  R E G I S T R Y
        //REGISTER CLIENT HANDLERS
        //[Client]
        internal override void RegisterClientMessageHandlers()
        {
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "LOGIN USER");
            NetworkClient.RegisterHandler<Response.UserLogin>(HandleServerMessageOnClient, false);
            //Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "LOGOUT");
            //NetworkClient.RegisterHandler<Response.UserLogout>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "REGISTER USER");
            NetworkClient.RegisterHandler<Response.UserRegister>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "DELETE USER");
            NetworkClient.RegisterHandler<Response.UserDelete>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "CHANGE USER PASSWORD");
            NetworkClient.RegisterHandler<Response.UserChangePassword>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "CONFIRM USER");
            NetworkClient.RegisterHandler<Response.UserConfirm>(HandleServerMessageOnClient, false);
        }
        //REGISTER SERVER HANDLERS
        //[Server]
        internal override void RegisterServerMessageHandlers()
        {
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "LOGIN USER");
            NetworkServer.RegisterHandler<Request.UserLogin>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "LOGOUT USER");
            NetworkServer.RegisterHandler<Request.UserLogout>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "REGISTER USER");
            NetworkServer.RegisterHandler<Request.UserRegister>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "DELETE USER");
            NetworkServer.RegisterHandler<Request.UserDelete>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "CHANGE USER PASSWORD");
            NetworkServer.RegisterHandler<Request.UserChangePassword>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "CONFIRM USER");
            NetworkServer.RegisterHandler<Request.UserConfirm>(HandleClientMessageOnServer, false);
        }
        #endregion //MESSAGE HANDLER REGISTRY
    }
}
