
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// PlayerMovement
	// ===================================================================================
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class PlayerMovementComponent : EntityMovementComponent
	{
		
		[Header("Movement Factors")]
        public float rotationSpeed = 100;
		[Range(0,100)]public float walkFactor 		= 1.0f;
		[Range(0,100)]public float runFactor 		= 1.5f;
		[Range(0,100)]public float backwardFactor 	= 0.5f;
		
		[Header("Input")]
		public KeyCode runKey = KeyCode.LeftShift;
		
		protected float verticalMovementInput;
		protected float horizontalMovementInput;
		protected bool 	running;
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		protected override void Start()
    	{
    		agent.updateRotation = false;
        	base.Start();
		}
		
		// -------------------------------------------------------------------------------
		// 
		// -------------------------------------------------------------------------------
		public override void OnStartLocalPlayer()
    	{
		}
		
		// -------------------------------------------------------------------------------
		// 
		// -------------------------------------------------------------------------------
		void OnDestroy()
    	{
        }
		
		// -------------------------------------------------------------------------------
		// UpdateServer
		// @Server
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			base.UpdateServer();
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer));
		}
		
		// -------------------------------------------------------------------------------
		// UpdateClient
		// @Client
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			
            if (!isLocalPlayer) return;
            if (Tools.AnyInputFocused) return;
            
            horizontalMovementInput 	= Input.GetAxis("Horizontal");
            verticalMovementInput 		= Input.GetAxis("Vertical");
            running						= Input.GetKey(runKey);
            
            UpdateVelocity();
           
			base.UpdateClient();
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient));

		}
		
		// -------------------------------------------------------------------------------
		// LateUpdateClient
		// @Client
		// -------------------------------------------------------------------------------
		protected override void LateUpdateClient()
		{
			base.LateUpdateClient();		
			this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient));
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================