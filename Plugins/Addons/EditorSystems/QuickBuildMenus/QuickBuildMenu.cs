//BY DX4D
//#define TEST_MODE //Uncomment this to access additional menus for testing purposes.
//#define SKIP_BUILD_REPORT //Uncomment this to turn off build reporting - May speed up build times slightly, but you will get no console feedback about your build.
#if UNITY_EDITOR

using OpenMMO;
using UnityEngine;
using UnityEditor;
using OpenMMO.Network;
using OpenMMO.Zones;
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
#if !SKIP_BUILD_REPORT
        static StringBuilder buildLog = new StringBuilder(" <color=orange><b>[BUILD REPORT]</b></color> ");
#endif
        #endregion

            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
        //INITIALIZE BUILD REPORT
        static void InitializeBuildReport(string buildName)
        {
            buildLog = new StringBuilder();
            buildLog.AppendLine(" <color=purple><b>[BUILD REPORT]</b></color> ");
            buildLog.AppendLine("<b>" + buildName + "</b>");
        }
#endif
            #endregion

        //GET SCENE FROM BUILD
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
                    /*TODO*/
                    //MOBILE
                    //case BuildTarget.iOS: break;
                    //case BuildTarget.Android: break;
                    //CONSOLE
                    //case BuildTarget.PS4: break;
                    //case BuildTarget.XboxOne: break;
                    //case BuildTarget.Switch: break;
                    //WEB
                    //case BuildTarget.WebGL: buildFileExtension = FileExtension.exe; break;
                    /*    //OTHER
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
            buildPlayerOptions.locationPathName = "_BUILD" + "/" + buildType.ToString().ToUpper() + "/" + targetPlatform + "/" + buildType.ToString().ToLower() + "." + buildFileExtension;
            buildPlayerOptions.target = targetPlatform;
            buildPlayerOptions.options = (headless) ? (BuildOptions.EnableHeadlessMode) : (BuildOptions.None);

            #region  BUILD REPORT - header
#if !SKIP_BUILD_REPORT
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
#if !SKIP_BUILD_REPORT
                float sizeInMegabytes = (summary.totalSize / 1024f / 1024f);
                sizeInMegabytes -= sizeInMegabytes % 0.0001f;
                float durationInSeconds = (float)((summary.buildEndedAt - summary.buildStartedAt).TotalSeconds);
                durationInSeconds -= durationInSeconds % 0.01f;

                buildLog.Insert(0, ("<color=green><b>[" + buildType.ToString().ToUpper() + "]</b></color>")); //SUCCESS SYMBOL
                buildLog.AppendLine("<color=green><b>" + targetPlatform + " " + buildType + " build succeeded..." + "</b></color>"
                    + ((summary.totalSize > 0) ? ("\nBuild size: " + sizeInMegabytes + " MB") : (""))
                    + "\nBuild duration: " + durationInSeconds + "s");
#endif
                #endregion
            }
            //FAILURE
            if (summary.result == BuildResult.Failed)
            {
                #region  BUILD REPORT - failure
#if !SKIP_BUILD_REPORT
                buildLog.Insert(0, ("<color=red><b>[" + buildType.ToString().ToUpper() + "]</b></color>")); //FAILURE SYMBOL
                buildLog.AppendLine("<color=red><b>" + targetPlatform + " " + buildType + " build failed...</b></color>"
                    + "\n" + report.ToString());
#endif
                #endregion
            }
        }


        // - - - - - - - - - - - - - - - - -
        // M U L T I  D E P L O Y M E N T S
        // - - - - - - - - - - - - - - - - -

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
        /// <summary>Builds a client and a server back to back. It is recommended to use a Headless Server instead.</summary>
		public static void BuildClientAndServer(BuildTarget targetPlatform)
		{
            BuildServer(targetPlatform);
            BuildClient(targetPlatform);
        }
        //HEADLESS SERVER + CLIENT
        /// <summary>Builds a client and a headless server back to back.</summary>
		public static void BuildClientAndHeadlessServer(BuildTarget targetPlatform)
		{
            BuildHeadlessServer(targetPlatform);
            BuildClient(targetPlatform);
        }


        // - - - - - - - - - - - - - - - -
        // S O L O  D E P L O Y M E N T S
        // - - - - - - - - - - - - - - - -

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


        // - - - - - - - - - - -
        // B U I L D  M E N U S
        // - - - - - - - - - - -

        //FULL DEPLOYMENT
        [MenuItem("OpenMMO/Quick Build/FULL BUILD/Full Deployment (headless + multi-client) - [linux-osx-win64]", priority = 1)]
        public static void BuildFull()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("Full Deployment Package"); //INITIALIZE
#endif
            #endregion

            BuildFullDeployment(); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //WINDOWS
        //headless server + client
        [MenuItem("OpenMMO/Quick Build/WIN64/Client and Server (headless)", priority = 10)]
        public static void BuildWindows64ClientAndHeadlessServer()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("Windows 64 Headless Server and Client"); //INITIALIZE
#endif
            #endregion
            
            BuildClientAndHeadlessServer(BuildTarget.StandaloneWindows64); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //headless server
        [MenuItem("OpenMMO/Quick Build/WIN64/Server (headless)", priority = 11)]
        public static void BuildWindows64HeadlessServer()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("Windows 64 Headless Server"); //INITIALIZE
#endif
            #endregion

            BuildHeadlessServer(BuildTarget.StandaloneWindows64); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //client
        [MenuItem("OpenMMO/Quick Build/WIN64/Client", priority = 12)]
        public static void BuildWindows64Client()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("Windows 64 Client"); //INITIALIZE
#endif
            #endregion
            
            BuildClient(BuildTarget.StandaloneWindows64); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //MAC OSX
        //headless server + client
        [MenuItem("OpenMMO/Quick Build/OSX/Client and Server (headless)", priority = 20)]
        public static void BuildOSXClientAndHeadlessServer()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("OSX Headless Server and Client"); //INITIALIZE
#endif
            #endregion
            
            BuildClientAndHeadlessServer(BuildTarget.StandaloneOSX); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //headless server
        [MenuItem("OpenMMO/Quick Build/OSX/Server (headless)", priority = 21)]
        public static void BuildOSXHeadlessServer()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("OSX Headless Server"); //INITIALIZE
#endif
            #endregion
            
            BuildHeadlessServer(BuildTarget.StandaloneOSX); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //client
        [MenuItem("OpenMMO/Quick Build/OSX/Client", priority = 22)]
        public static void BuildOSXClient()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("OSX Client"); //INITIALIZE
#endif
            #endregion

            BuildClient(BuildTarget.StandaloneOSX); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //LINUX
        //headless server and client
        [MenuItem("OpenMMO/Quick Build/Linux/Client and Server (headless)", priority = 30)]
        public static void BuildLinuxClientAndHeadlessServer()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("Linux Headless Server and Client"); //INITIALIZE
#endif
            #endregion
            
            BuildClientAndHeadlessServer(BuildTarget.StandaloneLinux64); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //headless server
        [MenuItem("OpenMMO/Quick Build/Linux/Server (headless)", priority = 31)]
        public static void BuildLinuxHeadlessServer()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("Linux Headless Server"); //INITIALIZE
#endif
            #endregion
            
            BuildHeadlessServer(BuildTarget.StandaloneLinux64); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //client
        [MenuItem("OpenMMO/Quick Build/Linux/Client", priority = 32)]
        public static void BuildLinuxClient()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("Linux Client"); //INITIALIZE
#endif
            #endregion

            BuildClient(BuildTarget.StandaloneLinux64); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }
        //WEBGL
        //client
        [MenuItem("OpenMMO/Quick Build/WEBGL/Client", priority = 42)]
        public static void BuildWebGLClient()
        {
            #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("WebGL Client"); //INITIALIZE
#endif
            #endregion

            BuildClient(BuildTarget.WebGL); //BUILD

            #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
            #endregion
        }

        //TODO: Android and iOS
        //TODO: PS4, XBoxOne, Switch

#if TEST_MODE
        //OSX
        [MenuItem("OpenMMO/Quick Build/TEST_MODE/OSX Client and Server (NOT headless)", priority = 0)]
        public static void BuildOSXClientAndServer()
        {
        #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("OSX Server and Client"); //INITIALIZE
#endif
        #endregion
        
        BuildClientAndServer(BuildTarget.StandaloneOSX); //BUILD

        #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
        #endregion
        }
        //WIN64
        [MenuItem("OpenMMO/Quick Build/TEST_MODE/Win64 Client and Server (NOT headless)", priority = 1)]
        public static void BuildWin64ClientAndServer()
        {
        #region  BUILD REPORT - initialize
#if !SKIP_BUILD_REPORT
            InitializeBuildReport("Windows 64 Server and Client"); //INITIALIZE
#endif
        #endregion

        BuildClientAndServer(BuildTarget.StandaloneWindows64); //BUILD

        #region  BUILD REPORT
#if !SKIP_BUILD_REPORT
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
        #endregion
        }
#endif
    }
}
#endif