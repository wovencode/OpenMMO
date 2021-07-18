//BY FHIZ
//MODIFIED BY DX4D

using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        // @Server
        /// <summary>
        /// Public override event <c>OnServerConnect</c>.
        /// Does nothing. Waits for the <c>ConnectRequest</c> to be received from the client.
        /// Event occurs on the server. 
        /// </summary>
        /// <param name="conn"></param>
        public override void OnServerAuthenticate(NetworkConnection conn)
        {
            // do nothing...wait for AuthRequestMessage from client
        }
    }
}