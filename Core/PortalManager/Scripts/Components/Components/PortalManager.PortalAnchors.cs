
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.Portals;
using OpenMMO.DebugManager;

namespace OpenMMO.Portals
{

	// ===================================================================================
	// PortalManager
	// ===================================================================================
	public partial class PortalManager
	{
	
		public static List<PortalAnchorEntry> portalAnchors = new List<PortalAnchorEntry>();
		
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
        	{
        		UnityEngine.Debug.Log(anchor.name+"/"+_name);
        		if (anchor.name == _name)
        			return true;
        	}	

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