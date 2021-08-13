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
    public enum FileExtension { exe, x86_64, app, apk }

    // ===================================================================================
    // QuickBuildMenu
    // ===================================================================================
    public class QuickBuildMenu
	{
        /// The parent folder that all options will fall under
        const string MENU_FOLDER = "OpenMMO/Quick Build/";

        //FULL DEPLOY CONFIG
        /// <summary>When running a Full Deploy, this is the type of server that will be built.</summary>
        public static BuildTarget fullDeployServerType = BuildTarget.StandaloneLinux64;
        /// <summary>When running a Full Deploy, these are the types of clients that will be built.</summary>
        public static BuildTarget[] fullDeployClientTypes = new BuildTarget[] {
            BuildTarget.StandaloneWindows64,
            BuildTarget.StandaloneOSX };

        #region BUILD REPORT - build log
#if !SKIP_BUILD_REPORT
        static StringBuilder buildLog = new StringBuilder();//(" <color=orange><b>[BUILD REPORT]</b></color> ");
#endif
        #endregion
        
        #region INITIALIZE BUILD REPORT
        static void InitializeBuildReport(string buildName)
        {
#if !SKIP_BUILD_REPORT
            buildLog = new StringBuilder();
            buildLog.AppendLine("<b>" + buildName + "</b>");
#endif
        }
        #endregion //END INITIALIZE BUILD REPORT

        #region PRINT BUILD REPORT
        /// <summary>Writes the build report to the debug console.</summary>
        static void PrintBuildReport()
        {
#if !SKIP_BUILD_REPORT
            buildLog.Insert(0, "<b>[<color=purple>BUILD REPORT</color>] -</b>");
            Debug.Log(buildLog.ToString()); //PRINT BUILD LOG
            buildLog.Clear(); //CLEAR BUILD LOG
#endif
        }
        #endregion //END PRINT BUILD REPORT

        #region GET SCENE FROM BUILD
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
        #endregion //END GET SCENES FROM BUILD

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
                    //WEB
                    //case BuildTarget.WebGL: buildFileExtension = FileExtension.exe; break;
                    //MOBILE
                    //case BuildTarget.iOS: break;
                case BuildTarget.Android: buildFileExtension = FileExtension.apk; break;
                    //CONSOLE
                    //case BuildTarget.PS4: break;
                    //case BuildTarget.PS5: break;
                    //case BuildTarget.XboxOne: break;
                    //case BuildTarget.Switch: break;
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

                buildLog.Insert(0, ("<color=green><b>[" + buildType.ToString().ToUpper() + "] </b></color>")); //SUCCESS SYMBOL
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
            Debug.Log("BUILDING " + targetPlatform.ToString().ToUpper() + " HEADLESS SERVER..."
                + "\n" + "PLEASE WAIT... (this may take a while)");
            Build(targetPlatform, NetworkType.Server, true);
        }
        //SERVER
        /// <summary>Builds a server for the target platform. It is recommended to use a Headless Server instead.</summary>
        public static void BuildServer(BuildTarget targetPlatform)
        {
            Debug.Log("BUILDING " + targetPlatform.ToString().ToUpper() + " SERVER"
                + "\n" + "PLEASE WAIT... (this may take a while)");
            Build(targetPlatform, NetworkType.Server);
        }
        //CLIENT
        /// <summary>Builds a client for the target platform.</summary>
        public static void BuildClient(BuildTarget targetPlatform)
		{
            Debug.Log("BUILDING " + targetPlatform.ToString().ToUpper() + " CLIENT"
                + "\n" + "PLEASE WAIT... (this may take a while)");
            Build(targetPlatform, NetworkType.Client);
        }


        // - - - - - - - - - - - - - - - - -
        // B U I L D  M E N U  O P T I O N S
        // - - - - - - - - - - - - - - - - -

        //FULL DEPLOYMENT
        [MenuItem(MENU_FOLDER + "FULL BUILD/Full Deployment (headless + multi-client) - [linux-osx-win64]", priority = 1)]
        public static void BuildFull()
        {
            InitializeBuildReport("Full Deployment Package"); //INITIALIZE
            BuildFullDeployment(); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }

        // - - - - - - - - -
        // P C  B U I L D S
        // - - - - - - - -
        //WINDOWS
        //headless server + client
        [MenuItem(MENU_FOLDER + "WIN64/Client and Server (headless)", priority = 10)]
        public static void BuildWindows64ClientAndHeadlessServer()
        {
            InitializeBuildReport("Windows 64 Headless Server and Client"); //INITIALIZE
            BuildClientAndHeadlessServer(BuildTarget.StandaloneWindows64); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //headless server
        [MenuItem(MENU_FOLDER + "WIN64/Server (headless)", priority = 11)]
        public static void BuildWindows64HeadlessServer()
        {
            InitializeBuildReport("Windows 64 Headless Server"); //INITIALIZE
            BuildHeadlessServer(BuildTarget.StandaloneWindows64); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //client
        [MenuItem(MENU_FOLDER + "WIN64/Client", priority = 12)]
        public static void BuildWindows64Client()
        {
            InitializeBuildReport("Windows 64 Client"); //INITIALIZE
            BuildClient(BuildTarget.StandaloneWindows64); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //MAC OSX
        //headless server + client
        [MenuItem(MENU_FOLDER + "OSX/Client and Server (headless)", priority = 20)]
        public static void BuildOSXClientAndHeadlessServer()
        {
            InitializeBuildReport("OSX Headless Server and Client"); //INITIALIZE
            BuildClientAndHeadlessServer(BuildTarget.StandaloneOSX); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //headless server
        [MenuItem(MENU_FOLDER + "OSX/Server (headless)", priority = 21)]
        public static void BuildOSXHeadlessServer()
        {
            InitializeBuildReport("OSX Headless Server"); //INITIALIZE
            BuildHeadlessServer(BuildTarget.StandaloneOSX); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //client
        [MenuItem(MENU_FOLDER + "OSX/Client", priority = 22)]
        public static void BuildOSXClient()
        {
            InitializeBuildReport("OSX Client"); //INITIALIZE
            BuildClient(BuildTarget.StandaloneOSX); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //LINUX
        //headless server and client
        [MenuItem(MENU_FOLDER + "Linux/Client and Server (headless)", priority = 30)]
        public static void BuildLinuxClientAndHeadlessServer()
        {
            InitializeBuildReport("Linux Headless Server and Client"); //INITIALIZE
            BuildClientAndHeadlessServer(BuildTarget.StandaloneLinux64); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //headless server
        [MenuItem(MENU_FOLDER + "Linux/Server (headless)", priority = 31)]
        public static void BuildLinuxHeadlessServer()
        {
            InitializeBuildReport("Linux Headless Server"); //INITIALIZE
            BuildHeadlessServer(BuildTarget.StandaloneLinux64); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //client
        [MenuItem(MENU_FOLDER + "Linux/Client", priority = 32)]
        public static void BuildLinuxClient()
        {
            InitializeBuildReport("Linux Client"); //INITIALIZE
            BuildClient(BuildTarget.StandaloneLinux64); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }

        // - - - - - - - - - -
        // W E B  B U I L D S
        // - - - - - - - - -
        //WEBGL
        //client
        [MenuItem(MENU_FOLDER + "WEBGL/Client", priority = 40)]
        public static void BuildWebGLClient()
        {
            InitializeBuildReport("WebGL Client"); //INITIALIZE
            BuildClient(BuildTarget.WebGL); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }

        // - - - - - - - - - - - - -
        // M O B I L E  B U I L D S
        // - - - - - - - - - - - -
        //ANDROID
        //client
        [MenuItem(MENU_FOLDER + "Android/Client", priority = 50)]
        public static void BuildAndroidClient()
        {
            InitializeBuildReport("Android Client"); //INITIALIZE
            BuildClient(BuildTarget.Android); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //iOS
        //client
        [MenuItem(MENU_FOLDER + "iOS/Client", priority = 51)]
        public static void BuildIOSClient()
        {
            InitializeBuildReport("iOS Client"); //INITIALIZE
            BuildClient(BuildTarget.iOS); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }

        // - - - - - - - - - - - - - -
        // C O N S O L E  B U I L D S
        // - - - - - - - - - - - - -
        //NINTENDO SWITCH
        [MenuItem(MENU_FOLDER + "Nintendo Switch/Client", priority = 60)]
        public static void BuildNintendoSwitchClient()
        {
            InitializeBuildReport("Nintendo Switch Client"); //INITIALIZE
            BuildClient(BuildTarget.Switch); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //XBOX ONE
        [MenuItem(MENU_FOLDER + "XBox One/Client", priority = 61)]
        public static void BuildXBoxOneClient()
        {
            InitializeBuildReport("XBox One Client"); //INITIALIZE
            BuildClient(BuildTarget.XboxOne); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //PS4
        [MenuItem(MENU_FOLDER + "PS4/Client", priority = 62)]
        public static void BuildPS4Client()
        {
            InitializeBuildReport("PS4 Client"); //INITIALIZE
            BuildClient(BuildTarget.PS4); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //PS5
        [MenuItem(MENU_FOLDER + "PS5/Client", priority = 63)]
        public static void BuildPS5Client()
        {
            InitializeBuildReport("PS5 Client"); //INITIALIZE
            BuildClient(BuildTarget.PS5); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }

#if TEST_MODE
        //OSX
        [MenuItem(MENU_FOLDER + "TEST_MODE/OSX Client and Server (NOT headless)", priority = 0)]
        public static void BuildOSXClientAndServer()
        {
            InitializeBuildReport("OSX Server and Client"); //INITIALIZE
            BuildClientAndServer(BuildTarget.StandaloneOSX); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
        //WIN64
        [MenuItem(MENU_FOLDER + "TEST_MODE/Win64 Client and Server (NOT headless)", priority = 1)]
        public static void BuildWin64ClientAndServer()
        {
            InitializeBuildReport("Windows 64 Server and Client"); //INITIALIZE
            BuildClientAndServer(BuildTarget.StandaloneWindows64); //BUILD
            PrintBuildReport(); //OUTPUT BUILD REPORT TO CONSOLE
        }
#endif
    }
}
#endif