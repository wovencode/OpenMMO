//by  Fhiz
using System;

namespace OpenMMO.Database
{

	/// <summary>
	/// This partial section of DatabaseManager is exposed to the public and responsible for user (= account) related actions.
	/// </summary>
	public partial class DatabaseManager
	{
		/// <summary>
		/// Checks if the user is online right now, using 'lastonline' time
		/// </summary>
		public bool GetUserOnline(string userName)
		{
		
			TableUser tableUser = FindWithQuery<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE userName=? AND banned=0 AND deleted=0", userName);
			
			if (tableUser == null)
				return false;
			else
			{
                DateTime dateTime = tableUser.lastonline.AddSeconds(logoutInterval);
				return DateTime.Compare(DateTime.UtcNow, dateTime) <= 0;
            }
           
		}
		
		/// <summary>
		/// Tries to login an existing user (= account) with the provided name and password.
		/// </summary>
		public override bool TryUserLogin(string userName, string password)
		{
		
			if (!base.TryUserLogin(userName, password) || !UserValid(userName, password))
				return false;
			
			return true;
			
		}
		
		/// <summary>
		/// Tries to register a new user (= account) using the provided name and password. email and deviceId are optional and used for extra security checks.
		/// </summary>
		public override bool TryUserRegister(string userName, string password, string email, string deviceid)
		{
		
			if (!base.TryUserRegister(userName, password, email, deviceid) || UserExists(userName))
				return false;

			// -- check if maximum amount of users per device reached

			int userCount = GetUserCount(deviceid, email);
			if (userCount >= GameRulesTemplate.singleton.maxUsersPerDevice || userCount >= GameRulesTemplate.singleton.maxUsersPerEmail)
				return false;

			UserRegister(userName, password, email, Tools.GetDeviceId);
			return true;
			
		}
		
		/// <summary>
		/// Tries to delete an existing user (= account) using the provided name and password.
		/// </summary>
		public override bool TryUserDelete(string userName, string password, DatabaseAction action = DatabaseAction.Do)
		{
		
			if (!base.TryUserDelete(userName, password) || !UserValid(userName, password))
				return false;
				
			UserSetDeleted(userName, action);
			return true;	
			
		}
		
		/// <summary>
		/// Tries to change the password of a existing user (= account) using the provided name, password and new password.
		/// </summary>
		public override bool TryUserChangePassword(string userName, string oldpassword, string newpassword)
		{
		
			if (!base.TryUserChangePassword(userName, oldpassword, newpassword) || !UserValid(userName, oldpassword))
				return false;
			
			UserChangePassword(userName, oldpassword, newpassword);
			return true;	
			
		}
		
		/// <summary>
		/// Tries to ban an existing user (= account) using the provided name and password.
		/// </summary>
		public override bool TryUserBan(string userName, string password, DatabaseAction action = DatabaseAction.Do)
		{
			
			if (!base.TryUserBan(userName, password) || !UserValid(userName, password))
				return false;
				
			UserSetBanned(userName, action);
			return true;	
			
		}
		
		/// <summary>
		/// Tries to confirm an existing, non-confirmed user (= account), using the provided name and password.
		/// </summary>
		public override bool TryUserConfirm(string userName, string password, DatabaseAction action = DatabaseAction.Do)
		{
		
			if (!base.TryUserConfirm(userName, password) || !UserValid(userName, password))
				return false;
				
			UserSetConfirmed(userName, action);
			return true;	
			
		}
		
		/// <summary>
		/// Tries to check if a existing user is valid (not banned or deleted) using the provided name and password.
		/// </summary>
		public override bool  TryUserGetValid(string userName, string password)
		{
			if (!base.TryUserGetValid(userName, password))
				return false;
		
			return UserValid(userName, password);
			
		}
		
		/// <summary>
		/// Tries to check if a user (= account) of the provided name exists.
		/// </summary>
		public override bool  TryUserGetExists(string userName)
		{
			if (!base.TryUserGetExists(userName))
				return false;
			
			return UserExists(userName);
			
		}
		
		/// <summary>
		/// Tries to get the number of players (= characters) of this user (= account).
		/// </summary>
		public override int TryUserGetPlayerCount(string userName)
		{
			if (!Tools.IsAllowedName(userName))
				return 0;
			
			return GetPlayerCount(userName);
		}
		
	}

}