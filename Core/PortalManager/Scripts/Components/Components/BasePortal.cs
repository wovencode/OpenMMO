
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.UI;
using OpenMMO.DebugManager;

namespace OpenMMO.Portals
{

	// ===================================================================================
	// BasePortal
	// ===================================================================================
	public abstract partial class BasePortal : MonoBehaviour
	{
		
		[Header("Options")]
		[Tooltip("Trigger automatically on enter?")]
		public bool triggerOnEnter;
		
		[Header("System Texts")]
		public string popupEnter 	= "Enter {0}?";
		public string popupWait 	= "Please wait {0} seconds!";
		
		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public abstract void OnTriggerEnter(Collider co);
		
		// -------------------------------------------------------------------------------
		// OnClickConfirm
		// @Client
		// -------------------------------------------------------------------------------
		public abstract void OnClickConfirm();
		
		
		// -------------------------------------------------------------------------------
		// OnTriggerExit
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public virtual void OnTriggerExit(Collider co)
		{
		
			GameObject player = PlayerComponent.localPlayer;
						
			if (player)
				UIPopupPrompt.singleton.Hide();
				
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================