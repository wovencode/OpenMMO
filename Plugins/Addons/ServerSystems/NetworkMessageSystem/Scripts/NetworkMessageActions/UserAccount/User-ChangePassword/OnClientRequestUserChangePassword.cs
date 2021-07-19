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
        /// Event <c>OnClientRequestUserChangePassword</c>.
        /// Triggered when the server receives a user change password request.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestUserChangePassword(NetworkConnection conn, ClientRequestUserChangePassword msg)
        {
        	ServerResponseUserChangePassword message = new ServerResponseUserChangePassword
            {
                action              = NetworkAction.UserChangePassword, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
        	if (!GetIsUserLoggedIn(msg.username) && DatabaseManager.singleton.TryUserChangePassword(msg.username, msg.oldPassword, msg.newPassword))
			{
				message.text = systemText.userChangePasswordSuccess;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserChangePassword), conn.Id(), "Success"); //DEBUG
				
			}
			else
			{
				message.text = systemText.userChangePasswordFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserChangePassword), conn.Id(), "DENIED"); //DEBUG
				
			}
					
        	conn.Send(message);
        }
    }
}