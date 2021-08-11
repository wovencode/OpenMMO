//BY DX4D

using System;
using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
#region  N E T W O R K  M E S S A G E  H A N D L E R S
    // ---------------------------------------------

    //NETWORK AUTHENTICATOR
    public partial class NetworkAuthenticator
    {
        //@CLIENT SIDED RESPONSE TO MESSAGE FROM @SERVER
        // Direction: @Server -> @Client
        // Execution: @Client
        /// <summary>
        /// Event <c>OnServerMessageResponseAuth</c>.
        /// Is triggered when the server returns a authentication response message.
        /// Either authenicates the client, disconnects the client and returns an error message if there is one. 
        /// If the authentication was succesful the client is readied.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        internal void OnServerResponseExample(ServerExampleResponse msg)
        {
            //NetworkConnection conn = NetworkClient.connection; //NOTE: <- How to get the connection here.

            //SHOW SERVER RESPONSE MESSAGE IN A POPUP
            if (!String.IsNullOrWhiteSpace(msg.exampleMessage))
            {
                Debug.Log("CLIENT RECEIVED A RESPONSE MESSAGE FROM THE SERVER:"
                    + "\n" + "RECEIVED: " + msg.exampleMessage);

                UI.UIPopupConfirm.singleton.Init(msg.exampleMessage); //POPUP
            }
        }

        //@SERVER SIDED RESPONSE TO MESSAGE FROM @CLIENT
        // Direction: @Client -> @Server
        // Execution: @Server
        /// <summary>
        /// Event <c>OnClientMessageRequestAuth</c>.
        /// Triggered when the server received an authentiation request from a client.
        /// The event attempts to authenticate the client.
        /// Occurs on the server.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="_clientRequest"></param>
        internal void OnClientRequestExample(NetworkConnection conn, ClientExampleRequest _clientRequest)
        {
            Response.ExampleServerResponseMessage _serverResponse = new Response.ExampleServerResponseMessage
            {
                success = true,
                text = "",
                causesDisconnect = false
            };

            if (_clientRequest == null) //NO REQUEST MESSAGE
            {
                _serverResponse.text = "Oops! Something went wrong...";
                _serverResponse.success = false; //FAILURE
            }
            else
            {
                Debug.Log("SERVER RECIEVED A MESSAGE FROM A CLIENT:"
                    + "\n" + "RECEIVED: " + _clientRequest.exampleMessage); //LOG THE EXAMPLE MESSAGE TO SERVER CONSOLE
                _serverResponse.success = true; //SUCCESS
            }

            Debug.Log("SERVER SENDING RESPONSE MESSAGE TO CLIENT:"
                + "\n" + "SENDING: " + _serverResponse.exampleMessage);
            conn.Send(_serverResponse); //SEND

            if (!_serverResponse.success) { Debug.Log(_serverResponse.text); } //ERROR LOG
        }
    }
#endregion //NETWORK MESSAGE HANDLERS
}
