//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
		// @Client -> @Server
        /// <summary>
        /// Event <c>OnClientMessageRequest</c>.
        /// Triggerd when the server receives a Client message request from the client.
        /// Doesn't do anything as htis message is never called directly.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientRequest(NetworkConnection conn, ClientRequest msg)
        {
            //TODO: Add default client request handler that uses NetworkActions
    		// do nothing (this message is never called directly)
        }
    }
}