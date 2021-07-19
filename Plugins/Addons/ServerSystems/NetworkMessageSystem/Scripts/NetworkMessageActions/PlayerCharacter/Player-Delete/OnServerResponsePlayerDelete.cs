//BY FHIZ
// Direction: @Server -> @Client
// Execution: @Client

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerDelete</c>.
        /// Triggered when the client receives a player delete response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponsePlayerDelete(NetworkConnection conn, ServerResponsePlayerDelete msg) //REMOVED - DX4D
        void OnServerResponsePlayerDelete(ServerResponsePlayerDelete msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerResponsePlayerDelete), conn.Id(), msg.success.ToString()); //DEBUG

            OnServerResponse(msg);
        }
    }
}