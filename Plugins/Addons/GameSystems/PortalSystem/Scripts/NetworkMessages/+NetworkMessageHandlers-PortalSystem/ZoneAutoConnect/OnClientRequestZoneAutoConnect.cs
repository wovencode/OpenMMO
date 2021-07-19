//BY FHIZ
//MODIFIED BY DX4D
// Direction: @Server -> @Client
// Execution: @Server

using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        void OnClientRequestZoneAutoConnect(NetworkConnection conn, ClientRequestZoneAutoConnect msg)
		{

			ServerResponseZoneAutoConnect message = new ServerResponseZoneAutoConnect
            {
                action              = NetworkAction.ZoneAutoConnect, //ADDED - DX4D
                success             = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (checkApplicationVersion && msg.clientVersion != Application.version)
			{
				message.text = systemText.versionMismatch;
            	message.success = false;
			}
			else
			{
				base.OnServerAuthenticated.Invoke(conn);
			}
			
			conn.Send(message);
			
			if (!message.success)
			{
				conn.isAuthenticated = false;
				conn.Disconnect();
			}
			
			debug.LogFormat(this.name, nameof(OnClientRequestZoneAutoConnect), conn.Id(), msg.success.ToString(), msg.text); //DEBUG
		
		}
    }
}