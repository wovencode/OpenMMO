
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// NetworkZoneTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New NetworkZone", menuName = "OpenMMO - Servers/New NetworkZone", order = 999)]
	public partial class NetworkZoneTemplate : ScriptableTemplate
	{
    	
    	[Header("NetworkZone")]
    	public ServerInfoTemplate server;
    	public UnityScene scene;
    	
		[Tooltip("Times out after zoneIntervalMain * zoneTimeoutMultiplier (in seconds, 0 to disable)")]
		public int zoneTimeoutMultiplier = 6;
	
    	// -------------------------------------------------------------------------------
    	
		public static string _folderName = "";
		
		static NetworkZoneTemplateDictionary _data;
		
		// -------------------------------------------------------------------------------
        // GetZoneBySceneName
        // -------------------------------------------------------------------------------
		public static NetworkZoneTemplate GetZoneBySceneName(string name)
		{
		
			foreach (KeyValuePair<int, NetworkZoneTemplate> template in data)
			{
				if (template.Value.scene != null && template.Value.scene.SceneName == name)
					return template.Value;
			}
			
			return null;
		
		
		}
		
		
		// -------------------------------------------------------------------------------
        // data
        // -------------------------------------------------------------------------------
		public static ReadOnlyDictionary<int, NetworkZoneTemplate> data
		{
			get {
				NetworkZoneTemplate.BuildCache();
				return _data.data;
			}
		}
		
		// -------------------------------------------------------------------------------
        // BuildCache
        // -------------------------------------------------------------------------------
		public static void BuildCache(bool forced=false)
		{
			if (_data == null || forced)
				_data = new NetworkZoneTemplateDictionary(NetworkZoneTemplate._folderName);
		}
		
		// -------------------------------------------------------------------------------
        // OnEnable
        // -------------------------------------------------------------------------------
		public void OnEnable()
		{
			if (_folderName != folderName)
				_folderName = folderName;
			
			_data = null;
			
		}
		
		// -------------------------------------------------------------------------------
        // OnValidate
        // You can add custom validation checks here
        // -------------------------------------------------------------------------------
		public override void OnValidate()
		{
			base.OnValidate();
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================