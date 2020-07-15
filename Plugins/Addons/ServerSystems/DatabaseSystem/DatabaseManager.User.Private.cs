//by  Fhiz
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;
using Mirror;

namespace OpenMMO.Database
{

	/// <summary>
	/// This partial section of DatabaseManager is responsible to handle user (= account) related actions.
	/// </summary>
	public partial class DatabaseManager
	{

		/// <summary>
		/// Hooks into init and creates TableUser if it does not exist yet on the database.
		/// </summary>
		[DevExtMethods(nameof(Init))]
		void Init_User()
		{
	   		CreateTable<TableUser>();
		}
		
		/// <summary>
		/// Hooks into LoadDataPlayerPriority. Not used yet.
		/// </summary>
		[DevExtMethods(nameof(LoadDataPlayerPriority))]
		void LoadDataPlayerPriority_User(GameObject player)
		{
		
		}
				
	   	/// <summary>
		/// Hooks into LoadDataPlayer. Not used yet.
		/// </summary>
		[DevExtMethods(nameof(LoadDataPlayer))]
		void LoadDataPlayer_User(GameObject player)
		{
	   	
		}

		/// <summary>
		/// Hooks into SaveDataPlayer and is called when a player (= character) is saved to the database.
		/// </summary>
		/// <remarks>
		/// It is correct that this code modifies user, when the player logs in. As we have to update the user (= account) lastonline time as well.
		/// </remarks>
		[DevExtMethods(nameof(SaveDataPlayer))]
		void SaveDataPlayer_User(GameObject player, bool isNew)
		{
			// dont update the time on a new player or we log ourselves out of login
			if (isNew) return; 
			
			string userName = player.GetComponent<PlayerAccount>()._tablePlayer.username;
			Execute("UPDATE "+nameof(TableUser)+" SET lastonline=?, lastsaved=? WHERE username=?", DateTime.UtcNow, DateTime.UtcNow, userName);

		}
		
		/// <summary>
		/// Hooks into LoginPlayer and is called when a player (= character) logs in.
		/// </summary>
		/// <remarks>
		/// It is correct that this code modifies user, when the player logs in. As we have to update the user (= account) lastonline time as well.
		/// </remarks>
	   	[DevExtMethods(nameof(LoginPlayer))]
	   	void LoginPlayer_User(NetworkConnection conn, GameObject player, string playerName, string userName)
	   	{
	   		// -- we update lastlogin of user only when a player character logs in (otherwise we lock ourselves out)
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastonline=? WHERE username=?", DateTime.UtcNow, userName);
	   	}
	   	
	   	/// <summary>
		/// Hooks into LogoutPlayer and is called when a player (= character) logs out.
		/// </summary>
		/// <remarks>
		/// It is correct that this code modifies user, when the player logs out. As we have to update the user (= account) lastonline time as well.
		/// </remarks>
	   	[DevExtMethods(nameof(LogoutPlayer))]
	   	void LogoutPlayer_User(GameObject player)
	   	{
	   		// -- lastlogin is UtcNow minus 'logoutInterval' to allow immediate login
	   		string userName = player.GetComponent<PlayerAccount>()._tablePlayer.username;
			DateTime dateTime = DateTime.UtcNow.AddSeconds(-logoutInterval);
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastonline=? WHERE username=?", dateTime, userName);
	   	}
	   	
	   	/// <summary>
		/// Hooks into SaveDataUser and is called when a user (= account) is saved to the database.
		/// </summary>
		[DevExtMethods(nameof(SaveDataUser))]
		void SaveDataUser_User(string username, bool isNew)
		{
		
			// dont update the time on a new player or we log ourselves out of login
			if (isNew) return; 
			
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastonline=?, lastsaved=? WHERE username=?", DateTime.UtcNow, DateTime.UtcNow, username);
		}
		
		/// <summary>
		/// Hooks into LoginUser. Not used yet.
		/// </summary>
	   	[DevExtMethods(nameof(LoginUser))]
	   	void LoginUser_User(string username)
	   	{
	   		// -- Note: We do NOT set the lastonline time here as it would lock us out!
	   	}
	   	
		/// <summary>
		/// Hooks into LogoutUser and is called when either the user (= account) or player (= character) logs out.
		/// </summary>
	   	[DevExtMethods(nameof(LogoutUser))]
	   	void LogoutUser_User(string username)
	   	{
	   		// -- this resets lastlogin to allow immediate re-login
	   		// -- lastlogin is UtcNow minus 'logoutInterval' to allow immediate login
			DateTime dateTime = DateTime.UtcNow.AddSeconds(-logoutInterval);
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastonline=? WHERE username=?", dateTime, username);
	   	}
	   	
		/// <summary>
		/// Hooks into DeleteDataUser and is called when a user (= account) is deleted.
		/// </summary>
	   	[DevExtMethods(nameof(DeleteDataUser))]
	   	void DeleteDataUser_User(string username)
	   	{
	   		Execute("DELETE FROM "+nameof(TableUser)+" WHERE username=?", username);
	   	}
	   	
	}

}