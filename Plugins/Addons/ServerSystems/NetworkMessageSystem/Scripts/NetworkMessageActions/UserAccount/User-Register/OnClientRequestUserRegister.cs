//BY FHIZ
// Direction: @Client -> @Server
// Execution: @Server

using OpenMMO.Database;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        /// <summary>
        /// Event <c>OnClientRequestUserRegister</c>.
        /// Triggered when the server receives a user registration request from the client.
        /// </summary>
        /// <param name="conn"></param><param name="msg"></param>
        void OnClientRequestUserRegister(NetworkConnection conn, ClientRequestUserRegister msg)
        {
        	ServerResponseUserRegister message = new ServerResponseUserRegister
            {
                action              = NetworkAction.UserRegister, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
        	if (DatabaseManager.singleton.TryUserRegister(msg.username, msg.password, msg.email, msg.deviceid))
			{
				RegisterUser(msg.username);
				message.text = systemText.userRegisterSuccess;
			}
			else
			{
				message.text = systemText.userRegisterFailure;
				message.success = false;
				
				debug.LogFormat(this.name, nameof(OnClientRequestUserRegister), conn.Id(), "DENIED"); //DEBUG
				
			}

        	conn.Send(message);
        }

		// @Server
		protected void RegisterUser(string username)
		{
			// isNew = true
			// Transaction = false
			DatabaseManager.singleton.SaveDataUser(username, true, false);
            Debug.Log("<color=green><b>Account " + username + " added to database!</b></color>");

            debug.LogFormat(this.name, nameof(RegisterUser), userName); //DEBUG //TODO: Evaluate this - username and userName are not the same variable
		}
    }
}