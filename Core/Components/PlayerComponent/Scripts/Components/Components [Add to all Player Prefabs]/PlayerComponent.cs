
//using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
using Mirror;
//using OpenMMO;
using OpenMMO.Database;
//using UnityEngine.AI;

namespace OpenMMO {
	
	// ===================================================================================
	// PlayerComponent
	// ===================================================================================
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class PlayerComponent : EntityComponent
	{
	
		// holds exact replica of table data as in database
		// no need to sync, can be done individually if required
		public TablePlayer tablePlayer 				= new TablePlayer();
		public TablePlayerZones tablePlayerZones 	= new TablePlayerZones();
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		[ServerCallback]
		protected override void Start()
    	{
        	base.Start(); // required
		}
		
		// -------------------------------------------------------------------------------
		// OnStartLocalPlayer
		// -------------------------------------------------------------------------------
		public override void OnStartLocalPlayer()
    	{
    		base.OnStartLocalPlayer(); // required
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
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// UpdateClient
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			base.UpdateClient();
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// LateUpdateClient
		// -------------------------------------------------------------------------------
		[Client]
		protected override void LateUpdateClient()
		{
			this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient)); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// FixedClient
		// @Client
		// -------------------------------------------------------------------------------
		[Client]
		protected override void FixedUpdateClient()
		{
			
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================