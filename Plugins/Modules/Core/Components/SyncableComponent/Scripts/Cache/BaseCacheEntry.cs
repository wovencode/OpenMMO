
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// BaseCacheEntry
	// ===================================================================================
	public abstract partial class BaseCacheEntry
	{
		
		public 		double 	timer;
		protected 	double 	interval;
		
		// -------------------------------------------------------------------------------
		// CheckUpdateInterval
		// -------------------------------------------------------------------------------
		public bool CheckUpdateInterval(double _interval)
		{
			if (interval == 0)
				interval = _interval;
			return Time.time > timer;
		}
		
		// -------------------------------------------------------------------------------
		// RefreshUpdateInterval
		// -------------------------------------------------------------------------------
		public void RefreshUpdateInterval()
		{
			timer = Time.time + interval;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================