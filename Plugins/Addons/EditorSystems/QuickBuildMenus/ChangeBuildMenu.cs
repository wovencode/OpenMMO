//BY DX4D
#if UNITY_EDITOR

using OpenMMO;
using UnityEngine;
using UnityEditor;
using OpenMMO.Network;
using OpenMMO.Zones;

namespace OpenMMO
{

	// ===================================================================================
	// ChangeBuildMenu
	// ===================================================================================
	public class ChangeBuildMenu
	{

        // -------------------------------------------------------------------------------
        // SetBuildType
        // -------------------------------------------------------------------------------
        public static void SetBuildType(NetworkType buildType, bool headless = false)
        {
#if DEBUG
            Debug.Log("<b>[<color=purple>" + " " + buildType.ToString().ToUpper() + " MODE ACTIVATED" + " " + "</color>]</b>");
#endif
            ServerConfigTemplate.singleton.networkType = buildType;
            ServerConfigTemplate.singleton.OnValidate();

            EditorUserBuildSettings.enableHeadlessMode = headless;

            if (PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.Standalone) != ApiCompatibilityLevel.NET_4_6)
            {
                Debug.Log("<color=orange><b>OpenMMO requires .NET 4.x</b></color>\n<b>Changing to .NET 4.x</b>"); //DEBUG
                PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);
            }
        }
	
		// -------------------------------------------------------------------------------
		// SetServer
		// -------------------------------------------------------------------------------
		[MenuItem("OpenMMO/Change Build Mode/Server Mode", priority = 1)]
		public static void SetServer()
		{
            SetBuildType(NetworkType.Server, true);
			//ProjectConfigTemplate.singleton.networkType = NetworkType.Server;
			//ProjectConfigTemplate.singleton.OnValidate();
        }
	
		// -------------------------------------------------------------------------------
		// SetClient
		// -------------------------------------------------------------------------------
		[MenuItem("OpenMMO/Change Build Mode/Client Mode", priority = 2)]
		public static void SetClient()
		{
            SetBuildType(NetworkType.Client);
			//ProjectConfigTemplate.singleton.networkType = NetworkType.Client;
			//ProjectConfigTemplate.singleton.OnValidate();
		}
		
		// -------------------------------------------------------------------------------
		// SetHostAndPlay
		// -------------------------------------------------------------------------------
		[MenuItem("OpenMMO/Change Build Mode/Host and Play Mode", priority = 3)]
		public static void SetHostAndPlay()
		{
            SetBuildType(NetworkType.HostAndPlay);
			//ProjectConfigTemplate.singleton.networkType = NetworkType.HostAndPlay;
			//ProjectConfigTemplate.singleton.OnValidate();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}
#endif