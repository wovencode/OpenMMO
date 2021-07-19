//BY FHIZ

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // Direction: @Server -> @Client
        /// <summary>
        /// Event <c>OnServerResponseError</c>.
        /// Triggered when the server sends a response to the client.
        /// Occurs on the client.
        /// Checks for errors.
        /// </summary>
        /// <param name="msg"></param>
        void OnServerResponseError(ServerResponseError msg) //REMOVED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            debug.LogFormat(this.name, nameof(OnServerResponseUserChangePassword), conn.Id(), msg.success.ToString()); //DEBUG
            OnServerResponse(msg);
        }
    }
}