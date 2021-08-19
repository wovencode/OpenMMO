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

    // ===================================================================================
    // NetworkManager
    // ===================================================================================
    public partial class NetworkManager
    {

        // ======================= PUBLIC METHODS - USER =================================

        // -------------------------------------------------------------------------------
        // CanClick
        // @Client
        // can any network related button be clicked at the moment?
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>CanClick</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the network related button can be clicked at that moment.
        /// Called by <see cref="UI.UIAccountMenu"/>
        /// </summary>
        /// <returns> Returns a boolean value detailing whether the network related button can be clicked at that moment. </returns>
        public bool CanClick()
        {
            return (isNetworkActive && IsConnecting());
        }
    }
}