//BY DX4D

using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
    [CreateAssetMenu(menuName = "OpenMMO/Plugins/Account System Plugin", order = 0)]
    public class AccountSystemPlugin : ScriptableNetworkPlugin
    {
        [Header("PLUGIN INFO")]
        [SerializeField] string _pluginName = "USER ACCOUNT SYSTEM";
        public override string PLUGIN_NAME => _pluginName;

        #region M E S S A G E  H A N D L E R S
        //CLIENT HANDLERS
        [Client]
        internal override void HandleServerMessageOnClient<T>(T msg)
        {
            if (msg is Response.UserPlayerPreviews) manager.OnServerResponseUserPlayerPreviews((ServerPlayerPreviewsResponse)msg);
            if (msg is Response.UserLogin) manager.OnServerResponseUserLogin((ServerLoginUserResponse)msg);
            //if (msg is Response.UserLogout) manager.OnServerResponseUserLogout(msg);
            if (msg is Response.UserRegister) manager.OnServerMessageResponseUserRegister((ServerRegisterUserResponse)msg);
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

        internal override void RegisterClientMessageHandlers()
        {
            throw new System.NotImplementedException();
        }

        internal override void RegisterServerMessageHandlers()
        {
            throw new System.NotImplementedException();
        }
    }
}
