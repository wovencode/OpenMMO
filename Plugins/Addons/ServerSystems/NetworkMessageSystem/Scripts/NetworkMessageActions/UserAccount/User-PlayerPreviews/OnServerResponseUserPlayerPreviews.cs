//BY FHIZ
// Direction: @Server -> @Client
// Execution: @Client

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnServerMessageResponseUserPlayerPreviews</c>.
        /// Triggered when the client receives a UserPlayerPreviews response from the server.
        /// Updates the clients Player Previews list.
        /// Occurs on the client.
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponseUserPlayerPreviews(NetworkConnection conn, ServerResponseUserPlayerPreviews msg) //REMOVED - DX4D
        void OnServerResponseUserPlayerPreviews(ServerResponseUserPlayerPreviews msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            if (msg.success)
        	{
				playerPreviews.Clear();
				playerPreviews.AddRange(msg.players);
				maxPlayers	= msg.maxPlayers;
			}
			
			debug.LogFormat(this.name, nameof(OnServerResponseUserPlayerPreviews), conn.Id(), msg.players.Length.ToString()); //DEBUG
			
        	OnServerResponse(msg);
        }
    }
}