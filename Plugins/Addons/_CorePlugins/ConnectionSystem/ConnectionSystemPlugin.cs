//BY DX4D

using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
    [CreateAssetMenu(menuName = "OpenMMO/Plugins/Connection System Plugin", order = 0)]
    public class ConnectionSystemPlugin : ScriptableNetworkPlugin
    {
        [Header("PLUGIN INFO")]
        [SerializeField] string _pluginName = "CONNECTION SYSTEM";
        public override string PLUGIN_NAME => _pluginName;

        #region M E S S A G E  H A N D L E R S
        //CLIENT HANDLERS
        [Client]
        internal override void HandleServerMessageOnClient<T>(T msg)
        {
            if (msg is Response.Auth) authenticator.OnServerMessageResponseAuth(msg);
        }
        [Server]
        internal override void HandleClientMessageOnServer<T>(NetworkConnection conn, T msg)
        {
            if (msg is Request.Auth) authenticator.OnClientMessageRequestAuth(conn, (ClientConnectRequest)msg);
        }
        #endregion

        #region M E S S A G E  H A N D L E R  R E G I S T R Y
        //REGISTER CLIENT HANDLERS
        [Client]
        internal override void RegisterClientMessageHandlers()
        {
            //#if _CLIENT
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "CONNECT");
            NetworkClient.RegisterHandler<Response.Auth>(HandleServerMessageOnClient, false);
            //Debug.Log("[CLIENT] - [REGISTER MESSAGE HANDLERS] - [CONNECTION SYSTEM] - [CONNECT] - "
            //    + "Registering Message Handlers to Client...");
            //#endif //_CLIENT
        }
        //REGISTER SERVER HANDLERS
        [Server]
        internal override void RegisterServerMessageHandlers()
        {
            //#if _SERVER
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "CONNECT");
            NetworkServer.RegisterHandler<Request.Auth>(HandleClientMessageOnServer, false);
            //Debug.Log("[SERVER] - [REGISTER MESSAGE HANDLERS] - [CONNECTION SYSTEM] - [CONNECT] - "
            //    + "Registering Message Handlers to Server...");
            //#endif //_SERVER
        }
        #endregion //MESSAGE HANDLER REGISTRY
    }
}
