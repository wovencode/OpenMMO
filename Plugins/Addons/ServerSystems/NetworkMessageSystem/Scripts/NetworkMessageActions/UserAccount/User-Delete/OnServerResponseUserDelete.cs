//BY FHIZ
// Direction: @Server -> @Client
// Execution: @Client

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnServerMessageResponseUserDelete</c>.
        /// Triggered when the client receives a user deletion response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponseUserDelete(NetworkConnection conn, ServerResponseUserDelete msg) //REMOVED - DX4D
        void OnServerMessageResponseUserDelete(ServerResponseUserDelete msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerMessageResponseUserDelete), conn.Id(), msg.success.ToString()); //DEBUG
        	OnServerResponse(msg);
        }
    }
}