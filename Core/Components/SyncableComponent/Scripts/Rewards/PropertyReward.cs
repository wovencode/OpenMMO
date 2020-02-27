// =======================================================================================
// PropertyReward
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using System;
using System.Text;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// PropertyReward
	// ===================================================================================
	[System.Serializable]
	public abstract partial class PropertyReward : BaseReward
	{
		
		[SerializeField]protected int minAmount;
		[SerializeField]protected int maxAmount;
	
		public int GetAmount
		{
			get
			{
				if (maxAmount <= 0) return minAmount;
				return UnityEngine.Random.Range(minAmount, maxAmount);
			}
		}
	
	}

}

// =======================================================================================