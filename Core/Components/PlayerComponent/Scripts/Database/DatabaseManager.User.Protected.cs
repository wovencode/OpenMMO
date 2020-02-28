
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;
using UnityEngine.AI;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// ============================ PROTECTED METHODS ================================
		
		// -------------------------------------------------------------------------------
		// UserValid
		// -------------------------------------------------------------------------------
		protected bool UserValid(string username, string password)
		{
			return FindWithQuery<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE username=? AND password=? AND banned=0 AND deleted=0", username, password) != null;
		}
		
		// -------------------------------------------------------------------------------
		// UserExists
		// -------------------------------------------------------------------------------
		protected bool UserExists(string username)
		{
			return FindWithQuery<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE username=?", username) != null;
		}
		
		// -------------------------------------------------------------------------------
		// UserRegister
		// -------------------------------------------------------------------------------
		protected void UserRegister(string userName, string userPassword, string userEmail, string userDeviceid)
		{
			// -- lastlogin is UtcNow minus 'logoutInterval' to allow immediate login
			DateTime dateTime = DateTime.UtcNow.AddSeconds(-logoutInterval);
			Insert(new TableUser{ username=userName, password=userPassword, email=userEmail, deviceid=userDeviceid, created=DateTime.UtcNow, lastonline=dateTime });
		}
		
		// -------------------------------------------------------------------------------
		// UserChangePassword
		// updates the oldpassword of a user to the new password, requires oldpassword to match
		// -------------------------------------------------------------------------------
		protected void UserChangePassword(string username, string oldpassword, string newpassword)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET password=? WHERE username=? AND password=?", newpassword, username, oldpassword);
		}

		// -------------------------------------------------------------------------------
		// UserSetDeleted
		// Sets the user to deleted (1) or undeletes it (0)
		// -------------------------------------------------------------------------------
		protected void UserSetDeleted(string username, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET deleted=? WHERE username=?", action == DatabaseAction.Do ? 1 : 0, username);
		}
		
		// -------------------------------------------------------------------------------
		// UserSetBanned
		// Bans (1) or unbans (0) the user
		// -------------------------------------------------------------------------------
		protected void UserSetBanned(string username, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET banned=? WHERE username=?", action == DatabaseAction.Do ? 1 : 0, username);
		}

        // -------------------------------------------------------------------------------
        // DeleteDataUser
        // Called when User Data has been deleted.
        // -------------------------------------------------------------------------------
        protected void DeleteDataUser(string username)
        {
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataUser), username); //HOOK
        }

		// -------------------------------------------------------------------------------
		// DeleteUser
		// Permanently deletes the user and all of its data (hard delete)
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(DeleteUser))]
		protected void DeleteUser(string username)
		{
		
			// -- delete player data too
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataPlayer), username); //HOOK

            DeleteDataUser(username);
		}
		
		// -------------------------------------------------------------------------------
		// UserSetConfirmed
		// Sets the user to confirmed (1) or unconfirms it (0)
		// -------------------------------------------------------------------------------
		protected void UserSetConfirmed(string username, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET confirmed=? WHERE username=?", action == DatabaseAction.Do ? 1 : 0, username);
		}
		
		// -------------------------------------------------------------------------------
		// GetUserCount
		// returns the number of user accounts registered on this device-id and/or email
		// -------------------------------------------------------------------------------
		protected int GetUserCount(string deviceId, string eMail)
		{

			List<TableUser> result = Query<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE deviceid=? AND email=? AND deleted=0", deviceId, eMail);
			
			if (result == null)
				return 0;
			else
				return result.Count;

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================