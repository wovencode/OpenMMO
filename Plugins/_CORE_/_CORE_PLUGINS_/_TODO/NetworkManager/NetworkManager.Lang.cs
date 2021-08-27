//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO.Network;
using OpenMMO;
using System;

namespace OpenMMO.Network
{
    ///<summary>Public partial class <c>NetworkManager_Lang</c>that contains actions and related messages</summary>
    [System.Serializable]
    public partial class NetworkManager_Lang
    {
        //CONNECT
        public string DISCONNECTED = "Disconnected.";
        public string ALREADY_CONNECTED = "User is already connected!";
        //LOGIN USER
        public string USER_LOGIN_SUCCESS = ""; // no message here as it would display a popup on every login
        public string USER_LOGIN_FAILURE = "Account Login failed!";
        //REGISTER USER
        public string USER_REGISTER_SUCCESS = "Account Registration successful!";
        public string USER_REGISTER_FAILURE = "Account Registration failed!";
        //CHANGE PASSWORD
        public string USER_CHANGE_PASSWORD_SUCCESS = "Change Password successful!";
        public string USER_CHANGE_PASSWORD_FAILURE = "Change Password failed!";
        //DELETE USER
        public string USER_DELETE_SUCCESS = "Delete Account successful!";
        public string USER_DELETE_FAILURE = "Delete Account failed!";
        //CONFIRM USER
        public string USER_CONFIRM_SUCCESS = "Account confirmed!";
        public string USER_CONFIRM_FAILURE = "Confirm failed!";
        //JOIN CHARACTER
        public string CHARACTER_JOIN_SUCCESS = ""; // no message here as it would display a popup on every login
        public string CHARACTER_JOIN_FAILURE = "Player Login failed!";
        //CREATE CHARACTER
        public string CHARACTER_CREATE_SUCCESS = "Create player successful!";
        public string CHARACTER_CREATE_FAILURE = "Create player failed!";
        //DELETE CHARACTER
        public string CHARACTER_DELETE_SUCCESS = "Delete player successful!";
        public string CHARACTER_DELETE_FAILURE = "Delete player failed!";
        //SWITCH SERVER
        public string SWITCH_SERVER_SUCCESS = "Server switch successful!";
        public string SWITCH_SERVER_FAILURE = "Server switch failed!";
        //ZONING
        public string AUTO_LOGIN_ERROR = "Zoning failed due to an Authentication Error!";
    }
}
