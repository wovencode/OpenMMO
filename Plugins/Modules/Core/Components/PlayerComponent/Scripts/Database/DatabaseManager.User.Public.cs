
using System;

namespace OpenMMO.Database
{

	/// <summary>
	/// 
	/// </summary>
	public partial class DatabaseManager
	{
		
		// -------------------------------------------------------------------------------
		// GetUserOnline
		// Checks if the user is online right now, using 'lastonline' time
		// -------------------------------------------------------------------------------
		public bool GetUserOnline(string userName)
		{
			TableUser tableUser = FindWithQuery<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE username=? AND banned=0 AND deleted=0", userName);
			
			if (tableUser == null)
				return false;
			else
			{
                DateTime dateTime = tableUser.lastonline.AddSeconds(logoutInterval);
				return DateTime.Compare(DateTime.UtcNow, dateTime) <= 0;
            }
           
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
		public override bool TryUserDelete(string name, string password, DatabaseAction action = DatabaseAction.Do)
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
		public override bool TryUserBan(string name, string password, DatabaseAction action = DatabaseAction.Do)
		{
			
			if (!base.TryUserBan(name, password) || !UserValid(name, password))
				return false;
				
			UserSetBanned(name, action);
			return true;	
			
		}
		
		// -------------------------------------------------------------------------------
		// TryUserConfirm
		// -------------------------------------------------------------------------------
		public override bool TryUserConfirm(string name, string password, DatabaseAction action = DatabaseAction.Do)
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