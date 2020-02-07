
using System;
using System.Text;
using UnityEngine;
using Mirror;
using OpenMMO;

namespace OpenMMO {

	// ===================================================================================
	// LevelableComponent
	// ===================================================================================
	[System.Serializable]
	public abstract partial class LevelableComponent : SyncableComponent
	{
	
		[Header("Level")]
		[SyncVar]
		public int level = 1;
		public int maxLevel = 3;
		
#if oCURRENCY
		public LevelCurrencyCost[] upgradeCost;
#endif
		
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
#if _CURRENCY
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
#if _CURRENCY
			GetComponentInParent<PlayerCurrencyComponent>().PayCost(upgradeCost, level);
#endif
			level++;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================