
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// ServerInfoTemplate
	// ===================================================================================
	[CreateAssetMenu(fileName = "New ServerInfo", menuName = "OpenMMO - Servers/New ServerInfo", order = 999)]
	public partial class ServerInfoTemplate : ScriptableTemplate
	{
		
		public string ip = "localhost";
		public bool visible = true;
		
		// -------------------------------------------------------------------------------
		
		public static string _folderName = "";	
		
		static ServerInfoTemplateDictionary _data;
		
		// -------------------------------------------------------------------------------
        // data
        // -------------------------------------------------------------------------------
		public static ReadOnlyDictionary<int, ServerInfoTemplate> data
		{
			get {
				ServerInfoTemplate.BuildCache();
				return _data.data;
			}
		}
		
		// -------------------------------------------------------------------------------
        // BuildCache
        // -------------------------------------------------------------------------------
		public static void BuildCache(bool forced=false)
		{
			if (_data == null || forced)
				_data = new ServerInfoTemplateDictionary(ServerInfoTemplate._folderName);
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
        // -------------------------------------------------------------------------------
		public override void OnValidate()
		{
			base.OnValidate();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================