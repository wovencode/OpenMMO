//BY DX4D

using System;
using Mirror;
using UnityEngine;
using OpenMMO.Database;

namespace OpenMMO.Network
{
#region  N E T W O R K  M E S S A G E  H A N D L E R S
    // ---------------------------------------------

    //NETWORK AUTHENTICATOR
    public partial class NetworkManager
    {
        //@CLIENT SIDED RESPONSE TO MESSAGE FROM @SERVER
        // Direction: @Server -> @Client
        // Execution: @Client

        //@SERVER SIDED RESPONSE TO MESSAGE FROM @CLIENT
        // Direction: @Client -> @Server
        // Execution: @Server

        //LOGIN
        /// <summary>
        /// Event <c>OnClientMessageRequestPlayerLogin</c>.
        /// Triggered by the server receiving a player login request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        internal void OnClientMessageRequestPlayerLogin(NetworkConnection conn, ClientLoginPlayerRequest msg)
        {
            Response.PlayerLogin message = new Response.PlayerLogin
            {
                action = NetworkAction.PlayerLogin, //ADDED - DX4D
                success = true,
                text = "",
                causesDisconnect = false
            };

            // -- check for GetIsUserLoggedIn because that covers all players on the account
            if (GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryPlayerLogin(msg.playername, msg.username))
            {
                LoginPlayer(conn, msg.username, msg.playername, 0); //dont check for token
                message.text = systemText.playerLoginSuccess;
            }
            else
            {
                message.text = systemText.playerLoginFailure;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestPlayerLogin), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);

        }

        //REGISTER
        /// <summary>
        /// Event <c>OnClientMessageRequestPlayerRegister</c>.
        /// Triggered by the server receiving a player regstration request from the client. 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        internal void OnClientMessageRequestPlayerRegister(NetworkConnection conn, ClientRegisterPlayerRequest msg)
        {

            Response.PlayerRegister message = new Response.PlayerRegister
            {
                action = NetworkAction.PlayerRegister, //ADDED - DX4D
                success = true,
                text = "",
                causesDisconnect = false,
                playername = msg.playername
            };

            if (DatabaseManager.singleton.TryPlayerRegister(msg.playername, msg.username, msg.prefabname))
            {
                RegisterPlayer(msg.username, msg.playername, msg.prefabname);
                message.text = systemText.playerRegisterSuccess;
            }
            else
            {
                message.text = systemText.playerRegisterFailure;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestPlayerRegister), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);

        }

        //DELETE
        /// <summary>
        /// Event <c>OnClientMessageRequestPlayerDelete</c>.
        /// Triggered by the server receiving a player deletion request from the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        internal void OnClientMessageRequestPlayerDelete(NetworkConnection conn, ClientDeletePlayerRequest msg)
        {

            Response.PlayerDelete message = new Response.PlayerDelete
            {
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (DatabaseManager.singleton.TryPlayerDeleteSoft(msg.playername, msg.username))
            {
                message.text = systemText.playerDeleteSuccess;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestPlayerDelete), conn.Id(), "Success"); //DEBUG

            }
            else
            {
                message.text = systemText.playerDeleteFailure;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientMessageRequestPlayerDelete), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);

        }
    }
#endregion //NETWORK MESSAGE HANDLERS
}
