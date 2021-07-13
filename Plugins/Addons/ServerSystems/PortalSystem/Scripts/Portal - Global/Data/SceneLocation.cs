//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;

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
}