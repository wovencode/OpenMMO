//by Fhiz
using System;
using System.Text;
using UnityEngine;
using Mirror;
using OpenMMO;

namespace OpenMMO {

	/// <summary>
	/// Abstract partial Levelable Component is the base class for all SyncableComponents that feature a level. This could be the entity itself, but also inventories etc.
	/// </summary>
	[System.Serializable]
	public abstract partial class LevelableComponent : SyncableComponent
	{
	
		[Header("Level")]
		[SyncVar]
		public int level = 1;
		public int maxLevel = 3;
		
		/// <summary>
		/// Server-side Start for initializiation.
		/// </summary>
		[ServerCallback]
		protected override void Start() {
			base.Start();
		}
		
		/// <summary>
		/// Server-side, throttled update.
		/// </summary>
		[Server]
		protected override void UpdateServer() {}
		
		/// <summary>
		/// Client-side, throttled update.
		/// </summary>
		[Client]
		protected override void UpdateClient() {}
		
	}

}