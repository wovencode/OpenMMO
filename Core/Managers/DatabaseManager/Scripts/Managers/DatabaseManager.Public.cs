// by Fhiz
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Network;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Mirror;

namespace OpenMMO.Database
{
	
	/// <summary>
	/// This partial section of the DatabaseManager contains only public methods that are independent of the database layer in use.
	/// </summary>
	public partial class DatabaseManager
	{
    	
		/// <summary>
		/// This hook is called when a player (= character) is registered and is executed on all modules to parse default data onto the player (like starting Equipment etc.).
		/// </summary>
		public void CreateDefaultDataPlayer(GameObject player)
		{
			this.InvokeInstanceDevExtMethods(nameof(CreateDefaultDataPlayer), player); //HOOK
		}

        /// <summary>
		/// This hook is called at the start of LoadDataPlayer, before the rest of the method is called (some data needs to get loaded first).
		/// </summary>
        public virtual void LoadDataPlayerPriority(GameObject prefab, GameObject player)
        {
			this.InvokeInstanceDevExtMethods(nameof(LoadDataPlayerPriority), player); //HOOK
        }
		
		/// <summary>
		/// This hook is called when a player (= character) is loaded from the database, the hook is executed on all modules and used to load additional data provided by those.
		/// </summary>
		public GameObject LoadDataPlayer(GameObject prefab, string _name)
		{
			GameObject player = Instantiate(prefab);
			player.name = _name;
			
            LoadDataPlayerPriority(prefab, player);

			this.InvokeInstanceDevExtMethods(nameof(LoadDataPlayer), player); //HOOK
			return player;
		}
		
		/// <summary>
		/// Called when a user is saved to the database, this hook is executed on all modules and used to save additional data provided by those.
		/// <para>isNew = Is this a new player character? (Saving might be different in that case)</para>
		/// <para>useTransaction = When saved individually we can use a transaction</para>
		/// </summary>
		public void SaveDataUser(string username, bool isNew = false, bool useTransaction = true)
		{
			if (useTransaction)
				databaseLayer.BeginTransaction();
			
			this.InvokeInstanceDevExtMethods(nameof(SaveDataUser), username, isNew); //HOOK
			
			if (useTransaction)
				databaseLayer.Commit();
		}
		
		/// <summary>
		/// This hook is called when a player is saved to the database and executed on all modules to save additional data.
		/// <para>isNew = Is this a new player character? (Saving might be different in that case)</para>
		/// <para>useTransaction = When saved individually we can use a transaction</para>
		/// </summary>
		public void SaveDataPlayer(GameObject player, bool isNew = false, bool useTransaction = true)
		{
			if (useTransaction)
				databaseLayer.BeginTransaction();
			
			this.InvokeInstanceDevExtMethods(nameof(SaveDataPlayer), player, isNew); //HOOK
			
			if (useTransaction)
				databaseLayer.Commit();
		}
		
		/// <summary>
		/// Logs a existing but logged-out user (= account) into the database.
		/// <remarks>This method is called from the NetworkManager</remarks>
		/// </summary>
		public void LoginUser(string name)
		{
			this.InvokeInstanceDevExtMethods(nameof(LoginUser), name); //HOOK
		}
		
		/// <summary>
		/// Logs out a logged-in user (= account) from the database.
		/// <remarks>This method is called from the NetworkManager</remarks>
		/// </summary>
		public void LogoutUser(string name)
		{
			SaveDataUser(name, false);
			this.InvokeInstanceDevExtMethods(nameof(LogoutUser), name); //HOOK
		}
		
		/// <summary>
		/// Logs a existing but logged-out player (= character) into the database.
		/// <remarks>This method is called from the NetworkManager</remarks>
		/// </summary>
		public void LoginPlayer(NetworkConnection conn, GameObject player, string playerName, string userName)
		{
			this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), conn, player, playerName, userName); //HOOK
		}
		
		/// <summary>
		/// Logs out an logged-in player (= character) from the database.
		/// <remarks>This method is called from the NetworkManager</remarks>
		/// </summary>
		public void LogoutPlayer(GameObject player)
		{
			SaveDataPlayer(player, false);
			this.InvokeInstanceDevExtMethods(nameof(LogoutPlayer), player); //HOOK
        }
		
	}

}