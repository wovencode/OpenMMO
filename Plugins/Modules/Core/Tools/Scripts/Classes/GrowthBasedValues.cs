
using OpenMMO;
using System;
using UnityEngine;

namespace OpenMMO
{

	[Serializable]
	public partial struct LinearGrowthInt
	{
		public int baseValue;
		public int bonusPerLevel;
		public int Get(int level)
		{
			return Math.Max(0, bonusPerLevel * (level - 1)) + baseValue;
		}
		
	}

	[Serializable]
	public partial struct LinearGrowthLong
	{
		public long baseValue;
		public long bonusPerLevel;
		public long Get(int level)
		{
			return Math.Max(0, bonusPerLevel * (level - 1)) + baseValue;
		}
	}

	[Serializable]
	public partial struct LinearGrowthFloat
	{
		public float baseValue;
		public float bonusPerLevel;
		public float Get(int level)
		{
			return Mathf.Max(0, bonusPerLevel * (level - 1)) + baseValue;
		}
		
	}

	[Serializable]
	public partial struct ExponentialGrowthInt
	{
		public int multiplier;
		public float baseValue;
		public int Get(int level)
		{
			return Convert.ToInt32(multiplier * Mathf.Pow(baseValue, (level - 1)));
		}
		
	}

	[Serializable]
	public partial struct ExponentialGrowthLong
	{
		public long multiplier;
		public float baseValue;
		public long Get(int level)
		{
			return Convert.ToInt64(multiplier * Mathf.Pow(baseValue, (level - 1)));
		}
		
	}

	[Serializable]
	public partial struct ExponentialGrowthFloat
	{
		public float multiplier;
		public float baseValue;
		public float Get(int level)
		{
			return multiplier * Mathf.Pow(baseValue, (level - 1));
		}
		
	}

}

// =======================================================================================