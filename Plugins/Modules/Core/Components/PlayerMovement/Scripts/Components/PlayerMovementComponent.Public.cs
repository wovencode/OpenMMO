//by Fhiz
using System;
using UnityEngine;
using UnityEngine.AI;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// This partial section of the PlayerMovementComponent is exposed to public and used to check various movement related states.
	/// </summary>
	public partial class PlayerMovementComponent
	{
	
		/// <summary>
		/// Checks if the player is currently walking
		/// </summary>
		public bool GetIsWalking
		{
			get
			{
				return GetIsMoving && !running;
			}
		}
		
		/// <summary>
		/// Checks if the player is currently running
		/// </summary>
		public bool GetIsRunning
		{
			get
			{
				return GetIsMoving && running;
			}
		}
	
	
	}

}