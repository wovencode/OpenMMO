//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Zones;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        // -------------------------------------------------------------------------------
        // @Client
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public override event <c>OnClientAuthenticate</c>.
        /// This event is triggered upon requestion authentication.
        /// Invokes a authentication request to trigger.
        /// </summary>
        /// <param name="conn"></param>
        public override void OnClientAuthenticate() //FIX - MIRROR UPDATE - NetworkConnection conn parameter Replaced with NetworkClient.connection - DX4D
        {
        	if (GetComponent<ZoneManager>() != null && !GetComponent<ZoneManager>().GetAutoConnect)
        		Invoke(nameof(ClientConnect), connectDelay);

            this.InvokeInstanceDevExtMethods(nameof(OnClientAuthenticate)); //HOOK //, conn); //FIX - MIRROR UPDATE - conn parameter is no longer passed through - it was replaced with NetworkClient.connection - DX4D
        }
        
		// -------------------------------------------------------------------------------
        // ClientConnect
        // @Client -> @Server
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public Method <c>ClientConnect</c>
        /// Sends a authentication request message from the client to the server.
        /// </summary>
		public void ClientConnect()
		{

            ClientRequestConnect msg = new ClientRequestConnect
            {
                clientVersion = Application.version
            };

#if DEBUG
            Debug.Log("<color=yellow><b>" + this.name + " Connecting to Server...</b></color>"
                + "\nClient @" + NetworkClient.connection.address.ToString() + " connecting to Server @" + NetworkClient.serverIp + "...");
#endif
            NetworkClient.Send<ClientRequestConnect>(msg);
            
            debug.LogFormat(this.name, nameof(ClientConnect)); //DEBUG
		}
    }
}