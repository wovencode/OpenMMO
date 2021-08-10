//BY DX4D

using System;
using Mirror;
using UnityEngine;
using OpenMMO.Database;

namespace OpenMMO.Network
{
    [CreateAssetMenu(menuName = "OpenMMO/Plugins/Character System Plugin", order = 0)]
    public class CharacterSystemPlugin : ScriptableNetworkPlugin
    {
        [Header("PLUGIN INFO")]
        [SerializeField] string _pluginName = "CHARACTER SYSTEM";
        public override string PLUGIN_NAME => _pluginName;
        
        #region M E S S A G E  H A N D L E R S
        //CLIENT HANDLERS
        [Client]
        internal override void HandleServerMessageOnClient<T>(T msg)
        {
            if (msg is Response.PlayerLogin) manager.OnServerResponsePlayerLogin(msg);
            if (msg is Response.PlayerRegister) manager.OnServerResponsePlayerRegister((ServerRegisterPlayerResponse)msg);
            if (msg is Response.PlayerDelete) manager.OnServerResponsePlayerDelete(msg);
        }
        [Server]
        internal override void HandleClientMessageOnServer<T>(NetworkConnection conn, T msg)
        {
            if (msg is Request.PlayerLogin) manager.OnClientMessageRequestPlayerLogin(conn, (ClientLoginPlayerRequest)msg);
            if (msg is Request.PlayerRegister) manager.OnClientMessageRequestPlayerRegister(conn, (ClientRegisterPlayerRequest)msg);
            if (msg is Request.PlayerDelete) manager.OnClientMessageRequestPlayerDelete(conn, (ClientDeletePlayerRequest)msg);
        }
        #endregion

        #region M E S S A G E  H A N D L E R  R E G I S T R Y
        //REGISTER CLIENT HANDLERS
        [Client]
        internal override void RegisterClientMessageHandlers()
        {
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "LOGIN");
            NetworkClient.RegisterHandler<Response.PlayerLogin>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "REGISTER");
            NetworkClient.RegisterHandler<Response.PlayerRegister>(HandleServerMessageOnClient, false);
            Log(CLIENT, REGISTER_HANDLER, PLUGIN_NAME, "DELETE");
            NetworkClient.RegisterHandler<Response.PlayerDelete>(HandleServerMessageOnClient, false);
        }
        //REGISTER SERVER HANDLERS
        [Server]
        internal override void RegisterServerMessageHandlers()
        {
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "LOGIN");
            NetworkServer.RegisterHandler<Request.PlayerLogin>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "REGISTER");
            NetworkServer.RegisterHandler<Request.PlayerRegister>(HandleClientMessageOnServer, false);
            Log(SERVER, REGISTER_HANDLER, PLUGIN_NAME, "DELETE");
            NetworkServer.RegisterHandler<Request.PlayerDelete>(HandleClientMessageOnServer, false);
        }
        #endregion //MESSAGE HANDLER REGISTRY
    }
}
