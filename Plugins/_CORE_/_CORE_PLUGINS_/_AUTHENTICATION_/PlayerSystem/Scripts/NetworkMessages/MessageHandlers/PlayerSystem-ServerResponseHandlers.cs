//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Zones;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

using OpenMMO.UI;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
    public partial class NetworkManager
    {
        // ======================== MESSAGE HANDLERS - PLAYER ============================

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerLogin</c>.
        /// Triggered when the client receives a player login response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponsePlayerLogin(NetworkConnection conn, ServerResponsePlayerLogin msg) //REMOVED - DX4D
        internal void OnServerResponsePlayerLogin(ServerResponse msg) //ADDED - DX4D
        {
            //NetworkConnection conn = NetworkClient.connection; //REMOVED ADDED - DX4D

            //debug.LogFormat(this.name, nameof(OnServerResponsePlayerLogin), conn.Id(), msg.success.ToString()); //DEBUG //REMOVED DX4D
        	
        	OnServerResponse(msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerRegister</c>.
        /// Triggered when the client receives a player register response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponsePlayerRegister(NetworkConnection conn, ServerResponsePlayerRegister msg) //REMOVED - DX4D
        internal void OnServerResponsePlayerRegister(ServerRegisterPlayerResponse msg) //ADDED - DX4D
        {
            //NetworkConnection conn = NetworkClient.connection; //REMOVED ADDED - DX4D

            if (msg.success)
        	{
                playerPreviews.Add(new PlayerPreview { playername = msg.playername, prefabname = msg.prefabname });
        		UIWindowPlayerSelect.singleton.UpdatePlayerPreviews(true);
        	}
        	
        	//debug.LogFormat(this.name, nameof(OnServerResponsePlayerRegister), conn.Id(), msg.success.ToString()); //DEBUG //REMOVED - DX4D
        	
        	OnServerResponse(msg);
        }

        // -------------------------------------------------------------------------------
        /// <summary>
        /// Event <c>OnServerMessageResponsePlayerDelete</c>.
        /// Triggered when the client receives a player delete response from the server.
        /// Triggers the <c>OnServerMessageResponse</c> event.
        /// Occurs on the client.        
        /// </summary>
        /// <param name="msg"></param>
        //void OnServerResponsePlayerDelete(NetworkConnection conn, ServerResponsePlayerDelete msg) //REMOVED - DX4D
        internal void OnServerResponsePlayerDelete(ServerResponse msg) //ADDED - DX4D
        {
            //NetworkConnection conn = NetworkClient.connection; //REMOVED ADDED - DX4D

            //debug.LogFormat(this.name, nameof(OnServerResponsePlayerDelete), conn.Id(), msg.success.ToString()); //DEBUG //REMOVED DX4D

            OnServerResponse(msg);
        }
    }
}
