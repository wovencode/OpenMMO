
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
using OpenMMO.Chat;

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
		public virtual void OnClickConfirm()
		{
			if (ChatManager.singleton)
				ChatManager.singleton.LocalChatSend(infoEntered);
		}
		
		// -------------------------------------------------------------------------------
		// OnTriggerExit
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public virtual void OnTriggerExit(Collider co)
		{
			
			PlayerComponent pc = co.GetComponentInParent<PlayerComponent>();
			
			if (pc == null || !pc.IsLocalPlayer)
				return;
			
			UIPopupPrompt.singleton.Hide();
				
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================