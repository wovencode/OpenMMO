
using System;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Portals;

namespace OpenMMO.Portals
{
	
	// -----------------------------------------------------------------------------------
	// SceneLocation
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class SceneLocation
	{
	
		public UnityScene scene;
		public Vector3 position;

		public bool Valid
		{
			get
			{
				return scene.IsSet();
			}
		}
		
	}
	
	// -----------------------------------------------------------------------------------
	// PortalAnchorEntry
	// -----------------------------------------------------------------------------------
	public partial class PortalAnchorEntry
	{
		public string name;
		public Vector3 position;
	}
	
	// -----------------------------------------------------------------------------------
	// StartAnchorEntry
	// -----------------------------------------------------------------------------------
	public partial class StartAnchorEntry
	{
		public string name;
		public Vector3 position;
		public ArchetypeTemplate[] archeTypes;
	}
	
	// -----------------------------------------------------------------------------------
	
}

// =======================================================================================