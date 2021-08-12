//by Fhiz
using System;
using System.Text;
using UnityEngine;
using Mirror;
using OpenMMO;

namespace OpenMMO {

	/// <summary>
	/// Upgradable components are based on LevelableComponents and can be upgraded by the player. That could be a inventory or any other storage of any kind.
	/// </summary>
	[System.Serializable]
	public abstract partial class UpgradableComponent : LevelableComponent
	{
	
		public LinearGrowthInt capacity = new LinearGrowthInt{baseValue=99, bonusPerLevel=0};
		
		/// <summary>
		/// How many syncData entries can this component hold currently? That could be inventory items, pieces of equipment, currencies, attributes etc.
		/// </summary>
		public int GetCapacity => capacity.Get(level);
		
		///// <summary> Server-side Start calls base.Start(); </summary>
		//[ServerCallback] protected override void Start() { base.Start(); }
		
		///// <summary> Server-side throttled update. </summary>
		//[Server] protected override void UpdateServer() {}
		
		///// <summary> Client-side throttled update. </summary>
		//[Client] protected override void UpdateClient() {}
		
	}

}