//BY DX4D

using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    [CreateAssetMenu( menuName = "OpenMMO/Network/Message Handlers/"
        + nameof(PlayerAutoLoginNetworkMessageHandler) )]
    public class PlayerAutoLoginNetworkMessageHandler : BaseNetworkMessageHandler// : ScriptableNetworkMessageHandler
    {
        //@CLIENT@SERVER
        public override NetworkAction networkAction => NetworkAction.Authenticate;

        //@CLIENT
        [Client]
        public override void HandleServerResponse(NetworkConnection conn, ServerResponse response)
        {
#if !_CLIENT
            if (!IsValidAction(response.action)) return; //VALIDATION <- //ESSENTIAL Be sure to include validation for added security

            throw new System.NotImplementedException();
#endif
        }
        //@SERVER
        [Server]
        public override void HandleClientRequest(NetworkConnection conn, ClientRequest request)
        {
#if _SERVER
            if (!IsValidAction(request.action)) return; //VALIDATION <- //ESSENTIAL Be sure to include validation for added security

            throw new System.NotImplementedException();
#endif
        }
    }
    /*
    public class OldPlayerAutoLoginNetworkMessage : ScriptableNetworkMessage
    {
        public override NetworkAction networkAction {
            get { return NetworkAction.PlayerAutoLogin; }
            }

        /// <summary>
        /// @CLIENT: Called on the client when it receives the server's response to it's client request.
        /// @SERVER: Called on the server when it receives a request from the client.
        /// </summary>
        public override void OnResponseReceivedFromServer(NetworkConnection conn, NetworkMessage msg)
        {
            throw new System.NotImplementedException();
        }
        public override T CreateClientRequestMessage<T>(NetworkMessage msg)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// @CLIENT: Called on the client to send a client request to the server.
        /// </summary>
        public override T CreateServerResponseMessage<T>(NetworkMessage msg)
        //public override void SendRequest(ClientRequest clientRequest)// string _playername, string _username, int _token)
        {
            return new ClientRequestPlayerAutoLogin
            {
                playername = _playername,
                username = _username,
                token = _token
            };
        }
        /// <summary>
        /// @SERVER: Called on the server when it receives a request from the client.
        /// </summary>
        public override void OnRequestReceivedFromClient(NetworkConnection conn, Mirror.NetworkMessage msg)
        {
            ServerResponse response = CreateServerResponseMessage();
        }
        /// <summary>
        /// @SERVER: Called on the server to send a server response to the client.
        /// </summary>//ServerResponsePlayerAutoLogin
        public override T CreateServerResponseMessage<T>(NetworkMessage msg)
        {
            return new T //PlayerAutoLogin
            {
                //action = networkAction, //ADDED - DX4D
                success = true,
                text = "",
                causesDisconnect = false
            };
        }
        public override void SendServerResponseMessage(ServerResponse response)
        {

        }
        /// <summary>
        /// @CLIENT: Called on the client when it receives the server's response to it's client request.\
        /// </summary>
        public override void OnResponseReceivedFromServer(Mirror.NetworkMessage msg) { }
    }*/
}
