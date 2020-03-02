//by Fhiz
using System;
using UnityEngine;
using Mirror;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// This partial section of the player component takes care of the global cooldown (used to limit 'Risky Actions')
	/// </summary>
	public partial class PlayerComponent
	{

		[Header("Base cooldown for 'risky actions' (in seconds)")]
		[Range(1,999)]public double baseCooldown;
		
		// a global player cooldown to handle "risky actions"
		[SyncVar] double cooldown = 0;
		
		/// <summary>
		/// 
		/// </summary>
		public bool CheckCooldown
    	{
    		get
    		{
        		return NetworkTime.time >= cooldown;
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// Sent from CLIENT to SERVER.
		/// </remarks>
		[Command]
		public void Cmd_UpdateCooldown(float extraCooldown)
		{
			UpdateCooldown(extraCooldown);
		}
		
		/// <summary>
		/// Updates the cooldown timer on the server.
		/// </summary>
		/// <remarks>
		/// Sent from CLIENT to SERVER.
		/// </remarks>
		[Server]
		protected void UpdateCooldown(float extraCooldown)
		{
			cooldown = NetworkTime.time + (float)(baseCooldown + Mathf.Abs(extraCooldown));
			tablePlayer.cooldown = GetCooldownTimeRemaining();
		}
		
		/// <summary>
		/// Returns the remaining cooldown (until another Risky Action is allowed).
		/// </summary>
		public float GetCooldownTimeRemaining()
		{
			return CheckCooldown ? 0 : (float)(cooldown - NetworkTime.time);
		}
		
	}

}