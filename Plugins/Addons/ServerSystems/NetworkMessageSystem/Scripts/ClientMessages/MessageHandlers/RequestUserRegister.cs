//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client
        /// <summary>
        /// Protected override function <c>RequestUserRegister</c> that returns a boolean.
        /// Sends a user registration request to the server.
        /// Checks whether the user register request is valid and can be sent to the server.
        /// Returns a boolean detailing whether the request was sent or not.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="usermail"></param>
        /// <returns> Returns a boolean detailing whether the request was sent to the server. </returns>
        //protected override bool RequestUserRegister(NetworkConnection conn, string userName, string password, string usermail) //REMOVED - DX4D
        protected override bool RequestUserRegister(string userName, string password, string usermail) //ADDED - DX4D
        {
            //if (!base.RequestUserRegister(conn, userName, password, usermail)) //REMOVED - DX4D
            if (!base.RequestUserRegister(userName, password, usermail)) //ADDED - DX4D
                return false;

			ClientRequestUserRegister message = new ClientRequestUserRegister
			{
				username 	= userName,
				password 	= Tools.GenerateHash(name, password),
				email 		= usermail,
				deviceid	= Tools.GetDeviceId
			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserRegister), conn.Id(), userName); //DEBUG
						
			return true;
		}
    }
}