//BY FHIZ
// Direction: @Server -> @Client
// Execution: @Client

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnServerMessageResponseUserConfirm</c>.
        /// Triggered when the client receives a user changed on user confirmation response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponseUserConfirm(NetworkConnection conn, ServerResponseUserConfirm msg) //REMOVED - DX4D
        void OnServerResponseUserConfirm(ServerResponseUserConfirm msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerResponseUserConfirm), conn.Id(), msg.success.ToString()); //DEBUG
        	OnServerResponse(msg);
        }
    }
}