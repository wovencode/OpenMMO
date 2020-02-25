#if UNITY_EDITOR

using OpenMMO;
using UnityEngine;
using UnityEditor;
using OpenMMO.Network;
using OpenMMO.Portals;

namespace OpenMMO
{

	// ===================================================================================
	// ChangeBuild
	// ===================================================================================
	public class ChangeBuild
	{
	
		// -------------------------------------------------------------------------------
		// SetServer
		// -------------------------------------------------------------------------------
		[MenuItem("OpenMMO/Change Build/Server Mode")]
		public static void SetServer()
		{
			ProjectConfigTemplate.singleton.networkType = NetworkType.Server;
			ProjectConfigTemplate.singleton.OnValidate();
		}
	
		// -------------------------------------------------------------------------------
		// SetClient
		// -------------------------------------------------------------------------------
		[MenuItem("OpenMMO/Change Build/Client Mode")]
		public static void SetClient()
		{
			ProjectConfigTemplate.singleton.networkType = NetworkType.Client;
			ProjectConfigTemplate.singleton.OnValidate();
		}
		
		// -------------------------------------------------------------------------------
		// SetHostAndPlay
		// -------------------------------------------------------------------------------
		[MenuItem("OpenMMO/Change Build/Host and Play Mode")]
		public static void SetHostAndPlay()
		{
			ProjectConfigTemplate.singleton.networkType = NetworkType.HostAndPlay;
			ProjectConfigTemplate.singleton.OnValidate();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}
#endif