//by Fhiz
using System;
using System.Text;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Abstract partial Property Reward is the base class for all rewards that feature an amount (most often Currencies)
	/// </summary>
	[System.Serializable]
	public abstract partial class PropertyReward : BaseReward
	{
		
		[SerializeField]protected int minAmount;
		[SerializeField]protected int maxAmount;
		
		/// <summary>
		/// Returns the amount of reward added.
		/// </summary>
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