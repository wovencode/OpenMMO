//BY DX4D

using System.Collections.Generic;
using UnityEngine;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        [Header("PLUGIN SYSTEM")]
        [SerializeField] List<ScriptableNetworkPlugin> _plugins = new List<ScriptableNetworkPlugin>();

        //@CLIENT+SERVER REGISTER NETWORK MESSAGE HANDLERS
        /// <summary><para>Executes on Client and Server</para>
        /// <para>Registers Network Messages to the Client and Server.</para>
        /// <para>Should be Hooked into <see cref="OnStartClient"/> and <see cref="OnStartServer"/></para>
        /// </summary>
        void RegisterMessageHandlers()
        {
            foreach(INetworkPlugin _plugin in _plugins)
            {
                _plugin.RegisterMessageHandlers();
            }
        }
        
        //@CLIENT REGISTER MESSAGE HANDLERS
        [DevExtMethods(nameof(OnStartClient))]
        void OnStartClient_RegisterClientMessageHandlers()
        {
            RegisterMessageHandlers(); //REGISTER HANDLERS TO CLIENT
        }

        //@SERVER REGISTER MESSAGE HANDLERS
        [DevExtMethods(nameof(OnStartServer))]
        void OnStartServer_RegisterServerMessageHandlers()
        {
            RegisterMessageHandlers(); //REGISTER HANDLERS TO SERVER
        }
    }
}
