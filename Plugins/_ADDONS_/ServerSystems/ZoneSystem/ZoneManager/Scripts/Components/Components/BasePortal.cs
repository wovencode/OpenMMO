//By Fhiz
//MODIFIED BY DX4D
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
using OpenMMO.Debugging;
//using OpenMMO.Chat; //DEPRECIATED - Creates a Depencency With the Chat - This completely breaks the potential modularity of the Chat Plugin.

namespace OpenMMO.Zones
{

	// ===================================================================================
	// BasePortal
	// ===================================================================================
	public abstract partial class BasePortal : MonoBehaviour
	{
		
		[Header("Options")]
		[Tooltip("Skip the confirmation popup and teleport right away?")]
		public bool bypassConfirmation;
		
		[Header("System Texts")]
		public string popupEnter 	= "Enter {0}?";
		public string popupWait 	= "Please wait {0} seconds!";
		public string popupClosed	= "This portal is offline.";
		public string infoEntered	= "You stepped into a warp portal.";
		
		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public abstract void OnTriggerEnter(Collider co);

		// -------------------------------------------------------------------------------
		// OnClickConfirm
		// @Client
		// -------------------------------------------------------------------------------
		public virtual void OnClickConfirm() {}

        // -------------------------------------------------------------------------------
        // OnTriggerExit
        // @Client / @Server
        // -------------------------------------------------------------------------------
        public virtual void OnTriggerExit(Collider co)
		{
			
			PlayerAccount pc = co.GetComponentInParent<PlayerAccount>();
			
			if (pc == null || !pc.IsLocalPlayer)
				return;
			
			UIPopupPrompt.singleton.Hide();
				
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================