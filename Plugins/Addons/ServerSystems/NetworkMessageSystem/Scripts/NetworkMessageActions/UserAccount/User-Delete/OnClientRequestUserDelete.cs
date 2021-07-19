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
        /// Event <c>OnClientRequestUserDelete</c>.
        /// Triggered when the server receives a user deletion request.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestUserDelete(NetworkConnection conn, ClientRequestUserDelete msg)
        {
        	ServerResponseUserDelete message = new ServerResponseUserDelete
            {
                action              = NetworkAction.UserDelete, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
        	if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserDelete(msg.username, msg.password))
			{
				message.text = systemText.userDeleteSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserDelete), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.userDeleteFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserDelete), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        }
    }
}