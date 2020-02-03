
using System;
using UnityEngine;
using System.Linq;
using OpenMMO;

namespace OpenMMO
{
	
	// ===================================================================================
	// GameRulesTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New GameRules", menuName = "OpenMMO - Configuration/New GameRules", order = 999)]
	public partial class GameRulesTemplate : ScriptableObject
	{

		//[Header("GameRules")]
		
		static GameRulesTemplate _instance;

		// -------------------------------------------------------------------------------
		// singleton
		// -------------------------------------------------------------------------------
		public static GameRulesTemplate singleton
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<GameRulesTemplate>().FirstOrDefault();
				return _instance;
			}
		}

		// -------------------------------------------------------------------------------
	}

}

// =======================================================================================
