
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.DebugManager;
using UnityEngine;
using System;

namespace OpenMMO.Database
{
	
	// ===================================================================================
	// BaseDatabase
	// ===================================================================================
	public abstract partial class BaseDatabaseManager : MonoBehaviour, IAccountableManager
	{
		
		[Header("Debug Helper")]
		public DebugHelper debug;
		
		// -------------------------------------------------------------------------------
		// Awake (Base)
		// -------------------------------------------------------------------------------
		public virtual void Awake()
		{
			debug = new DebugHelper();
			debug.Init();
		}
		
    	// ======================= PUBLIC METHODS - USER =================================
    	
		// -------------------------------------------------------------------------------
		public virtual bool TryUserLogin(string username, string userpassword)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(userpassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserRegister(string username, string userpassword, string email, string deviceid)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(userpassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserDelete(string username, string userpassword, int action=1)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(userpassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserBan(string username, string userpassword, int action=1)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(userpassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserChangePassword(string username, string oldPassword, string newPassword)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(oldPassword) && Tools.IsAllowedPassword(newPassword) && oldPassword != newPassword);
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserConfirm(string username, string userpassword, int action=1)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(userpassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserGetValid(string username, string userpassword)
		{
			return (Tools.IsAllowedName(username) && Tools.IsAllowedPassword(userpassword));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserGetExists(string username)
		{
			return (Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		public abstract int TryUserGetPlayerCount(string username);
				
		// ======================= PUBLIC METHODS - PLAYER ===============================
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerLogin(string playername, string username)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerRegister(string playername, string username, string prefabname)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username) && !String.IsNullOrWhiteSpace(prefabname));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerDeleteSoft(string playername, string username, int action=1)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerDeleteHard(string playername, string username)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerBan(string playername, string username, int action=1)
		{
			return (Tools.IsAllowedName(playername) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================