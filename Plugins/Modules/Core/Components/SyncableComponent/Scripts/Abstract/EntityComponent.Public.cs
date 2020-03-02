//by Fhiz
using System;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Debugging;

namespace OpenMMO {
	
	/// <summary>
	/// This partial section of Entity Component is public and exposed to other parts of the code.
	/// </summary>
	public abstract partial class EntityComponent
	{
	
		/// <summary>
		/// Server-side Warp method to teleport the entity to any location on the current scene.
		/// </summary>
		[ServerCallback]
		public virtual void Warp(Vector3 position)
    	{
    		agent.Warp(position);
    		RpcWarp(position);
    		DebugManager.LogFormat(this.name, nameof(Warp), position.ToString()); //DEBUG
		}
		
		/// <summary>
		/// Broadcasts the updated position to all nearby clients.
		/// </summary>
		[ClientRpc]
   		protected void RpcWarp(Vector3 position)
    	{
        	agent.Warp(position);
    	}
		
	}

}