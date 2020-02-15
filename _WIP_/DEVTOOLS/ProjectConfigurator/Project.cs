//BY DX4D
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
            if (config)
            {
                config.networkType = NetworkType.Server;
                Debug.Log("<b><color=yellow>[SERVER] Build Mode Activated</color></b>");
            }

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
            if (config)
            {
                config.networkType = NetworkType.Client;
                Debug.Log("<b><color=blue>[CLIENT] Build Mode Activated</color></b>");
            }

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
            if (config)
            {
                config.networkType = NetworkType.HostAndPlay;
                Debug.Log("<b><color=green>[HOST+PLAY] Build Mode Activated</color></b>");
            }

            PortalManager portal = GameObject.FindObjectOfType<PortalManager>();
            if (portal)
            {
                portal.active = false;
            }
        }
    }
}
#endif