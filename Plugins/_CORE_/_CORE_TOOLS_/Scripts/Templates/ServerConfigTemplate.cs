//BY FHIZ
//MODIFIED BY DX4D

using System;
using System.Collections.Generic;
using OpenMMO;

using UnityEngine;
using System.Linq;
using OpenMMO.Network;
using OpenMMO.Debugging;

namespace OpenMMO
{
	
	/// <summary>
	/// Contains configuration for the project itself (like debug mode, location of logs etc.)
	/// </summary>
	[CreateAssetMenu(fileName = "New Server Configuration", menuName = "OpenMMO - Configuration/New Server Configuration", order = 999)]
	public partial class ServerConfigTemplate : ScriptableObject
	{

		[Header("Project Configuration")]
#pragma warning disable CS0649
		[Tooltip("Toggle type of build: Server, Client or Host+Play")]
		[SerializeField] NetworkType _networkType;
        internal NetworkType networkType
        {
            get { return _networkType; }
            set
            {
                if (_networkType != value)
                {
                    _networkType = value;
#if UNITY_EDITOR
                    UpdateScriptingDefines(); //ADD SCRIPTING DEFINES
#endif //UNITY_EDITOR
                }
            }
        }
#pragma warning restore CS0649
		[Tooltip("Toggle debug mode for logging (globally, affects all components with a DebugHelper as well)")]
		public bool globalDebugMode;
		
		[Header("Security")]
    	public string securitySalt = "16_or_more_bytes";
		
		[Header("Servers")]
		public ServerInfoTemplate[] serverList;
		
		[Header("Logging")]
		public bool logMode;
		[Tooltip("Filename for the text log file (zone and chat servers append a random suffix to this)")]
		public string logFilename = "OpenMMOLog";
		[Tooltip("Foldername for all text log files (normally found inside project folder or project package)")]
		public string logFolder = "OpenMMOLogs";
		
		static ServerConfigTemplate _instance;
		
		/// <summary>
		/// Creates a singleton on this class to be accesible from code anywhere. Singleton is OK in this situation because this template (= Scriptable Object) exists only once.
		/// </summary>
		public static ServerConfigTemplate singleton
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<ServerConfigTemplate>().FirstOrDefault();
				return _instance;
			}
		}
		
		/// <summary>
		/// Public getter for NetworkType, so that the original property cannot be modified from external code.
		/// </summary>
		public NetworkType GetNetworkType
		{
			get
			{
				return networkType;
			}
		}

        /// <summary>
        /// After changing, updates scripting defines and recompiles the code.
        /// </summary>
        //public void OnValidate() { }

        // U P D A T E  S C R I P T I N G  D E F I N E S
        void UpdateScriptingDefines()
        {
            switch (networkType)
            {
                case NetworkType.Client: //CLIENT
                    {
                        SwitchToClientMode();
                        break;
                    }
                case NetworkType.Server: //SERVER
                    {
                        SwitchToServerMode();
                        break;
                    }
                case NetworkType.HostAndPlay: //HOST & PLAY
                    {
                        SwitchToHostMode();
                        break;
                    }
            }
        }

        // S W I T C H  M O D E
        //CLIENT
        void SwitchToClientMode()
        {
            RemoveServerDefine();
            AddClientDefine();
            //DebugManager.Log("<b><color=blue>[ProjectConfig] Switched to CLIENT mode.</color></b>");
        }
        //SERVER
        void SwitchToServerMode()
        {
            RemoveClientDefine();
            AddServerDefine();
			//DebugManager.Log("<b><color=yellow>[ProjectConfig] Switched to SERVER mode.</color></b>");
        }
        //HOST
        void SwitchToHostMode()
        {
            AddServerDefine();
            AddClientDefine();
            //DebugManager.Log("<b><color=green>[ProjectConfig] Switched to HOST & PLAY mode.</color></b>");
        }

        // C H A N G E  S E R V E R  D E F I N E S
        //SERVER
        void AddServerDefine()
        {
#if UNITY_EDITOR
            if (!EditorTools.HasScriptingDefine(Defines.BuildMode.Server)) //IF NO SERVER DEFINE
            {
                EditorTools.AddScriptingDefine(Defines.BuildMode.Server); //ADD SERVER DEFINE
            }
#endif
        }
        void RemoveServerDefine()
        {
#if UNITY_EDITOR
            if (EditorTools.HasScriptingDefine(Defines.BuildMode.Server)) //IF HAS SERVER DEFINE
            {
                EditorTools.RemoveScriptingDefine(Defines.BuildMode.Server); //REMOVE SERVER DEFINE
            }
#endif
        }
        //CLIENT
        void AddClientDefine()
        {
#if UNITY_EDITOR
            if (!EditorTools.HasScriptingDefine(Defines.BuildMode.Client)) //IF NO CLIENT DEFINE
            {
                EditorTools.AddScriptingDefine(Defines.BuildMode.Client); //ADD CLIENT DEFINE
            }
#endif
        }
        void RemoveClientDefine()
        {
#if UNITY_EDITOR
            if (EditorTools.HasScriptingDefine(Defines.BuildMode.Client)) //IF HAS CLIENT DEFINE
            {
                EditorTools.RemoveScriptingDefine(Defines.BuildMode.Client); //REMOVE CLIENT DEFINE
            }
#endif
        }
	}

}