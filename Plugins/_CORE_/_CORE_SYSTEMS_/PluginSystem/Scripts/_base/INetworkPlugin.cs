//BY DX4D

namespace OpenMMO
{
    public interface INetworkPlugin
    {
        /// <summary>Executes on Client or Server - Registers Network Messages used by this Plugin
        /// <para>Implemented by <see cref="ScriptableNetworkPlugin"/>
        /// which executes Plugin Methods: 
        /// <see cref="ScriptableNetworkPlugin.RegisterClientMessageHandlers"/>
        ///  and 
        /// <see cref="ScriptableNetworkPlugin.RegisterServerMessageHandlers"/>
        /// </para></summary>
        void RegisterMessageHandlers();
        //void RegisterClientMessageHandlers();
        //void RegisterServerMessageHandlers();
    }
}
