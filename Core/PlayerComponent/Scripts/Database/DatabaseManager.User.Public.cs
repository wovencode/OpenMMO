
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		
		// -------------------------------------------------------------------------------
		// GetUserOnline
		// 
		// -------------------------------------------------------------------------------
		public bool GetUserOnline(string userName)
		{
			TableUser tableUser = FindWithQuery<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE username=? AND banned=0 AND deleted=0", userName);
			
			if (tableUser.lastlogin == DateTime.MinValue)
				debug.Log("[TIME IS MINVALUE]");
			
			if (tableUser == null || (tableUser != null && tableUser.lastlogin == DateTime.MinValue))
			{
				return false;
			}
			else
			{
                DateTime nextAllowedLoginTime = tableUser.lastlogin.AddSeconds(saveInterval); //NEW

                TimeSpan timesincelastlogin = (DateTime.Now.ToUniversalTime() - tableUser.lastlogin);
                double timePassed = timesincelastlogin.TotalSeconds;
                Debug.Log("<b>[LOGIN TIME STAMP]</b>"
                    + "\n last login time:" + tableUser.lastlogin
                    + "\n next login time:" + nextAllowedLoginTime

                    + "\n\n current local time:" + DateTime.Now
                    + "\n current UTC time:" + DateTime.Now.ToUniversalTime()

                    + "\n\n duration until next login:" + timesincelastlogin
                    + "\n time since last login:" + timePassed
                    + "\n save interval:" + saveInterval);


                return DateTime.Now.ToUniversalTime() <= nextAllowedLoginTime; //NEW
                //return timePassed <= saveInterval; //OLD
            }
            //END TEST
			
		}
		
		// ============================== PUBLIC METHODS =================================
		
		// -------------------------------------------------------------------------------
		// TryUserLogin
		// -------------------------------------------------------------------------------
		public override bool TryUserLogin(string name, string password)
		{
		
			if (!base.TryUserLogin(name, password) || !UserValid(name, password))
				return false;
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
		// TryUserRegister
		// -------------------------------------------------------------------------------
		public override bool TryUserRegister(string name, string password, string email, string deviceid)
		{
		
			if (!base.TryUserRegister(name, password, email, deviceid) || UserExists(name))
				return false;

			// -- check if maximum amount of users per device reached

			int userCount = GetUserCount(deviceid, email);
			if (userCount >= GameRulesTemplate.singleton.maxUsersPerDevice || userCount >= GameRulesTemplate.singleton.maxUsersPerEmail)
				return false;

			UserRegister(name, password, email, Tools.GetDeviceId);
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
		// TryUserDelete
		// -------------------------------------------------------------------------------
		public override bool TryUserDelete(string name, string password, int action=1)
		{
		
			if (!base.TryUserDelete(name, password) || !UserValid(name, password))
				return false;
				
			UserSetDeleted(name, action);
			return true;	
			
		}
		
		// -------------------------------------------------------------------------------
		// TryUserChangePassword
		// -------------------------------------------------------------------------------
		public override bool TryUserChangePassword(string name, string oldpassword, string newpassword)
		{
		
			if (!base.TryUserChangePassword(name, oldpassword, newpassword) || !UserValid(name, oldpassword))
				return false;
			
			UserChangePassword(name, oldpassword, newpassword);
			return true;	
			
		}
		
		// -------------------------------------------------------------------------------
		// TryUserBan
		// -------------------------------------------------------------------------------
		public override bool TryUserBan(string name, string password, int action=1)
		{
			
			if (!base.TryUserBan(name, password) || !UserValid(name, password))
				return false;
				
			UserSetBanned(name, action);
			return true;	
			
		}
		
		// -------------------------------------------------------------------------------
		// TryUserConfirm
		// -------------------------------------------------------------------------------
		public override bool TryUserConfirm(string name, string password, int action=1)
		{
		
			if (!base.TryUserConfirm(name, password) || !UserValid(name, password))
				return false;
				
			UserSetConfirmed(name, action);
			return true;	
			
		}
		
		// -------------------------------------------------------------------------------
		// TryUserGetValid
		// -------------------------------------------------------------------------------
		public override bool  TryUserGetValid(string name, string password)
		{
			if (!base.TryUserGetValid(name, password))
				return false;
		
			return UserValid(name, password);
			
		}
		
		// -------------------------------------------------------------------------------
		// TryUserGetExists
		// -------------------------------------------------------------------------------
		public override bool  TryUserGetExists(string name)
		{
			if (!base.TryUserGetExists(name))
				return false;
			
			return UserExists(name);
			
		}
		
		// -------------------------------------------------------------------------------
		// TryUserGetPlayerCount
		// -------------------------------------------------------------------------------
		public override int TryUserGetPlayerCount(string name)
		{
			if (!Tools.IsAllowedName(name))
				return 0;
			
			return GetPlayerCount(name);
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================