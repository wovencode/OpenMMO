//BY FHIZ
//MODIFIED BY DX4D

using System;
using UnityEngine;
using OpenMMO.UI;
using OpenMMO.Debugging;

namespace OpenMMO.Zones
{

	// ===================================================================================
	// MultiWarpPortal
	// ===================================================================================
	[DisallowMultipleComponent]
	public class MultiTeleporter : MultiTargetActionTrigger
	{
	
		//[Header("TELEPORT TO RANDOM MARKER")]
		//[Tooltip("The Location Markers in the same scene to teleport to")]
		//public string[] targetNames;
		
		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public override void OnTriggerEnter(Collider co)
		{
			
			if (targetNames == null || targetNames.Length == 0)
			{
				DebugManager.LogWarning("You must add location markers to portal: " + this.name);
				return;
			}
			
			PlayerAccount pc = co.GetComponentInParent<PlayerAccount>();
			
			if (pc == null || !pc.IsLocalPlayer)
				return;
			
			if (!action.bypassConfirmation)
			{
				if (pc.CheckCooldown)
					UIPopupPrompt.singleton.Init(action.popupEnter, OnClickConfirm);
				else
					UIPopupNotify.singleton.Init(String.Format(action.popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
			}
			else
				OnClickConfirm();
					
		}
		
		// -------------------------------------------------------------------------------
		// OnClickConfirm
		// @Client
		// -------------------------------------------------------------------------------
		public override void OnClickConfirm()
		{
			
			GameObject player = PlayerAccount.localPlayer;
			
			if (player == null)
				return;
			
			PlayerAccount pc = player.GetComponent<PlayerAccount>();
			
			int index = UnityEngine.Random.Range(0, targetNames.Length);
			string targetAnchor = targetNames[index];

			if (player != null && !String.IsNullOrWhiteSpace(targetAnchor) && pc.CheckCooldown)
				pc.Cmd_WarpLocal(targetAnchor);
			
			base.OnClickConfirm();
			
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================