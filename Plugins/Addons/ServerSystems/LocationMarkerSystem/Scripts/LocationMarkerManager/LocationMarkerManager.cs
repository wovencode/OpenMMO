
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.Zones;
using OpenMMO.Debugging;

namespace OpenMMO.Zones
{
	[DisallowMultipleComponent]
	public partial class LocationMarkerManager : MonoBehaviour
	{
	
		public List<LocationMarkerEntry> portalAnchors = new List<LocationMarkerEntry>();
		public List<StartingLocationData> startAnchors = new List<StartingLocationData>();
		
		public static LocationMarkerManager singleton;
		
		// -------------------------------------------------------------------------------
        // Awake
        // -------------------------------------------------------------------------------
		void Awake()
		{
			portalAnchors.Clear();
			startAnchors.Clear();
			singleton = this;
		}
		
        // ============================ START ANCHORS ====================================
        
        // -------------------------------------------------------------------------------
        // GetArchetypeStartPositionAnchorName
        // -------------------------------------------------------------------------------
        public string GetArchetypeStartPositionAnchorName(GameObject player)
        {

			PlayerAccount pc = player.GetComponent<PlayerAccount>();

			startAnchors.Shuffle();
			
            foreach (StartingLocationData anchor in startAnchors)
            {
                foreach (ArchetypeTemplate template in anchor.archeTypes)
                {
                    if (template == pc.archeType)
                    {
						DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(GetArchetypeStartPositionAnchorName), anchor.name); //DEBUG
                        return anchor.name;
					}
				}
            }
			
			DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(GetArchetypeStartPositionAnchorName), "NOT FOUND"); //DEBUG
            return "";

        }
        
        // -------------------------------------------------------------------------------
    	// CheckStartAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public bool CheckStartAnchor(string _name)
        {
        
        	if (String.IsNullOrWhiteSpace(_name))
        		return false;
        	
        	foreach (StartingLocationData anchor in startAnchors)
        	{
        		if (anchor.name == _name)
        		{
        			DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(CheckStartAnchor), anchor.name); //DEBUG
        			return true;
				}
			}
			
			DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(CheckStartAnchor), _name, "NOT FOUND"); //DEBUG
			return false;
        }
        
        // -------------------------------------------------------------------------------
    	// GetStartAnchorPosition
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public Vector3 GetStartAnchorPosition(string _name)
        {
        
        	foreach (StartingLocationData anchor in startAnchors)
        	{
        		if (anchor.name == _name)
				{
					DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(GetStartAnchorPosition), anchor.name); //DEBUG
					return anchor.position;
				}
			}
			
			DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(GetStartAnchorPosition), _name, "NOT FOUND"); //DEBUG
			return Vector3.zero;
        }
        
        // -------------------------------------------------------------------------------
    	// RegisterStartingLocationMarker
    	// we are not using a game object or component here as this won't work across scenes!
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public void RegisterStartingLocationMarker(string _name, Vector3 _position, ArchetypeTemplate[] _archeTypes)
        {
        
            startAnchors.Add(
            				new StartingLocationData
            				{
            					name 		= _name,
            					position 	= _position,
            					archeTypes 	= _archeTypes
            				}
            );
            
            DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(RegisterStartingLocationMarker), _name); //DEBUG
        }

        // -------------------------------------------------------------------------------
    	// UnRegisterStartingLocationMarker
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public void UnRegisterStartingLocationMarker(string _name)
        {
           for (int i = startAnchors.Count-1; i >= 0; i--)
           {
           		if (startAnchors[i].name == _name)
           		{
           			startAnchors.RemoveAt(i);
           			DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(UnRegisterStartingLocationMarker), _name); //DEBUG
           		}
           	}
        }
		
        // ============================ PORTAL ANCHORS ===================================
        
        // -------------------------------------------------------------------------------
    	// CheckPortalAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public bool CheckPortalAnchor(string _name)
        {
        
        	if (String.IsNullOrWhiteSpace(_name))
        		return false;
        	
        	foreach (LocationMarkerEntry anchor in portalAnchors)
        	{
        		if (anchor.name == _name)
        		{
        			DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(CheckPortalAnchor), anchor.name); //DEBUG
        			return true;
				}
			}
			
			DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(CheckPortalAnchor), _name, "NOT FOUND"); //DEBUG
			return false;
        }
        
        // -------------------------------------------------------------------------------
    	// GetPortalAnchorPosition
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public Vector3 GetPortalAnchorPosition(string _name)
        {
        
        	foreach (LocationMarkerEntry anchor in portalAnchors)
        	{
        		if (anchor.name == _name)
				{
					DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(GetPortalAnchorPosition), anchor.name); //DEBUG
					return anchor.position;
				}
			}
			
			DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(GetPortalAnchorPosition), _name, "NOT FOUND"); //DEBUG
			return Vector3.zero;
        }
        
        // -------------------------------------------------------------------------------
    	// RegisterPortalAnchor
    	// we are not using a game object or component here as this won't work across scenes!
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public void RegisterLocationMarker(string _name, Vector3 _position)
        {
            portalAnchors.Add(
            				new LocationMarkerEntry
            				{
            					name = _name,
            					position = _position
            				}
            );
            
            DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(RegisterLocationMarker), _name); //DEBUG
        }

        // -------------------------------------------------------------------------------
        // UnRegisterPortalAnchor
        // @Client / @Server
        // -------------------------------------------------------------------------------
        public void UnRegisterLocationMarker(string _name)
        {
           for (int i = portalAnchors.Count-1; i >= 0; i--)
           {
           		if (portalAnchors[i].name == _name)
           		{
           			portalAnchors.RemoveAt(i);
            		DebugManager.LogFormat(nameof(LocationMarkerManager), nameof(UnRegisterLocationMarker), _name); //DEBUG
            	}
            }
        }
       
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================