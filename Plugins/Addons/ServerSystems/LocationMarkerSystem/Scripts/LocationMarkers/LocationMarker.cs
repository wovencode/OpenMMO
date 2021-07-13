//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;

namespace OpenMMO.Zones
{
    /// <summary>
    /// A Location Marker is attached to a gameobject in a scene to mark a warp location for portals.
    /// Location Markers are tracked and validated through the Location Marker Manager.
    /// Location Markers are automatically registered when the marker is activated.
    /// Location Markers are automatically unregistered when the marker is deactivated.
    /// </summary>
    [DisallowMultipleComponent]
	public class LocationMarker : MonoBehaviour
	{
        public void Awake()
        {
            Invoke(nameof(AwakeLate), 0.1f);
        }
        
		void AwakeLate()
		{
			LocationMarkerManager.singleton.RegisterLocationMarker(name, transform.position);
		}
		
        public void OnDestroy()
        {
        	if (LocationMarkerManager.singleton)
            	LocationMarkerManager.singleton.UnRegisterLocationMarker(name);
        }
	}
}