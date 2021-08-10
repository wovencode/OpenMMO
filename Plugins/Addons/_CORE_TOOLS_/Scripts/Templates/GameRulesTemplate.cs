//by Fhiz
using System;
using UnityEngine;
using System.Linq;
using OpenMMO;

namespace OpenMMO
{
	
	/// <summary>
	/// Contains global configuration for the game itself (like basic formulas, global cooldowns etc.)
	/// </summary>
	[CreateAssetMenu(fileName = "New GameRules", menuName = "OpenMMO - Configuration/New GameRules", order = 999)]
	public partial class GameRulesTemplate : ScriptableObject
	{

		// .. empty, is added via partial ...
		
		static GameRulesTemplate _instance;

		/// <summary>
		/// Creates a singleton on this class to be accesible from code anywhere. Singleton is OK in this situation because this template (= Scriptable Object) exists only once.
		/// </summary>
		public static GameRulesTemplate singleton
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<GameRulesTemplate>().FirstOrDefault();
				return _instance;
			}
		}

	}

}