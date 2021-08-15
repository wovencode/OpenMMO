//BY DX4D

using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
#region H E L P E R  M E T H O D S
    // --------------------------

    //NETWORK AUTHENTICATOR
    public partial class NetworkAuthenticator
    {
        //CONNECT CLIENT
        void ConnectClient(NetworkConnection conn)
        {
            CancelInvoke();
            base.OnClientAuthenticated.Invoke(conn);

            UI.UIWindowAuth.singleton.Hide();
            //UI.UIWindowAccountOptionsMenu.singleton.Show();
            //UI.UIAccountOptionsMenu.singleton.Show();
            UI.UILoginUserPrompt.singleton.Show(); //SHOW LOGIN WINDOW

            LogConnectionSuccess(conn); //LOG SUCCESS
        }
        //LOG CONNECT SUCCESS
        void LogConnectionSuccess(NetworkConnection conn)
        {
            Debug.Log("<b>[<color=green>CONNECTION CLIENT</color>]</b> - "
                + "<b>Connected successfully to Server!</b>"
                + "\n" + "Connection-" + conn.connectionId + " @" + conn.address + " connecting to Server @" + NetworkClient.serverIp + "...");

            debug.LogFormat(this.name, nameof(OnServerMessageResponseAuth), NetworkClient.connection.Id(), "Authenticated"); //DEBUG
        }

        //DISCONNECT CLIENT
        void DisconnectClient(NetworkConnection conn)
        {
            conn.isAuthenticated = false;
            conn.Disconnect();
            NetworkManager.singleton.StopClient();

            LogConnectionFailure(conn); //LOG FAILURE
        }
        //LOG CONNECT FAILURE
        void LogConnectionFailure(NetworkConnection conn)
        {
#if DEBUG
            Debug.Log("<b>>>>ISSUE<<< [<color=red>CONNECTION CLIENT</color>]</b> - "
                + "<b>Failed to connect to Server...</b>"
                + "\n" + "Connection-" + conn.connectionId + " @" + conn.address + " connecting to Server @" + NetworkClient.serverIp + "...");
#endif
            debug.LogFormat(this.name, nameof(OnServerMessageResponseAuth), NetworkClient.connection.Id(), "DENIED"); //DEBUG
        }
    }
    //NETWORK MANAGER
    public partial class NetworkManager
    {
    }
    #endregion //HELPER METHODS
}
