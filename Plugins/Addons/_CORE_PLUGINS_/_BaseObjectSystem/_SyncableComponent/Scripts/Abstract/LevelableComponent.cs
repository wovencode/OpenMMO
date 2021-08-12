//by Fhiz
//MODIFIED BY DX4D
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
		
		///// <summary> Server-side Start for initializiation. </summary>
		//[ServerCallback] protected override void Start() { base.Start(); }

		//CLIENT
		/// <summary> [Client] start. </summary>
		[Client] protected override void StartClient() {}
		/// <summary> [Client] throttled update. </summary>
		[Client] protected override void UpdateClient() {}
		//SERVER
		/// <summary> [Server] start. </summary>
		[Server] protected override void StartServer() {}
		/// <summary> [Server] throttled update. </summary>
		[Server] protected override void UpdateServer() {}
		
	}

}