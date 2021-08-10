
using OpenMMO.Network;
using OpenMMO;
using System;

namespace OpenMMO.Network
{
	
	// -----------------------------------------------------------------------------------
	// NetworkManager_Lang
	// -----------------------------------------------------------------------------------
    /// <summary>
    /// Public partial class <c>NetworkManager_Lang</c>that contains actions and related messages
    /// </summary>
	[System.Serializable]
	public partial class NetworkManager_Lang
	{
		public string clientDisconnected 		= "Disconnected.";
		public string userAlreadyOnline			= "User is already online!";
		
		public string userLoginSuccess 			= ""; // no message here as it would display a popup on every login
		public string userLoginFailure 			= "Account Login failed!";
		public string userRegisterSuccess 		= "Account Registration successful!";
		public string userRegisterFailure 		= "Account Registration failed!";
		public string userChangePasswordSuccess	= "Change Password successful!";
		public string userChangePasswordFailure	= "Change Password failed!";
		public string userDeleteSuccess 		= "Delete Account successful!";
		public string userDeleteFailure 		= "Delete Account failed!";
		public string userConfirmSuccess 		= "Account confirmed!";
		public string userConfirmFailure 		= "Confirm failed!";
		public string playerLoginSuccess 		= ""; // no message here as it would display a popup on every login
		public string playerLoginFailure 		= "Player Login failed!";
		public string playerRegisterSuccess 	= "Create player successful!";
		public string playerRegisterFailure 	= "Create player failed!";
		public string playerDeleteSuccess 		= "Delete player successful!";
		public string playerDeleteFailure 		= "Delete player failed!";
		public string playerSwitchServerSuccess = "Server switch successful!";
		public string playerSwitchServerFailure = "Server switch failed!";
		
		public string unknownError				= "Unknown error."; // usually used for hacking attempts
		
	}
	
}

// =======================================================================================