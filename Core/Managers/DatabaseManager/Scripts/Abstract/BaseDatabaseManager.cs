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
		/// The base class includes a debug helper, it is only initialized when set to active
		/// </summary>
		public virtual void Awake()
		{
			
		}
		
    	/// <summary>
		/// Basic input validation when trying to login a user (= account)
		/// </summary>
		public virtual bool TryUserLogin(string userName, string userPassword)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		/// <summary>
		/// Basic input validation when trying to login a user
		/// </summary>
		public virtual bool TryUserRegister(string userName, string userPassword, string email, string deviceid)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserDelete(string userName, string userPassword, int action=1)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserBan(string userName, string userPassword, int action=1)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserChangePassword(string userName, string oldPassword, string newPassword)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(oldPassword) && Tools.IsAllowedPassword(newPassword) && oldPassword != newPassword);
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserConfirm(string userName, string userPassword, int action=1)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserGetValid(string userName, string userPassword)
		{
			return (Tools.IsAllowedName(userName) && Tools.IsAllowedPassword(userPassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserGetExists(string userName)
		{
			return (Tools.IsAllowedName(userName));
		}
		
		// -------------------------------------------------------------------------------
		public abstract int TryUserGetPlayerCount(string userName);
				
		// ======================= PUBLIC METHODS - PLAYER ===============================
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerLogin(string playername, string userName)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(userName));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerRegister(string playername, string userName, string prefabname)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(userName) && !String.IsNullOrWhiteSpace(prefabname));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerDeleteSoft(string playername, string userName, int action=1)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(userName));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerDeleteHard(string playername, string userName)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(userName));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerBan(string playername, string userName, int action=1)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(userName));
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================