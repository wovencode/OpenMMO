//BY DX4D
#define SHOW_BUILD_REPORT //Comment this out to turn off build reporting - May speed up build times slightly, but you will get no console feedback about your build.
#if UNITY_EDITOR

using OpenMMO;
using UnityEngine;
using UnityEditor;
using OpenMMO.Network;
using OpenMMO.Portals;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;
using System.Text;

namespace OpenMMO
{
    public enum FileExtension { exe, x86_64, app }

    // ===================================================================================
    // QuickBuildMenu
    // ===================================================================================
    public class QuickBuildMenu
	{
        //FULL DEPLOY CONFIG
        /// <summary>When running a Full Deploy, this is the type of server that will be built.</summary>
        public static BuildTarget fullDeployServerType = BuildTarget.StandaloneLinux64;
        /// <summary>When running a Full Deploy, these are the types of clients that will be built.</summary>
        public static BuildTarget[] fullDeployClientTypes = new BuildTarget[] {
            BuildTarget.StandaloneWindows64,
            BuildTarget.StandaloneOSX };

        #region BUILD REPORT - build log
#if SHOW_BUILD_REPORT
        static StringBuilder buildLog = new StringBuilder();
#endif
        #endregion
        // -------------------------------------------------------------------------------
        // GetScenesFromBuild
        // -------------------------------------------------------------------------------
        public static string[] GetScenesFromBuild()
        {
            List<string> scenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                    scenes.Add(scene.path);
            }
            return scenes.ToArray();
        }

        //BUILD
        /// <summary>Builds an application using the passed in parameters.</summary>
        /// <param name="targetPlatform">BuildTarget.StandaloneWindows64, StandaloneLinux64, StandaloneOSX</param>
        /// <param name="buildType">NetworkType.Server - Client - HostAndPlay</param>
        /// <param name="headless">Build in headless mode? (console application)</param>
        public static void Build(BuildTarget targetPlatform, NetworkType buildType, bool headless = false)
        {
            ChangeBuildMenu.SetBuildType(buildType, headless); //ACTIVATE CHANGE BUILD MENU

            FileExtension buildFileExtension = FileExtension.exe;
            switch (targetPlatform)
            {
                    //STANDALONE
                case BuildTarget.StandaloneWindows64: buildFileExtension = FileExtension.exe; break;
                case BuildTarget.StandaloneWindows: buildFileExtension = FileExtension.exe; break;
                case BuildTarget.StandaloneLinux64: buildFileExtension = FileExtension.x86_64; break;
                case BuildTarget.StandaloneOSX: buildFileExtension = FileExtension.app; break;
                /*TODO
                    //MOBILE
                case BuildTarget.iOS: break;
                case BuildTarget.Android: break;
                    //CONSOLE
                case BuildTarget.PS4: break;
                case BuildTarget.XboxOne: break;
                case BuildTarget.Switch: break;
                    //WEB
                case BuildTarget.WebGL: break;
                    //OTHER
                case BuildTarget.WSAPlayer: break;
                case BuildTarget.tvOS: break;
                case BuildTarget.Lumin: break;
                case BuildTarget.Stadia: break;
                case BuildTarget.NoTarget: break;
                */
            }

            //SETUP BUILD OPTIONS
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = GetScenesFromBuild();
            buildPlayerOptions.locationPathName = targetPlatform + "/" + "" + buildType + "." + buildFileExtension;
            buildPlayerOptions.target = targetPlatform;
            buildPlayerOptions.options = (headless) ? (BuildOptions.EnableHeadlessMode) : (BuildOptions.None);

            #region  BUILD REPORT - header
#if SHOW_BUILD_REPORT
            buildLog.AppendLine("<b>[BUILD] " + targetPlatform + " - " + ((headless) ? "(headless) " : "") + buildType + "</b>"
                + "\n" + buildPlayerOptions.locationPathName);

            foreach (string scenePath in buildPlayerOptions.scenes)
            {
                buildLog.AppendLine(scenePath);
            }
#endif
            #endregion

            //BUILD REPORTING
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            //SUCCESS
            if (summary.result == BuildResult.Succeeded)
            {
                #region  BUILD REPORT - success
#if SHOW_BUILD_REPORT
                buildLog.AppendLine("<color=green><b>" + targetPlatform + " " + buildType + " build succeeded..." + "</b></color>"
                    + "\nBuild size: " + summary.totalSize + " bytes"
                    + "\nBuild duration: " + System.Math.Round((summary.buildEndedAt - summary.buildStartedAt).TotalSeconds) + "s" );
#endif
                #endregion
            }
            //FAILURE
            if (summary.result == BuildResult.Failed)
            {
                #region  BUILD REPORT - failure
#if SHOW_BUILD_REPORT
                buildLog.AppendLine("<color=red><b>" + targetPlatform + " " + buildType + " build failed...</b></color>"
                    + "\n" + report.ToString());
#endif
                #endregion
            }
        }

