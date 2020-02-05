// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
//
// =======================================================================================

using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Database;
using UnityEngine.AI;

namespace OpenMMO {
	
	// ===================================================================================
	// PlayerComponent
	// ===================================================================================
	public partial class PlayerComponent
	{

		[Header("Base cooldown for 'risky actions' (in seconds)")]
		[Range(1,999)]public double baseCooldown;
		
		// a global player cooldown to handle "risky actions"
		[SyncVar] double cooldown = 0;
		
		// -------------------------------------------------------------------------------
		// CheckCooldown
		// -------------------------------------------------------------------------------
		public bool CheckCooldown
    	{
    		get
    		{
        		return NetworkTime.time >= cooldown;
			}
		}
		
		// -------------------------------------------------------------------------------
		// Cmd_UpdateCooldown
		// @Client -> @Server
		// -------------------------------------------------------------------------------
		[Command]
		public void Cmd_UpdateCooldown(float extraCooldown)
		{
			UpdateCooldown(extraCooldown);
		}
		
		// -------------------------------------------------------------------------------
		// UpdateCooldown
		// @Server
		// -------------------------------------------------------------------------------
		[Server]
		protected void UpdateCooldown(float extraCooldown)
		{
			cooldown = NetworkTime.time + (float)(baseCooldown + Mathf.Abs(extraCooldown));
			tablePlayer.cooldown = GetCooldownTimeRemaining();
		}
		
		// -------------------------------------------------------------------------------
		// GetCooldownTimeRemaining
		// -------------------------------------------------------------------------------
		public float GetCooldownTimeRemaining()
		{
			return CheckCooldown ? 0 : (float)(cooldown - NetworkTime.time);
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================