//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Database;
using UnityEngine;
using System;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
    // NetworkManager
    // ===================================================================================
    public partial class NetworkManager
    {
        // ===============================================================================
        // ============================= MESSAGE HANDLERS ================================
        // ===============================================================================

        // -------------------------------------------------------------------------------
        // OnClientMessageRequest
        // @Client -> @Server
        // --------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnClientMessageRequest</c>.
        /// Triggerd when the server receives a Client message request from the client.
        /// Doesn't do anything as htis message is never called directly.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientMessageRequest(NetworkConnection conn, ClientRequest msg)
        {
            // do nothing (this message is never called directly)
        }

        // ========================== MESSAGE HANDLERS - USER ============================

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserLogin
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnClientMessageRequestUserLogin</c>.
        /// Triggered when the server receives a user login request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        internal void OnClientMessageRequestUserLogin(NetworkConnection conn, ClientLoginUserRequest msg)
        {

            Response.UserLoginResponse message = new Response.UserLoginResponse
            {
                action = NetworkAction.UserLogin, //ADDED - DX4D
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserLogin(msg.username, msg.password))
            {
                LoginUser(conn, msg.username);

                // TODO: Add increased maxPlayers from user data later
                message.maxPlayers = GameRulesTemplate.singleton.maxPlayersPerUser;
                message.LoadPlayerPreviews(DatabaseManager.singleton.GetPlayers(msg.username));
                message.text = systemText.USER_LOGIN_SUCCESS;
            }
            else
            {
                message.text = systemText.USER_LOGIN_FAILURE;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestUserLogin), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);

        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserLogout
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        internal void OnClientMessageRequestUserLogout(NetworkConnection conn, ClientLogoutUserRequest msg)
        {
            LogoutUser(conn);
        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserRegister
        // @Client -> @Server
        // -------------------------------------------------------------------------------    
        /// <summary>
        /// Event <c>OnClientMessageRequestUserRegister</c>.
        /// Triggered when the server receives a user registration request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        internal void OnClientMessageRequestUserRegister(NetworkConnection conn, ClientRegisterUserRequest msg)
        {

            Response.UserRegisterResponse message = new Response.UserRegisterResponse
            {
                action = NetworkAction.UserRegister, //ADDED - DX4D
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (DatabaseManager.singleton.TryUserRegister(msg.username, msg.password, msg.email, msg.deviceid))
            {
                RegisterUser(msg.username);
                message.text = systemText.USER_REGISTER_SUCCESS;
            }
            else
            {
                message.text = systemText.USER_REGISTER_FAILURE;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestUserRegister), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);

        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserDelete
        // @Client -> @Server
        // -------------------------------------------------------------------------------    
        /// <summary>
        /// Event <c>OnClientMessageRequestUserDelete</c>.
        /// Triggered when the server receives a user deletion request.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        internal void OnClientMessageRequestUserDelete(NetworkConnection conn, ClientDeleteUserRequest msg)
        {

            Response.UserDeleteResponse message = new Response.UserDeleteResponse
            {
                action = NetworkAction.UserDelete, //ADDED - DX4D
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserDelete(msg.username, msg.password))
            {
                message.text = systemText.USER_DELETE_SUCCESS;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestUserDelete), conn.Id(), "Success"); //DEBUG

            }
            else
            {
                message.text = systemText.USER_DELETE_FAILURE;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestUserDelete), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);

        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserChangePassword
        // @Client -> @Server
        // -------------------------------------------------------------------------------  
        /// <summary>
        /// Event <c>OnClientMessageRequestUserChangePassword</c>.
        /// Triggered when the server receives a user change password request.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        internal void OnClientMessageRequestUserChangePassword(NetworkConnection conn, ClientChangeUserPasswordRequest msg)
        {

            Response.UserChangePasswordResponse message = new Response.UserChangePasswordResponse
            {
                action = NetworkAction.UserChangePassword, //ADDED - DX4D
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserChangePassword(msg.username, msg.oldPassword, msg.newPassword))
            {
                message.text = systemText.USER_CHANGE_PASSWORD_SUCCESS;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestUserChangePassword), conn.Id(), "Success"); //DEBUG

            }
            else
            {
                message.text = systemText.USER_CHANGE_PASSWORD_FAILURE;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestUserChangePassword), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);

        }

        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserConfirm
        // @Client -> @Server
        // -------------------------------------------------------------------------------    
        /// <summary>
        /// Event <c>OnClientMessageRequestUserConfirm</c>.
        /// Triggered by the server receiving a user confirmation request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        internal void OnClientMessageRequestUserConfirm(NetworkConnection conn, ClientConfirmUserRequest msg)
        {

            Response.UserConfirmResponse message = new Response.UserConfirmResponse
            {
                action = NetworkAction.UserConfirm, //ADDED - DX4D
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserConfirm(msg.username, msg.password))
            {
                message.text = systemText.USER_CONFIRM_SUCCESS;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestUserConfirm), conn.Id(), "Success"); //DEBUG

            }
            else
            {
                message.text = systemText.USER_CONFIRM_FAILURE;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestUserConfirm), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);

        }
    }
}
