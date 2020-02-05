
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
		public virtual bool TryUserLogin(string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserRegister(string name, string password, string email, string deviceid)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserDelete(string name, string password, int action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserBan(string name, string password, int action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserChangePassword(string name, string oldPassword, string newPassword)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(oldPassword) && Tools.IsAllowedPassword(newPassword) && oldPassword != newPassword);
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserConfirm(string name, string password, int action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserGetValid(string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryUserGetExists(string name)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		public abstract int TryUserGetPlayerCount(string name);
				
		// ======================= PUBLIC METHODS - PLAYER ===============================
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerLogin(string name, string username)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerRegister(string name, string username, string prefabname)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username) && !String.IsNullOrWhiteSpace(prefabname));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerDeleteSoft(string name, string username, int action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerDeleteHard(string name, string username)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerBan(string name, string username, int action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedName(username));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool TryPlayerSwitchServer(string name)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================