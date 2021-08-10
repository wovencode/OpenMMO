//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using OpenMMO.Zones;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
    public partial class NetworkManager
    {
    
        // -------------------------------------------------------------------------------
        // OnStartClient
        // @Client
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Pubilc override event <c>OnStartClient</c>.
        /// Triggered when the client starts.
        /// Occurs on client.
        /// Registers all the user request and server response handlers.
        /// </summary>
        public override void OnStartClient()
        {

            // ---- user account
            // @Server -> @Client
            Debug.Log("[REGISTER NETWORK MESSAGES] - [ACCOUNT CLIENT] - [LOGIN/REGISTER/DELETE] - "
                + "Registering Message Handlers to Client...");
            NetworkClient.RegisterHandler<Response.UserLogin>(OnServerResponseUserLogin);
            NetworkClient.RegisterHandler<Response.UserRegister>(OnServerMessageResponseUserRegister);
            NetworkClient.RegisterHandler<Response.UserDelete>(OnServerMessageResponseUserDelete);
            //TODO: ServerResponseUserLogout

            Debug.Log("[REGISTER NETWORK MESSAGES] - [ACCOUNT CLIENT] - [CHANGEPASSWORD] - "
                + "Registering Message Handlers to Client...");
            NetworkClient.RegisterHandler<Response.UserChangePassword>(OnServerMessageResponseUserChangePassword);

            Debug.Log("[REGISTER NETWORK MESSAGES] - [ACCOUNT CLIENT] - [CONFIRM] - "
                + "Registering Message Handlers to Client...");
            NetworkClient.RegisterHandler<Response.UserConfirm>(OnServerResponseUserConfirm);


            Debug.Log("[REGISTER NETWORK MESSAGES] - [ACCOUNT CLIENT] - [PLAYERPREVIEWS] - "
                + "Registering Message Handlers to Client...");
            NetworkClient.RegisterHandler<Response.UserPlayerPreviews>(OnServerResponseUserPlayerPreviews);
            
            // ---- player character
            // @Server -> @Client
            Debug.Log("[REGISTER NETWORK MESSAGES] - [CHARACTER CLIENT] - [LOGIN/REGISTER/DELETE] - "
                + "Registering Message Handlers to Client...");
            //NetworkClient.RegisterHandler<Response.PlayerLogin>(OnServerResponsePlayerLogin);
            //NetworkClient.RegisterHandler<Response.PlayerRegister>(OnServerResponsePlayerRegister);
            //NetworkClient.RegisterHandler<Response.PlayerDelete>(OnServerResponsePlayerDelete);
            
            // --- errors
            // @Server -> @Client
            Debug.Log("[REGISTER NETWORK MESSAGES] - [CLIENT] - [ERROR] - "
                + "Registering Message Handlers to Client...");
            NetworkClient.RegisterHandler<Response.Error>(OnServerResponseError);
            
            //HOOKS AND EVENTS
            this.InvokeInstanceDevExtMethods(nameof(OnStartClient)); //HOOK
            eventListeners.OnStartClient.Invoke(); //EVENT
        }
    }
}
