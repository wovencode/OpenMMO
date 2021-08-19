//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // L O G O U T  U S E R

        /// <summary>
        /// Try to logout an existing user on the current connection
        /// </summary>
        // @Client
        public void TryLogoutUser()
        {
            //RequestUserLogout(NetworkClient.connection); //REMOVED - DX4D
            RequestUserLogout(); //ADDED - DX4D
        }
        // @Client
        //protected override bool RequestUserLogout(NetworkConnection conn) //REMOVED - DX4D
        protected override bool RequestUserLogout() //ADDED - DX4D
		{
            if (!CanUserLogout()) return false; //ADDED - DX4D

            Request.UserLogoutRequest message = new Request.UserLogoutRequest
			{

			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserLogout), conn.Id(), userName); //DEBUG
			
			return true;

		}
    }
}
