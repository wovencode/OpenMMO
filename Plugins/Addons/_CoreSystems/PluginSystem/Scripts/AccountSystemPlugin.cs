//BY DX4D

using Mirror;
using UnityEngine;

namespace OpenMMO
{
    [CreateAssetMenu(menuName = "OpenMMO/Plugins/Account System Plugin", order = 0)]
    public class AccountSystemPlugin : ScriptableNetworkPlugin
    {
        [Header("PLUGIN INFO")]
        [SerializeField] string _pluginName = "USER ACCOUNT SYSTEM";
        public override string PLUGIN_NAME => _pluginName;

        internal override void HandleClientMessageOnServer<T>(NetworkConnection connection, T msg)
        {
            throw new System.NotImplementedException();
        }

        internal override void HandleServerMessageOnClient<T>(T msg)
        {
            throw new System.NotImplementedException();
        }

        internal override void RegisterClientMessageHandlers()
        {
            throw new System.NotImplementedException();
        }

        internal override void RegisterServerMessageHandlers()
        {
            throw new System.NotImplementedException();
        }
    }
}
