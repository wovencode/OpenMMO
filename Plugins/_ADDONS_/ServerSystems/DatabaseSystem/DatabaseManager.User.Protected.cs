//by  Fhiz
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
	// This partial section of DatabaseManager is responsible for handling user (= account) related actions.
	public partial class DatabaseManager
	{
		
		/// <summary>
		/// Checks if a user (= account) of the provided name and password is valid (exists, not banned and not deleted).
		/// </summary>
		protected bool UserValid(string username, string password)
		{
			return FindWithQuery<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE username=? AND password=? AND banned=0 AND deleted=0", username, password) != null;
		}
		
		/// <summary>
		/// Checks if a user of the provided name exists (ignores banned and/or deleted).
		/// </summary>
		protected bool UserExists(string username)
		{
			return FindWithQuery<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE username=?", username) != null;
		}
		
		/// <summary>
		/// Registers a new user (= account) using the provided name, password, email and deviceId (email and deviceId are optional).
		/// </summary>
		protected void UserRegister(string userName, string userPassword, string userEmail, string userDeviceid)
		{
			// -- lastlogin is UtcNow minus 'logoutInterval' to allow immediate login
			DateTime dateTime = DateTime.UtcNow.AddSeconds(-logoutInterval);
			Insert(new TableUser{ username=userName, password=userPassword, email=userEmail, deviceid=userDeviceid, created=DateTime.UtcNow, lastonline=dateTime });
		}
		
		/// <summary>
		/// Updates the password of a user to the new password. The provided oldPassword must match the one in the database in order to verify the process.
		/// </summary>
		protected void UserChangePassword(string username, string oldpassword, string newpassword)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET password=? WHERE username=? AND password=?", newpassword, username, oldpassword);
		}
		
		/// <summary>
		/// Sets the user to deleted (1) or undeletes it (0).
		/// </summary>
		protected void UserSetDeleted(string username, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET deleted=? WHERE username=?", action == DatabaseAction.Do ? 1 : 0, username);
		}
		
		/// <summary>
		/// Bans (1) or unbans (0) the user
		/// </summary>
		protected void UserSetBanned(string username, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET banned=? WHERE username=?", action == DatabaseAction.Do ? 1 : 0, username);
		}
		
		/// <summary>
		/// Called when user (= account) Data has been deleted.
		/// </summary>
        protected void DeleteDataUser(string username)
        {
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataUser), username); //HOOK
        }
		/// <summary>
		/// Permanently deletes the user (= account) and all of its data (hard delete)
		/// </summary>
		[DevExtMethods(nameof(DeleteUser))]
		protected void DeleteUser(string username)
		{
		
			// -- delete player data too
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataPlayer), username); //HOOK

            DeleteDataUser(username);
		}
		
		/// <summary>
		/// Sets the user (= account) to confirmed (1) or unconfirms it (0)
		/// </summary>
		protected void UserSetConfirmed(string username, DatabaseAction action = DatabaseAction.Do)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET confirmed=? WHERE username=?", action == DatabaseAction.Do ? 1 : 0, username);
		}
		
		/// <summary>
		/// Returns the number of user accounts registered on this device-id and/or email
		/// </summary>
		protected int GetUserCount(string deviceId, string eMail)
		{

			List<TableUser> result = Query<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE deviceid=? AND email=? AND deleted=0", deviceId, eMail);
			
			if (result == null)
				return 0;
			else
				return result.Count;

		}
		
	}

}