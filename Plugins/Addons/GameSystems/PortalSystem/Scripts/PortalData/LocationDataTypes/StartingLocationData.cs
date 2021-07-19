//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;

namespace OpenMMO.Zones
{
	/// <summary>
	/// This class stores a starting location (name, position and archetypes that can start there)
	/// </summary>
	public partial class StartingLocationData
	{
		public string name;
		public Vector3 position;
		public ArchetypeTemplate[] archeTypes;
	}
}