        // -------------------------------------------------------------------------------
        // Build - FullDeployment - ClientAndServer - Server - Client
        // -------------------------------------------------------------------------------
        //FULL DEPLOYMENT
        /// <summary>Builds a Full Deploy cycle for the platforms declared in this script.</summary>
		public static void BuildFullDeployment()
		{
            BuildHeadlessServer(fullDeployServerType);

            foreach (BuildTarget targetPlatform in fullDeployClientTypes)
            {
                BuildClient(targetPlatform);
            }
        }
        //SERVER + CLIENT
        /// <summary>Builds a client and a headless server back to back.</summary>
		public static void BuildClientAndServer(BuildTarget targetPlatform)
		{
            BuildHeadlessServer(targetPlatform);
            BuildClient(targetPlatform);
        }
        //HEADLESS SERVER
        /// <summary>Builds a headless server for the target platform.</summary>
		public static void BuildHeadlessServer(BuildTarget targetPlatform)
		{
            Build(targetPlatform, NetworkType.Server, true);
        }
        //SERVER
        /// <summary>Builds a server for the target platform. It is recommended to use a Headless Server instead.</summary>
        public static void BuildServer(BuildTarget targetPlatform)
        {
            Build(targetPlatform, NetworkType.Server);
        }
        //CLIENT
        /// <summary>Builds a client for the target platform.</summary>
        public static void BuildClient(BuildTarget targetPlatform)
		{
            Build(targetPlatform, NetworkType.Client);
        }

        // -------------------------------------------------------------------------------
        // BuildClientAndServer - Windows - Mac - Linux
        // -------------------------------------------------------------------------------
        //FULL DEPLOYMENT
        [MenuItem("OpenMMO/Quick Build/FULL BUILD/Full Deployment", priority = 10)]
        public static void BuildFull()
        {
            #region  BUILD REPORT - title
#if SHOW_BUILD_REPORT
            buildLog.AppendLine("<color=orange><b>[BUILD REPORT]</b></color> " + "\n<b>Full Deployment Package</b>"); //BUILD REPORT
#endif
            #endregion

            BuildFullDeployment(); //BUILD

            #region  BUILD REPORT
#if SHOW_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //WINDOWS
        [MenuItem("OpenMMO/Quick Build/WIN64/Client and Server", priority = 11)]
        public static void BuildWindows64ClientAndServer()
        {
            #region  BUILD REPORT - title
#if SHOW_BUILD_REPORT
            buildLog.AppendLine("<color=orange><b>[BUILD REPORT]</b></color> " + "\n<b>Windows 64 Headless Server and Client</b>"); //BUILD REPORT
#endif
            #endregion

            BuildClientAndServer(BuildTarget.StandaloneWindows64); //BUILD

            #region  BUILD REPORT
#if SHOW_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //MAC
        [MenuItem("OpenMMO/Quick Build/OSX/Client and Server", priority = 12)]
        public static void BuildMacClientAndServer()
        {
            #region  BUILD REPORT - title
#if SHOW_BUILD_REPORT
            buildLog.AppendLine("<color=orange><b>[BUILD REPORT]</b></color> " + "\n<b>OSX Headless Server and Client</b>"); //BUILD REPORT
#endif
            #endregion

            BuildClientAndServer(BuildTarget.StandaloneOSX); //BUILD

            #region  BUILD REPORT
#if SHOW_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //LINUX
        [MenuItem("OpenMMO/Quick Build/Linux/Client and Server", priority = 13)]
        public static void BuildLinuxClientAndServer()
        {
            #region  BUILD REPORT - title
#if SHOW_BUILD_REPORT
            buildLog.AppendLine("<color=orange><b>[BUILD REPORT]</b></color> " + "\n<b>Linux Headless Server and Client</b>"); //BUILD REPORT
#endif
            #endregion

            BuildClientAndServer(BuildTarget.StandaloneLinux64); //BUILD

            #region  BUILD REPORT
#if SHOW_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }

        //TODO: Android and iOS
        //TODO: PS4, XBoxOne, Switch
		
		// -------------------------------------------------------------------------------
		
	}

}
#endif