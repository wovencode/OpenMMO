
//using OpenMMO;
//using OpenMMO.Database;
using OpenMMO.Database.Table;
using OpenMMO.Network;
//using UnityEngine;
//using System;
using System.Collections.Generic;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{

		// -------------------------------------------------------------------------------
		// GetPlayers
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public function <c>GetPlayers</c> returns a list of <c>PlayerPreviews</c> based on username.
        /// Used to retrieve a user's player characters.
        /// </summary>
        /// <param name="username"></param>
        /// <returns> A list of unbanned <c>PlayerPreviews</c> created by a user.  </returns>
		public List<PlayerPreview> GetPlayers(string username)
		{

			List<PlayerCharacter> results = Query<PlayerCharacter>("SELECT * FROM "+nameof(PlayerCharacter)+" WHERE username=? AND deleted=0 AND banned=0", username);
			
			List<PlayerPreview> players = new List<PlayerPreview>();
			
			if (results == null)
				return players;
			
			foreach (PlayerCharacter result in results)
			{
				players.Add(new PlayerPreview { name = result.playername} );
			}
			
			return players;
		}

		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================