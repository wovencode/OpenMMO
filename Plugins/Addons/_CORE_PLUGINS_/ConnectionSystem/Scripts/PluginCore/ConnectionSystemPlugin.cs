//BY DX4D

using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
    /* "OpenMMO/Plugins/Connection System Plugin"*/
	[CreateAssetMenu(menuName = FULL_MENU_PATH, order = 0)]
    public class ConnectionSystemPlugin : ScriptableNetworkPlugin
    {
        const string DESCRIPTION = "Connection System";
        const string CREATE_PATH = "OpenMMO/Plugins/";

        #region P L U G I N  I N F O
        const string CREATE_MENU = DESCRIPTION + " - plugin";
        const string FULL_MENU_PATH = CREATE_PATH + CREATE_MENU;
        [Header("PLUGIN INFO")]
        [SerializeField] string _pluginName = DESCRIPTION;
        public override string PLUGIN_NAME => _pluginName;
        #endregion

        #region M E S S A G E  H A N D L E R S
        //CLIENT HANDLERS
        [Client] internal override void HandleServerMessageOnClient<T>(T msg)
        {
            if (msg is Response.Auth) authenticator.OnServerMessageResponseAuth(msg);
        }
        [Server] internal override void HandleClientMessageOnServer<T>(NetworkConnection conn, T msg)
        {
            if (msg is Request.Auth) authenticator.OnClientMessageRequestAuth(conn, (ClientConnectRequest)msg);
        }
        #endregion

        #region M E S S A G E  H A N D L E R  R E G I S T R Y
        //REGISTER CLIENT HANDLERS
        [Client] internal override void RegisterClientMessageHandlers()
        {
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "CONNECT");
            NetworkClient.RegisterHandler<Response.Auth>(HandleServerMessageOnClient, false);
        }
        //REGISTER SERVER HANDLERS
        [Server] internal override void RegisterServerMessageHandlers()
        {
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "CONNECT");
            NetworkServer.RegisterHandler<Request.Auth>(HandleClientMessageOnServer, false);
        }
        #endregion //MESSAGE HANDLER REGISTRY
    }
}
