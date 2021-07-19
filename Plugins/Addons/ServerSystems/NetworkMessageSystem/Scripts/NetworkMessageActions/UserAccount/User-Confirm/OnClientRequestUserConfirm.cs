//BY FHIZ
// Direction: @Client -> @Server
// Execution: @Server

using OpenMMO.Database;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnClientRequestUserConfirm</c>.
        /// Triggered by the server receiving a user confirmation request from the client.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestUserConfirm(NetworkConnection conn, ClientRequestUserConfirm msg)
        {
        	ServerResponseUserConfirm message = new ServerResponseUserConfirm
            {
                action              = NetworkAction.UserConfirm, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserConfirm(msg.username, msg.password))
			{
				message.text = systemText.userConfirmSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserConfirm), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.userConfirmFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserConfirm), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        }
    }
}