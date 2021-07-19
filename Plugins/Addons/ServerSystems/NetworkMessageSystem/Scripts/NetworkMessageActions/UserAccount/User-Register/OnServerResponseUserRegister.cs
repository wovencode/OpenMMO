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
        /// Event <c>OnServerMessageResponseUserRegister</c>.
        /// Triggered when the client receives a register response from the server.
        /// Checks whether the register request was succesful. 
        /// Doesn't login the player. To log the player in another request has to be made.
        /// Occurs on the client.
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerMessageResponseUserRegister(NetworkConnection conn, ServerResponseUserRegister msg) //REMOVED - DX4D
        void OnServerMessageResponseUserRegister(ServerResponseUserRegister msg) //ADDED - DX4D
        {
            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            // -- hide user registration window if succeeded
            if (msg.success)
        	{
        		UIWindowRegisterUser.singleton.Hide();
        	}
        	
        	debug.LogFormat(this.name, nameof(OnServerMessageResponseUserRegister), conn.Id(), msg.success.ToString()); //DEBUG
        	
        	OnServerResponse(msg);
        }
    }
}