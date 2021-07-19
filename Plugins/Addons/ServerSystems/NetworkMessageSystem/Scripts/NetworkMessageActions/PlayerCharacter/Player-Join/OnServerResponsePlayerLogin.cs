//BY FHIZ
// Direction: @Server -> @Client
// Execution: @Client

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerLogin</c>.
        /// Triggered when the client receives a player login response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponsePlayerLogin(NetworkConnection conn, ServerResponsePlayerLogin msg) //REMOVED - DX4D
        void OnServerResponsePlayerLogin(ServerResponsePlayerLogin msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerResponsePlayerLogin), conn.Id(), msg.success.ToString()); //DEBUG
        	
        	OnServerResponse(msg);
        }
    }
}