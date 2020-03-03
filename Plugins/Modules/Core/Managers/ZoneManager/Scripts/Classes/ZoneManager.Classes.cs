//by Fhiz
using System;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Zones;

namespace OpenMMO.Zones
{
	
	/// <summary>
	/// This class allows to store a scene and makes it drag-n-drop able in the Inspector
	/// </summary>
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
	
	/// <summary>
	/// This class stores a portal anchor (name and position)
	/// </summary>
	public partial class PortalAnchorEntry
	{
		public string name;
		public Vector3 position;
	}
	
	/// <summary>
	/// This class stores a start position anchor (name, position and archetypes that can start there)
	/// </summary>
	public partial class StartAnchorEntry
	{
		public string name;
		public Vector3 position;
		public ArchetypeTemplate[] archeTypes;
	}
	
}