//BY Fhiz
//MODIFIED BY DX4D

using OpenMMO.UI;
using OpenMMO.Zones;
using UnityEngine;
using System;
using Mirror;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
		// Direction: @Server -> @Client
		// Execution: @Client
        //void OnServerMessageResponseAutoAuth(NetworkConnection conn, ServerResponseAutoAuth msg) //REMOVED - DX4D
        void OnServerResponseAutoAuth(ServerResponseZoneAutoConnect msg) //ADDED - DX4D
        {
        	
        	// -- show popup if error message is not empty
        	if (!String.IsNullOrWhiteSpace(msg.text))
               	UIPopupConfirm.singleton.Init(msg.text);

            NetworkConnection conn = NetworkClient.connection; //ADDED - DX4D

            // -- disconnect and un-authenticate if anything went wrong
            if (!msg.success || msg.causesDisconnect)
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                NetworkManager.singleton.StopClient();
            }
            
            // -- ready client
            if (msg.success && !msg.causesDisconnect)
            {
            	CancelInvoke();
               	base.OnClientAuthenticated.Invoke(conn);
               	//ClientScene.Ready(conn); //REMOVED - DX4D
               	NetworkClient.Ready(); //ADDED - DX4D
            }
            
            debug.LogFormat(this.name, nameof(OnServerResponseAutoAuth), msg.success.ToString(), msg.text); //DEBUG
        	
        }

        // Direction: @Client -> @Server
        // Execution: @Client
		public void ClientAutoAuthenticate()
		{

            ClientRequestZoneAutoConnect msg = new ClientRequestZoneAutoConnect
            {
                action = NetworkAction.ZoneAutoConnect,
                clientVersion = Application.version
            };

            NetworkClient.Send(msg);
            
		}
		
        // @Client
       	[DevExtMethods(nameof(OnClientAuthenticate))]
        void OnClientAuthenticate_NetworkPortals() //FIX - MIRROR UPDATE - NetworkConnection conn parameter Replaced with NetworkClient.connection - DX4D
        {
        	if (GetComponent<ZoneManager>() != null && GetComponent<ZoneManager>().GetAutoConnect)
        		Invoke(nameof(ClientAutoAuthenticate), connectDelay);		 
        }
    	
        // @Client
        public void OnClientAuthenticated_NetworkPortals(NetworkConnection conn)
        {
        	ZoneManager.singleton.AutoLogin();
        }
    }
}