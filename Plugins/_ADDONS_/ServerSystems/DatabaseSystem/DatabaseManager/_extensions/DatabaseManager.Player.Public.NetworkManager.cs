
using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Network;
using UnityEngine;
using System;
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
        /// <returns> A list of unbanned and undeleted <c>PlayerPreviews</c> created by a user.  </returns>
		public List<PlayerPreview> GetPlayers(string username)
		{

            List<TablePlayer> characters = LoadCharactersFromDatabase(username);
            
            List<PlayerPreview> players = new List<PlayerPreview>();
			if (characters == null) return players; //NO CHARACTERS?
			
			foreach (TablePlayer character in characters)
			{
                players.Add(CreatePlayerPreview(character.playername, character.prefab));
			}
			
			return players;
		}
        //LOAD PLAYERS FROM DATABASE
        List<TablePlayer> LoadCharactersFromDatabase(string username)
        {
            return Query<TablePlayer>("SELECT * FROM " + nameof(TablePlayer) + " WHERE username=? AND deleted=0 AND banned=0", username);
        }
        PlayerPreview CreatePlayerPreview(string _playername, string _prefabname)
        {
                return new PlayerPreview { playername = _playername, prefabname = _prefabname };
        }

		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================