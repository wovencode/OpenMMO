//BY DX4D
#if UNITY_EDITOR
using OpenMMO;
using UnityEngine;
using UnityEditor;
using OpenMMO.Network;
using OpenMMO.Portals;

public class ChangeBuild
{
    //SERVER
    [MenuItem("OpenMMO/Change Build/Server Mode")]
    public static void SetServer()
    {
        Debug.Log("Please wait...");
        PortalManager portal = GameObject.FindObjectOfType<PortalManager>();
        if (portal)
        {
            portal.active = true;
        }

        ProjectConfigTemplate config = Resources.Load<ProjectConfigTemplate>("Configuration/ProjectConfiguration [Example]");
        if (config)
        {
            config.networkType = NetworkType.Server;
            Debug.Log("<b><color=yellow>[SERVER] Build Mode Activating</color>...please wait...</b>");
        }

        Project.ForceEditorRecompile();
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate); //NOTE: This is broken in Unity...we wrote our own function to replace it.
    }

    //CLIENT
    [MenuItem("OpenMMO/Change Build/Client Mode")]
    public static void SetClient()
    {
        Debug.Log("Please wait...");
        PortalManager portal = GameObject.FindObjectOfType<PortalManager>();
        if (portal)
        {
            portal.active = true;
        }

        ProjectConfigTemplate config = Resources.Load<ProjectConfigTemplate>("Configuration/ProjectConfiguration [Example]");
        if (config)
        {
            config.networkType = NetworkType.Client;
            Debug.Log("<b><color=blue>[CLIENT] Build Mode Activating</color>...please wait...</b>");
        }

        Project.ForceEditorRecompile();
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate); //NOTE: This is broken in Unity...we wrote our own function to replace it.
    }

    //HOST AND PLAY
    [MenuItem("OpenMMO/Change Build/Host and Play Mode")]
    public static void SetHostAndPlay()
    {
        Debug.Log("Please wait...");
        PortalManager portal = GameObject.FindObjectOfType<PortalManager>();
        if (portal)
        {
            portal.active = false;
        }

        ProjectConfigTemplate config = Resources.Load<ProjectConfigTemplate>("Configuration/ProjectConfiguration [Example]");
        if (config)
        {
            config.networkType = NetworkType.HostAndPlay;
            Debug.Log("<b><color=green>[HOST+PLAY] Build Mode Activating</color>...please wait...</b>");
        }

        Project.ForceEditorRecompile();
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate); //NOTE: This is broken in Unity...we wrote our own function to replace it.
    }
}
#endif