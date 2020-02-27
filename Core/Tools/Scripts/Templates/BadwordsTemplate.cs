/* //DEPRECIATED
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// BadwordsTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Badwords", menuName = "OpenMMO - Templates/New Badwords", order = 999)]
	public partial class BadwordsTemplate : ScriptableObject
	{
    
    	[Tooltip("Usernames & Playernames containing these will be denied. Chat messages will be censored.")]
    	public string[] badwords;
		
		static BadwordsTemplate _instance;
		
		// -------------------------------------------------------------------------------
		// singleton
		// -------------------------------------------------------------------------------
		public static BadwordsTemplate singleton
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<BadwordsTemplate>().FirstOrDefault();
				return _instance;
			}
		}

		// -------------------------------------------------------------------------------

	}

}
*/
// =======================================================================================