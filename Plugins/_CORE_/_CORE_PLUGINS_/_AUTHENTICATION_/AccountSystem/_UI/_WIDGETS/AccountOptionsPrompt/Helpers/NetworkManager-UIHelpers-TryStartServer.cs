//BY FHIZ
//MODIFIED BY DX4D

// =======================================================================================
// NetworkManager
// by Weaver (Fhiz)
// MIT licensed
//
// This part of the NetworkManager contains all public functions. That comprises all
// methods that are called on the NetworkManager from UI elements in order to check for
// an action or perform an action (like "Can we register an account with password X and
// name Y?" or "Now register an account with password X and name Y").
//
// =======================================================================================

using Mirror;
using OpenMMO;
using OpenMMO.Network;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace OpenMMO.Network
{
    public partial class NetworkManager
    {
        // S T A R T  S E R V E R

        /// <summary>Try to start a server (host only) if possible
        /// Public function <c>TryStartServer</c>.
        /// Tries to start the server.
        /// Occurs only when running a host and play topology.
        /// Runs on the client.
        /// </summary>
        // @Client
        public void TryStartServer()
        {
            if (!CanStartServer()) return;
            StartServer();
        }
        /// <summary>Can we start a server (host only) right now?
        /// Public function <c>CanStartServer</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the client can start a server.
        /// Runs when using a host and play topology.
        /// </summary>
        /// <returns> Returns a boolean value detailing whether the server can be started </returns>
        // @Client
        public bool CanStartServer()
        {
            return (Application.platform != RuntimePlatform.WebGLPlayer &&
                    !isNetworkActive &&
                    !IsConnecting());
        }

        /* //DEPRECIATED - unused
        /// <summary>Try to cancel whatever we are doing right now
        /// Public function <c>TryCancel</c>.
        /// Tries to cancel what they are doing right now.
        /// Runs on the client.
        /// </summary>
        // @Client
        public void TryCancel()
		{
			StopClient();
		}*/

        /* //DEPRECIATED - unused
        // -------------------------------------------------------------------------------
        // CanCancel
        // @Client
        // can we cancel what we are currently doing?
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>CanCancel</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the network related action can be canceld at the moment.
        /// 
        /// </summary>
        /// <returns> Returns a boolean value detailing whether the network related action can be canceled </returns>
        public bool CanCancel()
        {
            return IsConnecting();
        }*/
    }
}
