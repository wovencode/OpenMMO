//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // @Client
        //protected override bool RequestUserLogout(NetworkConnection conn) //REMOVED - DX4D
        protected override bool RequestUserLogout() //ADDED - DX4D
		{
		
			ClientRequestUserLogout message = new ClientRequestUserLogout
			{

			};

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            conn.Send(message);
			
			debug.LogFormat(this.name, nameof(RequestUserLogout), conn.Id(), userName); //DEBUG
			
			return true;
		}
    }
}