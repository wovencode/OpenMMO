//by Fhiz
using UnityEngine;
using OpenMMO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO
{
	
	/// <summary>
	/// The purpose of this component is to provide Templates (= Scriptable Objects) that are a Singleton and exist only once in the whole project, with exactly one reference. Otherwise it is not possible to access them via "name.singleton.x"
	/// </summary>
	[DisallowMultipleComponent]
	public partial class ConfigurationManager : MonoBehaviour
	{
		
		[Header("Templates (Unique/Singleton)")]
		public ProjectConfigTemplate projectConfigTemplate;
		public GameRulesTemplate gameRulesTemplate;
        public ServerAuthorityTemplate serverAuthorityTemplate;
		
	}

}