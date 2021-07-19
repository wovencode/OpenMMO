//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Zones;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        // Direction: @Client -> @Server
        // Execution: @Server
        /// <summary>
        /// Event <c>OnClientRequestConnect</c>.
        /// Triggered when the server received an authentiation request from a client.
        /// The event attempts to authenticate the client.
        /// Occurs on the server.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="msg"></param>
        void OnClientRequestConnect(NetworkConnection conn, ClientRequestConnect msg)
		{
            ServerResponseConnect message = new ServerResponseConnect
            {
                action = NetworkAction.Connect,
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			// -- Check for Network Portals
			// This prevents players from logging into a Network Zone. Directly logging
			// into a zone should not be possible and can only be done by warping to
			// that zone instead.
			bool portalChecked = true;
            ZoneManager zone = GetComponent<ZoneManager>(); //ADDED - DX4D
                if (zone != null && !zone.GetIsMainZone) portalChecked = false; //ADDED - DX4D

                //if (GetComponent<ZoneManager>() != null && !GetComponent<ZoneManager>().GetIsMainZone) //REMOVED - DX4D
                //    portalChecked = false; //REMOVED - DX4D
			
			if ((checkApplicationVersion && msg.clientVersion != Application.version) || !portalChecked)
			{
				message.text = systemText.versionMismatch;
            	message.success = false;
			}
			else
			{
				base.OnServerAuthenticated.Invoke(conn); //EVENT
#if DEBUG //START DEBUG
                Debug.Log("<color=green><b>Connected to " + conn.address + " successfully!</b></color>"
                    + "\n<b>Connection ID " + conn.connectionId + "</b>");
#endif //END DEBUG
                debug.LogFormat(this.name, nameof(OnClientRequestConnect), conn.Id(), "Authenticated"); //DEBUG
			}
			
			conn.Send(message); //SEND MESSAGE
			
			if (!message.success)
			{
				conn.isAuthenticated = false;
				conn.Disconnect();

#if DEBUG
                Debug.Log("<color=red><b>Connection to " + conn.address  + " failed!</b></color>"
                    + "\n<b>Connection ID " + conn.connectionId + "</b>");
#endif
                debug.LogFormat(this.name, nameof(OnClientRequestConnect), conn.Id(), "DENIED"); //DEBUG
			}
		}      
    }
}