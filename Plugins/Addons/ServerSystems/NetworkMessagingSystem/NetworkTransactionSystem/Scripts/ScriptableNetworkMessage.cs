//BY DX4D

using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
    public abstract class ScriptableNetworkMessageHandler : ScriptableObject
    {
        public abstract NetworkAction networkAction { get; }

        //HANDLE SERVER RESPONSE @CLIENT
        /// <summary>@CLIENT: Called on the client when it receives the server's response to it's client request. 
        /// <para>NOTE: Be sure to check <see cref="IsValidAction(NetworkAction)"/> before handling the response.</para></summary>
        public abstract void HandleServerResponse(NetworkConnection conn, ServerResponse response);

        //HANDLE CLIENT REQUEST @SERVER
        /// <summary>@SERVER: Called on the server when it receives a request from the client. 
        /// <para>NOTE: Be sure to check <see cref="IsValidAction(NetworkAction)"/> before handling the request.</para></summary>
        public abstract void HandleClientRequest(NetworkConnection conn, ClientRequest request);

        //IS VALID ACTION
        /// <summary>@ANY: Must be called when a Message is processed by a Handler to make sure it is a valid message.
        /// <para>
        /// <list type="bullet"><listheader>MUST CALL FROM</listheader>
        /// <item><see cref="HandleServerResponse(NetworkConnection, ServerResponse)"/></item>
        /// <item><see cref="HandleClientRequest(NetworkConnection, ClientRequest)"/></item>
        /// </list>
        /// </para></summary>
        /// <param name="action">The action from the ServerResponse or ClientRequest that we received</param>
        /// <returns>Does the action of the received message match the network action that this object handles?</returns>
        protected bool IsValidAction(NetworkAction action)
        {
            const string SPACER = " - ";
            string ERROR_HEADER = "[UNKNOWN ISSUE]";
            string SUCCESS_HEADER = "[UNKNOWN MESSAGE]";
#if _CLIENT
            ERROR_HEADER = "<b>[<color=red>CLIENT ISSUE</color>]</b>";
            SUCCESS_HEADER = "<b>[<color=blue>CLIENT MESSAGE</color>]</b>";
#endif
#if _SERVER
            ERROR_HEADER = "[SERVER ISSUE]";
            SUCCESS_HEADER = "[SERVER MESSAGE]";
#endif

            if (action != networkAction)
            {
                Debug.Log(ERROR_HEADER + SPACER
                + "Invalid Network Action!"
                    + " - Expected " + networkAction + " message - received " + action + " message");
                return false;
            }
            else
            {
                Debug.Log(SUCCESS_HEADER + SPACER
                    + "Handling " + networkAction + " Message!");
                return true;
            }
        }

        // - - - - - - - - - - - - - -
        // C L I E N T  R E Q U E S T
#if _CLIENT
        //CREATE NETWORK TRANSACTION
        /// <summary>@CLIENT@SERVER: 
        /// Creates a more specific transaction from the passed in Network Transaction.
        /// <para>Can be used to extract a <see cref="NetworkMessage"/> from a transaction.</para>
        /// <para>Example: <see cref="NetworkPingMessage"/> could be extracted, modified, and then sent across the network.
        /// </para>
        /// </summary>
        protected T CreateNetworkMessage<T>(NetworkTransaction msg)
            where T : struct, NetworkTransaction
        { return (T)msg; }

        //SEND NETWORK MESSAGE
        /// <summary>@CLIENT: Called on the client to send a client request to the server.</summary>
        protected void SendNetworkMessage<T>(NetworkConnection conn, T msg)
            where T : struct, NetworkTransaction
        {
            conn.Send<T>(CreateNetworkMessage<T>(msg));
        }
#endif
        // - - - - - - - - - - - - - - -
        // S E R V E R  R E S P O N S E
#if _SERVER
        //CREATE SERVER RESPONSE MESSAGE
        /// <summary>@SERVER: Called on the server to create a server response.</summary>
        /// <see cref="ServerResponsePlayerAutoLogin"/>
        protected T CreateServerResponseMessage<T>(Mirror.NetworkMessage msg)
            where T : struct, ServerResponse
        { return (T)msg; }
        //SEND SERVER RESPONSE MESSAGE
        /// <summary>@SERVER: Called on the server to send a server response to the client.</summary>
        protected void SendServerResponseMessage<T>(Mirror.NetworkConnection conn, Mirror.NetworkMessage msg)
                where T : struct, ServerResponse
        {
            conn.Send<T>(CreateServerResponseMessage<T>(msg));
        }
#endif


        //TODO: MOVE US TO TRANSACTION MANAGER
        /// <summary>
        /// @CLIENT: Called on the client when it receives the server's response to it's client request.
        /// @SERVER: Called on the server when it receives a request from the client.
        /// </summary>
        ///<param name="conn">The connection that sent the message</param>
        protected void OnMessageReceived<T>(NetworkConnection conn, NetworkMessage msg)
        {
#if _CLIENT
            if (msg is ServerResponse) HandleServerResponse(conn, msg as ServerResponse); //CLIENT
#endif
#if _SERVER
            if (msg is ClientRequest) HandleClientRequest(conn, msg as ClientRequest); //SERVER
#endif
        }

        //SEND MESSAGE @CLIENT @SERVER
        /// <summary>
        /// @CLIENT: Called on the client to send a client request to the server.
        /// @SERVER: Called on the server to send a server response to the client.
        /// </summary>
        protected void SendMessage<T>(Mirror.NetworkConnection conn, NetworkTransaction msg)
            where T : struct, NetworkTransaction
        {
            /*
#if _CLIENT
            SendClientRequestMessage<T>(conn, msg); //CLIENT
#endif
#if _SERVER
            SendServerResponseMessage<T>(conn, msg); //SERVER
#endif*/
        }
    }
}
