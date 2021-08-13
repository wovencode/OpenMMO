//BY DX4D

using System;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu( menuName = "OpenMMO/Network/Zones/Zone Server Process Launcher", order = 0)]
public class ZoneServerProcessLauncher : ScriptableProcessLauncher
{
    [SerializeField] string _commandLineArgument = "-zone";
    public string commandLineArgument => _commandLineArgument;

    public enum AppExtension { exe, x86_64, app }
    public bool LaunchProcess(int processIndex)
    {
        string _fileName;
        string _commandLineArgs = commandLineArgument + " " + processIndex.ToString(); ;
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
        if (Application.platform != RuntimePlatform.OSXPlayer)
        {
            _fileName = "server" + "." + extension.ToString(); //LINUX and WINDOWS
        }
        else
        {
            _fileName = GetProcessPath; //OSX
            //TODO: This is a potential fix for the server executable being locked into requiring a specific name.
            //process.StartInfo.FileName = Path.GetFullPath(Tools.GetProcessPath) + "/" + Path.GetFileNameWithoutExtension(Tools.GetProcessPath) + "." + Path.GetExtension(Tools.GetProcessPath);
        }

        return LaunchProcess(_fileName, _commandLineArgs);
        /*
        Process process = new Process(); //CREATE NEW PROCESS
        process.StartInfo.FileName = GetProcessPath; //SYSTEM AGNOSTIC FILENAME
        process.StartInfo.Arguments = commandLineArgs; //COMMAND LINE ARGS

        //string filePath = process.StartInfo.WorkingDirectory + process.StartInfo.FileName;

        //UnityEngine.Debug.Log("[SERVER] - Zone Server - Launching Server @" + filePath + "...");

        if (System.IO.File.Exists(process.StartInfo.FileName)) //FILE EXISTS - LAUNCH PROCESS
        {
            process.Start(); //LAUNCH THE SERVER PROCESS
            return true; //PROCESS LAUNCHED SUCCESSFULLY
        }
        else
        {
            return false; //NO SERVER EXE FOUND TO LAUNCH PROCESS
        }

        //debug.LogFormat(this.name, nameof(LaunchSubZoneServer), index.ToString()); //DEBUG //REMOVED DX4D
        */
    }
}
