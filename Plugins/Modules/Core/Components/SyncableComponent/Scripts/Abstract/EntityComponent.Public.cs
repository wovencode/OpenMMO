
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Debugging;

namespace OpenMMO {
	
	// ===================================================================================
	// EntityComponent
	// ===================================================================================
	public abstract partial class EntityComponent
	{
		
		// -------------------------------------------------------------------------------
		// Warp
		// @Server
		// -------------------------------------------------------------------------------
		[ServerCallback]
		public virtual void Warp(Vector3 position)
    	{
    		agent.Warp(position);
    		RpcWarp(position);
    		DebugManager.LogFormat(this.name, nameof(Warp), position.ToString()); //DEBUG
		}
		
		// -------------------------------------------------------------------------------
		// RpcWarp
		// @Server -> @Clients
		// -------------------------------------------------------------------------------
		[ClientRpc]
   		public void RpcWarp(Vector3 position)
    	{
        	agent.Warp(position);
    	}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================