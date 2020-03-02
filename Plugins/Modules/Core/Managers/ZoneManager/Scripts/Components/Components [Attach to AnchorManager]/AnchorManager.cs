
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

	// ===================================================================================
	// AnchorManager
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class AnchorManager : MonoBehaviour
	{
	
		public List<PortalAnchorEntry> portalAnchors = new List<PortalAnchorEntry>();
		public List<StartAnchorEntry> startAnchors = new List<StartAnchorEntry>();
		
		public static AnchorManager singleton;
		
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

			PlayerComponent pc = player.GetComponent<PlayerComponent>();

			startAnchors.Shuffle();
			
            foreach (StartAnchorEntry anchor in startAnchors)
            {
                foreach (ArchetypeTemplate template in anchor.archeTypes)
                {
                    if (template == pc.archeType)
                    {
						DebugManager.LogFormat(nameof(AnchorManager), nameof(GetArchetypeStartPositionAnchorName), anchor.name); //DEBUG
                        return anchor.name;
					}
				}
            }
			
			DebugManager.LogFormat(nameof(AnchorManager), nameof(GetArchetypeStartPositionAnchorName), "NOT FOUND"); //DEBUG
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
        	
        	foreach (StartAnchorEntry anchor in startAnchors)
        	{
        		if (anchor.name == _name)
        		{
        			DebugManager.LogFormat(nameof(AnchorManager), nameof(CheckStartAnchor), anchor.name); //DEBUG
        			return true;
				}
			}
			
			DebugManager.LogFormat(nameof(AnchorManager), nameof(CheckStartAnchor), _name, "NOT FOUND"); //DEBUG
			return false;
        }
        
        // -------------------------------------------------------------------------------
    	// GetStartAnchorPosition
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public Vector3 GetStartAnchorPosition(string _name)
        {
        
        	foreach (StartAnchorEntry anchor in startAnchors)
        	{
        		if (anchor.name == _name)
				{
					DebugManager.LogFormat(nameof(AnchorManager), nameof(GetStartAnchorPosition), anchor.name); //DEBUG
					return anchor.position;
				}
			}
			
			DebugManager.LogFormat(nameof(AnchorManager), nameof(GetStartAnchorPosition), _name, "NOT FOUND"); //DEBUG
			return Vector3.zero;
        }
        
        // -------------------------------------------------------------------------------
    	// RegisterStartAnchor
    	// we are not using a game object or component here as this won't work across scenes!
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public void RegisterStartAnchor(string _name, Vector3 _position, ArchetypeTemplate[] _archeTypes)
        {
        
            startAnchors.Add(
            				new StartAnchorEntry
            				{
            					name 		= _name,
            					position 	= _position,
            					archeTypes 	= _archeTypes
            				}
            );
            
            DebugManager.LogFormat(nameof(AnchorManager), nameof(RegisterStartAnchor), _name); //DEBUG
        }

        // -------------------------------------------------------------------------------
    	// UnRegisterStartAnchor
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public void UnRegisterStartAnchor(string _name)
        {
           for (int i = startAnchors.Count-1; i >= 0; i--)
           {
           		if (startAnchors[i].name == _name)
           		{
           			startAnchors.RemoveAt(i);
           			DebugManager.LogFormat(nameof(AnchorManager), nameof(UnRegisterStartAnchor), _name); //DEBUG
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
        	
        	foreach (PortalAnchorEntry anchor in portalAnchors)
        	{
        		if (anchor.name == _name)
        		{
        			DebugManager.LogFormat(nameof(AnchorManager), nameof(CheckPortalAnchor), anchor.name); //DEBUG
        			return true;
				}
			}
			
			DebugManager.LogFormat(nameof(AnchorManager), nameof(CheckPortalAnchor), _name, "NOT FOUND"); //DEBUG
			return false;
        }
        
        // -------------------------------------------------------------------------------
    	// GetPortalAnchorPosition
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public Vector3 GetPortalAnchorPosition(string _name)
        {
        
        	foreach (PortalAnchorEntry anchor in portalAnchors)
        	{
        		if (anchor.name == _name)
				{
					DebugManager.LogFormat(nameof(AnchorManager), nameof(GetPortalAnchorPosition), anchor.name); //DEBUG
					return anchor.position;
				}
			}
			
			DebugManager.LogFormat(nameof(AnchorManager), nameof(GetPortalAnchorPosition), _name, "NOT FOUND"); //DEBUG
			return Vector3.zero;
        }
        
        // -------------------------------------------------------------------------------
    	// RegisterPortalAnchor
    	// we are not using a game object or component here as this won't work across scenes!
    	// @Client / @Server
    	// -------------------------------------------------------------------------------
        public void RegisterPortalAnchor(string _name, Vector3 _position)
        {
            portalAnchors.Add(
            				new PortalAnchorEntry
            				{
            					name = _name,
            					position = _position
            				}
            );
            
            DebugManager.LogFormat(nameof(AnchorManager), nameof(RegisterPortalAnchor), _name); //DEBUG
        }

        // -------------------------------------------------------------------------------
        // UnRegisterPortalAnchor
        // @Client / @Server
        // -------------------------------------------------------------------------------
        public void UnRegisterPortalAnchor(string _name)
        {
           for (int i = portalAnchors.Count-1; i >= 0; i--)
           {
           		if (portalAnchors[i].name == _name)
           		{
           			portalAnchors.RemoveAt(i);
            		DebugManager.LogFormat(nameof(AnchorManager), nameof(UnRegisterPortalAnchor), _name); //DEBUG
            	}
            }
        }
       
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================