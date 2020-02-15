using OpenMMO;
using UnityEngine;
using UnityEditor;
using OpenMMO.Network;

public partial class Project
{
    public static class Config
    {
        [MenuItem("OpenMMO/Server Mode")]
        public static void SetServer()
        {
            ProjectConfigTemplate config = Resources.Load<ProjectConfigTemplate>("Configuration/ProjectConfiguration [Example]");
            config.networkType = NetworkType.Server;
        }

        [MenuItem("OpenMMO/Client Mode")]
        public static void SetClient()
        {
            ProjectConfigTemplate config = Resources.Load<ProjectConfigTemplate>("Configuration/ProjectConfiguration [Example]");
            config.networkType = NetworkType.Client;
        }

        [MenuItem("OpenMMO/Host and Play Mode")]
        public static void SetHostAndPlay()
        {
            ProjectConfigTemplate config = Resources.Load<ProjectConfigTemplate>("Configuration/ProjectConfiguration [Example]");
            config.networkType = NetworkType.HostAndPlay;
        }
    }
}
