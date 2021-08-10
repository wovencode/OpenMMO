//BY DX4D

using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
    [CreateAssetMenu(menuName = FULL_MENU_PATH, order = 0)]
    public class PlayerSystemPlugin : ScriptableNetworkPlugin
    {
        const string DESCRIPTION = "Player System";
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
            if (msg is Response.PlayerLogin) manager.OnServerResponsePlayerLogin(msg);
            if (msg is Response.PlayerRegister) manager.OnServerResponsePlayerRegister((ServerRegisterPlayerResponse)msg);
            if (msg is Response.PlayerDelete) manager.OnServerResponsePlayerDelete(msg);
        }
        [Server] internal override void HandleClientMessageOnServer<T>(NetworkConnection conn, T msg)
        {
            if (msg is Request.PlayerLogin) manager.OnClientMessageRequestPlayerLogin(conn, (ClientLoginPlayerRequest)msg);
            if (msg is Request.PlayerRegister) manager.OnClientMessageRequestPlayerRegister(conn, (ClientRegisterPlayerRequest)msg);
            if (msg is Request.PlayerDelete) manager.OnClientMessageRequestPlayerDelete(conn, (ClientDeletePlayerRequest)msg);
        }
        #endregion

        #region M E S S A G E  H A N D L E R  R E G I S T R Y
        //REGISTER CLIENT HANDLERS
        [Client] internal override void RegisterClientMessageHandlers()
        {
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "LOGIN PLAYER");
            NetworkClient.RegisterHandler<Response.PlayerLogin>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "REGISTER PLAYER");
            NetworkClient.RegisterHandler<Response.PlayerRegister>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "DELETE PLAYER");
            NetworkClient.RegisterHandler<Response.PlayerDelete>(HandleServerMessageOnClient, false);
        }
        //REGISTER SERVER HANDLERS
        [Server] internal override void RegisterServerMessageHandlers()
        {
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "LOGIN PLAYER");
            NetworkServer.RegisterHandler<Request.PlayerLogin>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "REGISTER PLAYER");
            NetworkServer.RegisterHandler<Request.PlayerRegister>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "DELETE PLAYER");
            NetworkServer.RegisterHandler<Request.PlayerDelete>(HandleClientMessageOnServer, false);
        }
        #endregion //MESSAGE HANDLER REGISTRY
    }
}
