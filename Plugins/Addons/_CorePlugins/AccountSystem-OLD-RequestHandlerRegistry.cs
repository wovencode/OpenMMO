//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Database;
using UnityEngine;
using System;
using Mirror;

namespace OpenMMO.Network
{

    // ===================================================================================
    // NetworkManager
    // ===================================================================================
    public partial class NetworkManager
    {

        // -------------------------------------------------------------------------------
        // OnStartServer
        // @Server
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public Event <c>OnStartServer</c>.
        /// Triggered when the server starts.
        /// Registers all the server's event handlers.
        /// Runs on server.
        /// </summary>
        public override void OnStartServer()
        {

            // ---- user account
            // @Client -> @Server
            Debug.Log("[REGISTER NETWORK MESSAGES] - [ACCOUNT SERVER] - [LOGIN/REGISTER/DELETE/LOGOUT] - "
                + "Registering Message Handlers to Server...");
            NetworkServer.RegisterHandler<Request.UserLogin>(OnClientMessageRequestUserLogin);
            NetworkServer.RegisterHandler<Request.UserRegister>(OnClientMessageRequestUserRegister);
            NetworkServer.RegisterHandler<Request.UserDelete>(OnClientMessageRequestUserDelete);
            NetworkServer.RegisterHandler<Request.UserLogout>(OnClientMessageRequestUserLogout);

            Debug.Log("[REGISTER NETWORK MESSAGES] - [ACCOUNT SERVER] - [CHANGEPASSWORD] - "
                + "Registering Message Handlers to Server...");
            NetworkServer.RegisterHandler<Request.UserChangePassword>(OnClientMessageRequestUserChangePassword);

            Debug.Log("[REGISTER NETWORK MESSAGES] - [ACCOUNT SERVER] - [CONFIRM] - "
                + "Registering Message Handlers to Server...");
            NetworkServer.RegisterHandler<Request.UserConfirm>(OnClientMessageRequestUserConfirm);

            // ---- player character
            // @Client -> @Server
            Debug.Log("[REGISTER NETWORK MESSAGES] - [CHARACTER SERVER] - [LOGIN/REGISTER/DELETE] - "
                + "Registering Message Handlers to Server...");
            //NetworkServer.RegisterHandler<Request.PlayerLogin>(OnClientMessageRequestPlayerLogin);
            //NetworkServer.RegisterHandler<Request.PlayerRegister>(OnClientMessageRequestPlayerRegister);
            //NetworkServer.RegisterHandler<Request.PlayerDelete>(OnClientMessageRequestPlayerDelete);

            //HOOKS AND EVENTS
            this.InvokeInstanceDevExtMethods(nameof(OnStartServer)); //HOOK
            eventListeners.OnStartServer.Invoke(); //EVENT
        }
    }
}
