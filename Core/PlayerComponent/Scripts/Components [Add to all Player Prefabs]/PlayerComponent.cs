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
using Wovencode;
using Wovencode.Database;
using UnityEngine.AI;

namespace Wovencode {
	
	// ===================================================================================
	// PlayerComponent
	// ===================================================================================
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class PlayerComponent : EntityComponent
	{
	
		// holds exact replica of table data as in database
		// no need to sync, can be done individually if required
		public TablePlayer tablePlayer = new TablePlayer();
		
		Camera mainCamera;
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		protected override void Start()
    	{
        	base.Start(); // required
		}
		
		// -------------------------------------------------------------------------------
		// OnStartLocalPlayer
		// -------------------------------------------------------------------------------
		public override void OnStartLocalPlayer()
    	{
    		base.OnStartLocalPlayer();
    		
        	mainCamera = Camera.main;
        	mainCamera.GetComponent<FollowCameraControls>().target = this.transform;
        	mainCamera.GetComponent<FollowCameraControls>().enabled = true;
		}
		
		// -------------------------------------------------------------------------------
		// 
		// -------------------------------------------------------------------------------
		void OnDestroy()
    	{
    	
        }
		
		// -------------------------------------------------------------------------------
		// UpdateServer
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			base.UpdateServer();
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer));
		}
		
		// -------------------------------------------------------------------------------
		// UpdateClient
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			base.UpdateClient();
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient));
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================