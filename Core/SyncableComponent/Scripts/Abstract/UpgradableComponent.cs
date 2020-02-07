
using System;
using System.Text;
using UnityEngine;
using Mirror;
using OpenMMO;

namespace OpenMMO {

	// ===================================================================================
	// UpgradableComponent
	// ===================================================================================
	[System.Serializable]
	public abstract partial class UpgradableComponent : LevelableComponent
	{
	
		public LinearGrowthInt capacity = new LinearGrowthInt{baseValue=99, bonusPerLevel=0};
		
		// -------------------------------------------------------------------------------
		// GetCapacity
		// How many syncData entries can this component hold currently? That could be
		// inventory items, pieces of equipment, currencies, attributes etc.
		// -------------------------------------------------------------------------------
		public int GetCapacity => capacity.Get(level);
		
		// -------------------------------------------------------------------------------
		// Start
		// @Server
		// -------------------------------------------------------------------------------
		[ServerCallback]
		protected override void Start() {
			base.Start();
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer() {}
		
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient() {}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================