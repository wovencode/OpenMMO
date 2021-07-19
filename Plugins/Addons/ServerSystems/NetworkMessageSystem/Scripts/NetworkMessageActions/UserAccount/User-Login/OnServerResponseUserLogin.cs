//BY FHIZ
// Direction: @Server -> @Client
// Execution: @Client

using OpenMMO.UI;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnServerResponseUserLogin</c>.
        /// Triggered when the client receives a login response from the server.
        /// Checks for the response succes and either shows the player select or auto selects the players.
        /// Occurs on the client.
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponseUserLogin(NetworkConnection conn, ServerResponseUserLogin msg) //REMOVED - DX4D
        void OnServerResponseUserLogin(ServerResponseUserLogin msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D
            
            if (msg.success)
            {
                playerPreviews.Clear();
                playerPreviews.AddRange(msg.players);
                maxPlayers = msg.maxPlayers;

                // -- Show Player Select if there are players
                // -- Show Player Creation if there are no players
                if (msg.players.Length > 0)
                    UIWindowPlayerSelect.singleton.Show();
                else
                    UIWindowPlayerCreate.singleton.Show();

                UIWindowLoginUser.singleton.Hide();

                debug.LogFormat(this.name, nameof(OnServerResponseUserLogin), conn.Id(), msg.players.Length.ToString()); //DEBUG

            }
        	
        	OnServerResponse(msg);
        }
    }
}