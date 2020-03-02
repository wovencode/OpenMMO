//by Fhiz
using System;
using System.Text;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {

	/// <summary>
	/// Abstract partial class BaseReward. Base class for all kinds of "Rewards" that can be handed out to a player (Currencies, Items etc.).
	/// </summary>
	[System.Serializable]
	public abstract partial class BaseReward
	{
		
		[HideInInspector]public string title;
		[HideInInspector]public long timer = 0;
	
	}
	
}