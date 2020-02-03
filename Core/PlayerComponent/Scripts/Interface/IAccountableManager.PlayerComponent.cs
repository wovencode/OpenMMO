
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO;
using OpenMMO.Database;

namespace OpenMMO.Database
{

	// ===================================================================================
	// IAccountableManager
	// ===================================================================================
	public partial interface IAccountableManager
	{
		
		// ---- User
		bool TryUserLogin(string name, string password);
		bool TryUserRegister(string name, string password, string email, string deviceid);
		bool TryUserDelete(string name, string password, int action=1);
		bool TryUserBan(string name, string password, int action=1);
		bool TryUserChangePassword(string name, string oldpassword, string newpassword);
		bool TryUserConfirm(string name, string password, int action=1);
		bool TryUserGetValid(string name, string password);
		bool TryUserGetExists(string name);
		int TryUserGetPlayerCount(string name);
		
		// ---- Player
		bool TryPlayerLogin(string name, string username);
		bool TryPlayerRegister(string name, string username, string prefabname);
		bool TryPlayerDeleteSoft(string name, string username, int action=1);
		bool TryPlayerDeleteHard(string name, string username);
		bool TryPlayerBan(string name, string username, int action=1);
		bool TryPlayerSwitchServer(string name, int _token=0);
		
	}
		
}

// =======================================================================================