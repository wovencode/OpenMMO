//BY DX4D

using System;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu( menuName = "OpenMMO/System/Process Launcher", order = 0)]
public class ScriptableProcessLauncher : ScriptableObject, IProcessLauncher
{
    //public enum AppExtension { exe, x86_64, app }
    public bool LaunchProcess(string processFileName, string commandLineArgs)
    {
        /*
        AppExtension extension = AppExtension.exe;
        switch (Application.platform)
        {
            //STANDALONE
            case RuntimePlatform.WindowsPlayer: extension = AppExtension.exe; break;
            case RuntimePlatform.OSXPlayer: extension = AppExtension.app; break;
            case RuntimePlatform.LinuxPlayer: extension = AppExtension.x86_64; break;
            //EDITOR
            case RuntimePlatform.WindowsEditor: extension = AppExtension.exe; break;
            case RuntimePlatform.OSXEditor: extension = AppExtension.app; break;
            case RuntimePlatform.LinuxEditor: extension = AppExtension.x86_64; break;
                //MOBILE + CONSOLE
                //NOTE: Probably no reason to use these for a server...maybe in some cases though (couch co-op games etc)
                //case RuntimePlatform.WebGLPlayer: break; case RuntimePlatform.IPhonePlayer: break; case RuntimePlatform.Android: break;
                //case RuntimePlatform.PS4: break; case RuntimePlatform.XboxOne: break; case RuntimePlatform.Switch: break;
        }
        
        //NOTE: This is a workaround, we leave the switch logic above just in case we ever check this extension variable in the future to extend this method.
        if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            process.StartInfo.FileName = GetProcessPath; //OSX
            //TODO: This is a potential fix for the server executable being locked into requiring a specific name.
            //process.StartInfo.FileName = Path.GetFullPath(Tools.GetProcessPath) + "/" + Path.GetFileNameWithoutExtension(Tools.GetProcessPath) + "." + Path.GetExtension(Tools.GetProcessPath);
        }
        else
        {
            process.StartInfo.FileName = "server" + "." + extension.ToString(); //LINUX and WINDOWS
        }*/

        Process process = new Process(); //CREATE NEW PROCESS
        process.StartInfo.FileName = processFileName; //FILENAME TO LAUNCH
        //TODO: GetProcessPath if this is empty
        process.StartInfo.Arguments = commandLineArgs; //COMMAND LINE ARGS

        //string filePath = process.StartInfo.WorkingDirectory + process.StartInfo.FileName;

        //UnityEngine.Debug.Log("[SERVER] - Zone Server - Launching Server @" + filePath + "...");

        if (System.IO.File.Exists(process.StartInfo.FileName)) //FILE EXISTS - LAUNCH PROCESS
        {
            process.Start(); //LAUNCH THE PROCESS
            return true; //PROCESS LAUNCHED SUCCESSFULLY
        }
        else
        {
            return false; //NO FILE FOUND TO LAUNCH
        }

        //debug.LogFormat(this.name, nameof(LaunchSubZoneServer), index.ToString()); //DEBUG //REMOVED DX4D
    }
    /// <summary>
    /// Returns the path this process has been started with. The first argument is the fileName itself.
    /// </summary>
    /// <remarks>
    /// Note: Arguments are always null on android - their usage only makes sense on an OS capable of hosting a server.
    /// </remarks>
    public string GetProcessPath
    {
        get
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args != null)
            {
                if (!String.IsNullOrWhiteSpace(args[0]))
                {
                    return args[0];
                }
            }

            return String.Empty;
        }
    }
}
