
using UnityEngine;
using Wovencode;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Wovencode
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