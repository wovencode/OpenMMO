#if UNITY_EDITOR
using OpenMMO;
using UnityEngine;
using UnityEditor;
using OpenMMO.Network;
using OpenMMO.Portals;

public partial class Project
{
    public static class Config
    {
        [MenuItem("OpenMMO/Server Mode")]
        public static void SetServer()
        {
            ProjectConfigTemplate config = Resources.Load<ProjectConfigTemplate>("Configuration/ProjectConfiguration [Example]");
            config.networkType = NetworkType.Server;

            PortalManager portal = GameObject.FindObjectOfType<PortalManager>();
            if (portal)
            {
                portal.active = true;
            }
        }

        [MenuItem("OpenMMO/Client Mode")]
        public static void SetClient()
        {
            ProjectConfigTemplate config = Resources.Load<ProjectConfigTemplate>("Configuration/ProjectConfiguration [Example]");
            config.networkType = NetworkType.Client;

            PortalManager portal = GameObject.FindObjectOfType<PortalManager>();
            if (portal)
            {
                portal.active = false;
            }
        }

        [MenuItem("OpenMMO/Host and Play Mode")]
        public static void SetHostAndPlay()
        {
            ProjectConfigTemplate config = Resources.Load<ProjectConfigTemplate>("Configuration/ProjectConfiguration [Example]");
            config.networkType = NetworkType.HostAndPlay;

            PortalManager portal = GameObject.FindObjectOfType<PortalManager>();
            if (portal)
            {
                portal.active = false;
            }
        }
    }
}
#endif