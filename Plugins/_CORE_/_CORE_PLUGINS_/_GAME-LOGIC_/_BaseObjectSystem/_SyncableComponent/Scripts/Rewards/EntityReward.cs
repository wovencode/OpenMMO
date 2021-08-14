//by Fhiz
using System;
using System.Text;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Abstract partial Entity Reward comprises all kinds of rewards that feature a level (pets, mounts etc.)
	/// </summary>
	[System.Serializable]
	public abstract partial class EntityReward : BaseReward
	{
		
		[SerializeField]protected int minLevel;
		[SerializeField]protected int maxLevel;
	
		public int GetLevel
		{
			get
			{
				if (maxLevel <= 0) return minLevel;
				return UnityEngine.Random.Range(minLevel, maxLevel);
			}
		}
	
	}

}