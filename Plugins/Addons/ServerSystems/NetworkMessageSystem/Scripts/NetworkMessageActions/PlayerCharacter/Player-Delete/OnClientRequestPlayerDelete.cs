//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Database;
using UnityEngine;
using System;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client -> @Server
        /// <summary>
        /// Event <c>OnClientRequestPlayerDelete</c>.
        /// Triggered by the server receiving a player deletion request from the client.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestPlayerDelete(NetworkConnection conn, ClientRequestPlayerDelete msg)
        {

            ServerResponsePlayerDelete message = new ServerResponsePlayerDelete
            {
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (DatabaseManager.singleton.TryPlayerDeleteSoft(msg.playername, msg.username))
            {
                message.text = systemText.playerDeleteSuccess;

                debug.LogFormat(this.name, nameof(OnClientRequestPlayerDelete), conn.Id(), "Success"); //DEBUG

            }
            else
            {
                message.text = systemText.playerDeleteFailure;
                message.success = false;

                debug.LogFormat(this.name, nameof(OnClientRequestPlayerDelete), conn.Id(), "DENIED"); //DEBUG

            }

            conn.Send(message);
        }
    }
}