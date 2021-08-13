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
        /// </summary>
        /// <returns> Returns a boolean value detailing whether the network related button can be clicked at that moment. </returns>
        public bool CanClick()
        {
            return (isNetworkActive && IsConnecting());
        }

        // -------------------------------------------------------------------------------
        // CanCancel
        // @Client
        // can we cancel what we are currently doing?
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>CanCancel</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the network related action can be canceld at the moment.
        /// </summary>
        /// <returns> Returns a boolean value detailing whether the network related action can be canceled </returns>
        public bool CanCancel()
        {
            return IsConnecting();
        }

        // -------------------------------------------------------------------------------
        // CanLoginUser
        // @Client
        // can we login into an existing user with the provided name and password?
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>CanLoginUser</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the user can login checks the username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> Returns a boolean value detailing whether the user can login </returns>
        public bool CanLoginUser(string username, string password)
        {
            return isNetworkActive &&
                Tools.IsAllowedName(username) &&
                Tools.IsAllowedPassword(password) &&
                IsConnecting();
        }

        // -------------------------------------------------------------------------------
        // CanRegisterUser
        // @Client
        // can we register a new user with the provided name and password?
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>CanRegisterUser</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the user can register checks the username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> Returns a boolean value detailing whether the user can register </returns>
        public bool CanRegisterUser(string username, string password)
        {
            return isNetworkActive &&
                Tools.IsAllowedName(username) &&
                Tools.IsAllowedPassword(password) &&
                IsConnecting();
        }

        // -------------------------------------------------------------------------------
        // CanDeleteUser
        // @Client
        // can we delete an user with the provided name and password?
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>CanDeleteUser</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the user can be deleted checks the username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> Returns a boolean value detailing whether the user can be deleted </returns>
        public bool CanDeleteUser(string username, string password)
        {
            return isNetworkActive &&
                Tools.IsAllowedName(username) &&
                Tools.IsAllowedPassword(password) &&
                IsConnecting();
        }

        // -------------------------------------------------------------------------------
        // CanChangePasswordUser
        // @Client
        // can we change the provided users password?
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>CanChangePasswordUser</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the user's password can be changed
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        /// <returns>  Returns a boolean value detailing whether the user can change their password </returns>
        public bool CanChangePasswordUser(string username, string oldpassword, string newpassword)
        {
            return isNetworkActive &&
                Tools.IsAllowedName(username) &&
                Tools.IsAllowedPassword(oldpassword) &&
                Tools.IsAllowedPassword(newpassword) &&
                IsConnecting();
        }

        // -------------------------------------------------------------------------------
        // CanStartServer
        // @Client
        // can we start a server (host only) right now?
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>CanStartServer</c>.
        /// Run on the client.
        /// Return a boolean value detailing whether the the client can start a server.
        /// Runs when using a host and play topology.
        /// </summary>
        /// <returns> Returns a boolean value detailing whether the server can be started </returns>
        public bool CanStartServer()
        {
            return (Application.platform != RuntimePlatform.WebGLPlayer &&
                    !isNetworkActive &&
                    !IsConnecting());
        }

        // ======================= PUBLIC METHODS - PLAYER ===============================

        // -------------------------------------------------------------------------------
        // CanRegisterPlayer
        // @Client
        // can we register a new player with the provided name?
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>CanRegisterPlayer</c>.
        /// Checks whether the player can be registered with the provided name.
        /// Runs on the client.
        /// </summary>
        /// <param name="playername"></param>
        /// <returns></returns>
        public bool CanRegisterPlayer(string playername)
        {
            return Tools.IsAllowedName(playername);
        }
    }
}