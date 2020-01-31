
using System;
using System.Text;
using UnityEngine;
using Mirror;
using Wovencode;

namespace Wovencode {

	// ===================================================================================
	// UpgradableComponent
	// ===================================================================================
	[System.Serializable]
	public abstract partial class UpgradableComponent : SyncableComponent
	{
	
		[Header("Level")]
		[SyncVar]
		public int level = 1;
		public int maxLevel = 3;
		public LinearGrowthInt capacity = new LinearGrowthInt{baseValue=99, bonusPerLevel=0};
#if wCURRENCY
		public LevelCurrencyCost[] upgradeCost;
#endif
		
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
		public bool CanUpgradeLevel()
		{
			return (level < maxLevel
#if wCURRENCY
					&& GetComponentInParent<PlayerCurrencyComponent>().CanPayCost(upgradeCost, level)
#endif
					);
		}
		
		// -------------------------------------------------------------------------------
		[Command]
		public void CmdUpgradeLevel()
		{
			if (CanUpgradeLevel())
				UpgradeLevel();
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected virtual void UpgradeLevel()
		{
#if wCURRENCY
			GetComponentInParent<PlayerCurrencyComponent>().PayCost(upgradeCost, level);
#endif
			level++;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================