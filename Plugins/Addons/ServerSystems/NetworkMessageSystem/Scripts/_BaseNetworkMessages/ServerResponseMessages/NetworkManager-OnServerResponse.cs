//BY FHIZ
// Direction: @Server -> @Client
// Execution: @Client

using OpenMMO.UI;
using System;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnServerMessageResponse</c>.
        /// Triggered when the server sends a response to the client.
        /// Occurs on the client.
        /// Checks for errors.
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponse(NetworkConnection conn, ServerResponse msg) //REMOVED - DX4D
        void OnServerResponse(ServerResponse msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            // -- show popup if error message is not empty
            if (!String.IsNullOrWhiteSpace(msg.text))
               	UIPopupConfirm.singleton.Init(msg.text);
    		
        	// -- disconnect and un-authenticate if anything went wrong
            if (msg.causesDisconnect)
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                NetworkManager.singleton.StopClient();
            }
            
            debug.LogFormat(this.name, nameof(OnServerResponse), conn.Id(), msg.causesDisconnect.ToString(), msg.text); //DEBUG
        }
    }
}