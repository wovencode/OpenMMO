
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.Portals;
using OpenMMO.DebugManager;

namespace OpenMMO.Portals
{

	// ===================================================================================
	// AnchorManager
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class AnchorManager : MonoBehaviour
	{
	
		public static List<PortalAnchorEntry> portalAnchors = new List<PortalAnchorEntry>();
		public static List<GameObject> startAnchors = new List<GameObject>();
		
        // ============================ START ANCHORS ===================================
        
        // -------------------------------------------------------------------------------
        // GetArchetypeStartPosition
        // -------------------------------------------------------------------------------
        public static Transform GetArchetypeStartPosition(GameObject player)
        {

			PlayerComponent pc = player.GetComponent<PlayerComponent>();

			startAnchors.Shuffle();
			
            foreach (GameObject anchor in startAnchors)
            {

                StartAnchor sc = anchor.GetComponent<StartAnchor>();

                foreach (ArchetypeTemplate template in sc.archeTypes)
                    if (template == pc.archeType)
                        return anchor.transform;

            }

            return player.transform;

        }
        
        // -------------------------------------------------------------------------------
    	// RegisterStartAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public static void RegisterStartAnchor(GameObject anchor)
        {
            startAnchors.Add(anchor);
        }

        // -------------------------------------------------------------------------------
    	// UnRegisterStartAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public static void UnRegisterStartAnchor(GameObject anchor)
        {
           for (int i = 0; i < startAnchors.Count; i++)
           		if (startAnchors[i] == anchor)
           			startAnchors.RemoveAt(i);
        }
		
        // ============================ PORTAL ANCHORS ===================================
        
        // -------------------------------------------------------------------------------
    	// CheckPortalAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public static bool CheckPortalAnchor(string _name)
        {
        	if (String.IsNullOrWhiteSpace(_name))
        		return false;
        	
        	foreach (PortalAnchorEntry anchor in portalAnchors)
        		if (anchor.name == _name)
        			return true;

			return false;
        }
        
        // -------------------------------------------------------------------------------
    	// GetPortalAnchorPosition
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public static Vector3 GetPortalAnchorPosition(string _name)
        {
        
        	foreach (PortalAnchorEntry anchor in portalAnchors)
        		if (anchor.name == _name)
					return anchor.position;
					
			return Vector3.zero;
        }
        
        // -------------------------------------------------------------------------------
    	// RegisterPortalAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public static void RegisterPortalAnchor(string _name, Vector3 _position)
        {
            portalAnchors.Add(
            				new PortalAnchorEntry
            				{
            					name = _name,
            					position = _position
            				}
            );
        }

        // -------------------------------------------------------------------------------
    	// UnRegisterPortalAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public static void UnRegisterPortalAnchor(string _name)
        {
           
           for (int i = 0; i < portalAnchors.Count; i++)
           		if (portalAnchors[i].name == _name)
           			portalAnchors.RemoveAt(i);
            
        }

    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================