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
        /// Event <c>OnServerMessageResponsePlayerRegister</c>.
        /// Triggered when the client receives a player register response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponsePlayerRegister(NetworkConnection conn, ServerResponsePlayerRegister msg) //REMOVED - DX4D
        void OnServerResponsePlayerRegister(ServerResponsePlayerRegister msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            if (msg.success)
        	{
        		playerPreviews.Add(new PlayerPreview{name=msg.playername});
        		UIWindowPlayerSelect.singleton.UpdatePlayerPreviews(true);
        	}
        	
        	debug.LogFormat(this.name, nameof(OnServerResponsePlayerRegister), conn.Id(), msg.success.ToString()); //DEBUG
        	
        	OnServerResponse(msg);
        }
    }
}