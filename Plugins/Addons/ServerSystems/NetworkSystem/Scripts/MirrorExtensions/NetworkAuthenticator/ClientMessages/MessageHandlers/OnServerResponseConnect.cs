//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.UI;
using System;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        /// <summary>
        /// Event <c>OnServerResponseConnect</c>.
        /// Is triggered when the server returns a authentication response message.
        /// Either authenicates the client, disconnects the client and returns an error message if there is one. 
        /// If the authentication was succesful the client is readied.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        //void OnServerMessageResponseAuth(NetworkConnection conn, ServerMessageResponseAuth msg) //REMOVED - MIRROR UPDATE - DX4D
        void OnServerResponseConnect(ServerResponseConnect msg) //ADDED - MIRROR UPDATE - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - MIRROR UPDATE - DX4D

        	// -- show popup if error message is not empty
        	if (!String.IsNullOrWhiteSpace(msg.text))
               	UIPopupConfirm.singleton.Init(msg.text);
    		
        	// -- disconnect and un-authenticate if anything went wrong
            if (msg.causesDisconnect)//!msg.success || 
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                NetworkManager.singleton.StopClient();
                
                debug.LogFormat(this.name, nameof(OnServerResponseConnect), conn.Id(), "DENIED"); //DEBUG
            }
            
            // -- ready client
            if (msg.success)// && !msg.causesDisconnect
            {
            	CancelInvoke();
               	base.OnClientAuthenticated.Invoke(conn);
               	
               	UIWindowAuth.singleton.Hide();
               	UIWindowMain.singleton.Show();
               	
				debug.LogFormat(this.name, nameof(OnServerResponseConnect), conn.Id(), "Authenticated"); //DEBUG
            }
        }
    }
}