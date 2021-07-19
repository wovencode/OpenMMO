//BY FHIZ
// Direction: @Server -> @Client
// Execution: @Client

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnServerResponseUserChangePassword</c>.
        /// Triggered when the client receives a user changed password response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponseUserChangePassword(NetworkConnection conn, ServerResponseUserChangePassword msg) //REMOVED - DX4D
        void OnServerResponseUserChangePassword(ServerResponseUserChangePassword msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerResponseUserChangePassword), conn.Id(), msg.success.ToString()); //DEBUG
        	OnServerResponse(msg);
        }
    }
}