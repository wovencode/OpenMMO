
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// BaseCache
	// ===================================================================================
	public abstract partial class BaseCache
	{
		
		protected double cacheUpdateInterval = 1f;
		protected double _timerCache = 0;
		
		// -------------------------------------------------------------------------------
		// BaseCache (Constructor)
		// -------------------------------------------------------------------------------
		public BaseCache(double _cacheUpdateInterval)
		{
			cacheUpdateInterval = _cacheUpdateInterval;
		}
		
	}

}

// =======================================================================================