
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
		
		[Header("Modules Configuration")]
		public ModulesConfigTemplate modulesConfigTemplate;
		
		[Header("Project Configuration")]
		public ProjectConfigTemplate projectConfigTemplate;
		
		[Header("Game Rules")]
		public GameRulesTemplate gameRulesTemplate;
		
		// -----------------------------------------------------------------------------------
	
	}

}