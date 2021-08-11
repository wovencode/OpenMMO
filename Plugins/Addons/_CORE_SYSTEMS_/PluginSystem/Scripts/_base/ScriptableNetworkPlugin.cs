//BY DX4D

using UnityEngine;
using Mirror;

namespace OpenMMO
{
    public abstract class ScriptableNetworkPlugin : ScriptableObject, INetworkPlugin
    {
        public abstract string PLUGIN_NAME { get; }

        #region INTERNAL - COMPONENT LINKS
        //NETWORK AUTHENTICATOR COMPONENT
        Network.NetworkAuthenticator _authenticator;
        internal Network.NetworkAuthenticator authenticator
        {
            get
            {
                if (!_authenticator) _authenticator = FindObjectOfType<Network.NetworkAuthenticator>();
                return _authenticator;
            }
        }
        //(OpenMMO) NETWORK MANAGER COMPONENT
        Network.NetworkManager _manager;
        internal Network.NetworkManager manager
        {
            get
            {
                if (!_manager) _manager = FindObjectOfType<Network.NetworkManager>();
                return _manager;
            }
        }
        #endregion //COMPONENT LINKS

        #region INTERNAL - CONSTANT STRING DEFINITIONS
        internal const string CLIENT = "CLIENT";
        internal const string SERVER = "SERVER";
        internal const string REGISTER_HANDLER = "REGISTER MESSAGE HANDLER";
        #endregion //CONSTANT STRING DEFINITIONS

        #region INTERNAL - REGISTER CLIENT MESSAGE HANDLERS
        internal abstract void RegisterClientMessageHandlers();
        internal abstract void HandleClientMessageOnServer<T>(NetworkConnection connection, T msg)
            where T : Network.ClientRequest;
        #endregion //REGISTER CLIENT MESSAGE HANDLERS

        #region INTERNAL - REGISTER SERVER MESSAGE HANDLERS
        internal abstract void RegisterServerMessageHandlers();
        internal abstract void HandleServerMessageOnClient<T>(T msg)
            where T : Network.ServerResponse;
        #endregion //REGISTER SERVER MESSAGE HANDLERS

        #region INTERNAL - DEBUG LOG
        const string spacer = " - ";
        internal virtual void Log(string clientOrServer, string actionTaken, string pluginName, string methodName)
        {
            //CLIENT/SERVER + PLUGIN
            string client_server_plugin = (string.IsNullOrWhiteSpace(clientOrServer)) ? ("")
                : ("[" + clientOrServer + " " + pluginName + "]" + spacer);
            //CLIENT OR SERVER
            string client_server = (string.IsNullOrWhiteSpace(clientOrServer)) ? ("")
                : ("[" + clientOrServer + "]");
            //ACTION
            string action_taken = (string.IsNullOrWhiteSpace(actionTaken)) ? ("")
                : ("[" + actionTaken + "]");
            //PLUGIN
            string plugin_name = (string.IsNullOrWhiteSpace(pluginName)) ? ("")
                : ("[" + pluginName + "]");
            //METHOD
            string method_name = (string.IsNullOrWhiteSpace(methodName)) ? ("")
                : ("[" + methodName + "]");

            //MESSAGE
            string message = "";
            //REGISTER HANDLER MESSAGE
            if (action_taken.Contains(REGISTER_HANDLER))
            {
                message += "Registering Message Handlers to " + clientOrServer.ToUpperInvariant() + "...";
            }

            //WRITE DEBUG LOG
            Debug.Log(client_server_plugin + message + "\n" + action_taken + spacer + method_name);
        }
        #endregion //DEBUG LOG

        #region GLOBAL - REGISTER MESSAGE HANDLERS
        //@CLIENT
        //@SERVER
        public void RegisterMessageHandlers()
        {
#if _CLIENT
            RegisterClientMessageHandlers();
#endif
#if _SERVER
            RegisterServerMessageHandlers();
#endif
        }
        #endregion //REGISTER MESSAGE HANDLERS
    }
}