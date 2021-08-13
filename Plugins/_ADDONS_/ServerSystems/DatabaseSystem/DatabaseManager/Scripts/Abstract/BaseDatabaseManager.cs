// by Fhiz
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Debugging;
using UnityEngine;
using System;

namespace OpenMMO.Database
{

	/// <summary>
	/// Abstract base class for DatabaseManagers
	/// </summary>
	public abstract partial class BaseDatabaseManager : MonoBehaviour, IAccountableManager
	{
		
		[Header("Debug Helper")]
		public DebugHelper debug = new DebugHelper();
		
		/// <summary>
		/// Awake is virtual so it can be overriden.
		/// </summary>
		public virtual void Awake() {}
		
    	/// <summary>
		/// Basic input validation when trying to login a user (= account).
		/// </summary>
		public virtual bool TryUserLogin(string userName, string userPassword)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		/// <summary>
		/// Basic input validation when trying to register a new user (= account).
		/// <para>eMail is not checked as it is optional, but has to be provided.</para>
		/// <para>deviceId is not checked as it is optional, but has to be provided as well.</para>
		/// </summary>
		public virtual bool TryUserRegister(string userName, string userPassword, string eMail, string deviceId)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		/// <summary>
		/// Basic input validation when trying to soft delete an existing user (= account).
		/// <para>databaseAction can only be (delete) or (un-delete)
		/// </summary>
		public virtual bool TryUserDelete(string userName, string userPassword, DatabaseAction databaseAction = DatabaseAction.Do)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		/// <summary>
		/// Basic input validation when trying to ban an existing user (= account).
		/// <para>databaseAction can only be (ban) or (un-ban)
		/// </summary>
		public virtual bool TryUserBan(string userName, string userPassword, DatabaseAction databaseAction = DatabaseAction.Do)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		/// <summary>
		/// Basic input validation when trying to change a user (= account) password.
		/// <para>new password may not match the old password in order to be valid.</para>
		/// </summary>
		public virtual bool TryUserChangePassword(string userName, string oldPassword, string newPassword)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(oldPassword) && Tools.IsAllowedPassword(newPassword) && oldPassword != newPassword);
		}
		
		/// <summary>
		/// Basic input validation when trying to confirm a user (= account).
		/// <para>databaseAction can only be (confirm) or (un-confirm)
		/// </summary>
		public virtual bool TryUserConfirm(string userName, string userPassword, DatabaseAction databaseAction = DatabaseAction.Do)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		/// <summary>
		/// Basic input validation when trying to check if a user (= account) is valid.
		/// </summary>
		public virtual bool TryUserGetValid(string userName, string userPassword)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		/// <summary>
		/// Basic input validation when trying to check if a user (= account) exists. 
		/// </summary>
		public virtual bool TryUserGetExists(string userName)
		{
			return (Tools.IsAllowedName(userName));
		}
		
		/// <summary>
		/// Basic input validation when trying to login a player (= character).
		/// </summary>
		public virtual bool TryPlayerLogin(string playerName, string userName)
		{
			return (Tools.IsAllowedName(playerName) && Tools.IsAllowedName(userName));
		}
		
		/// <summary>
		/// Basic input validation when trying to register a new player (= character).
		/// </summary>
		public virtual bool TryPlayerRegister(string playerName, string userName, string prefabName)
		{
			return (Tools.IsAllowedName(playerName) && Tools.IsAllowedName(userName) && !String.IsNullOrWhiteSpace(prefabName));
		}
		
		/// <summary>
		/// Basic input validation when trying to soft-delete an existing player (= character).
		/// <para>databaseAction can only be (delete) or (un-delete)
		/// </summary>
		public virtual bool TryPlayerDeleteSoft(string playerName, string userName, DatabaseAction databaseAction = DatabaseAction.Do)
		{
			return (Tools.IsAllowedName(playerName) && Tools.IsAllowedName(userName));
		}
		
		/// <summary>
		/// Basic input validation when trying to hard-delete an existing player (= character).
		/// </summary>
		public virtual bool TryPlayerDeleteHard(string playerName, string userName)
		{
			return (Tools.IsAllowedName(playerName) && Tools.IsAllowedName(userName));
		}
		
		/// <summary>
		/// Basic input validation when trying to ban an existing player (= character).
		/// <para>databaseAction can only be (ban) or (un-ban)
		/// </summary>
		public virtual bool TryPlayerBan(string playerName, string userName, DatabaseAction databaseAction = DatabaseAction.Do)
		{
			return (Tools.IsAllowedName(playerName) && Tools.IsAllowedName(userName));
		}
		
		/// <summary>
		/// Abstract to ensure the database manager provides it. As it returns an int, we cannot return a bool here.
		/// </summary>
		public abstract int TryUserGetPlayerCount(string userName);
		
	}

}