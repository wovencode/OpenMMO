
using UnityEngine;
using OpenMMO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO
{
	
	// ===================================================================================
	// ConfigurationManager
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class ConfigurationManager : MonoBehaviour
	{
		
		[Header("Project Configuration")]
		public ProjectConfigTemplate projectConfigTemplate;
		
		[Header("Game Rules")]
		public GameRulesTemplate gameRulesTemplate;
		
		[Header("Server Authority")]
        public ServerAuthorityTemplate serverAuthorityTemplate;
		
		// -----------------------------------------------------------------------------------
	
	}

}