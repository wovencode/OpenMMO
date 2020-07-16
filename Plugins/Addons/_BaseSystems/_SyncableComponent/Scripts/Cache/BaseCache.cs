//by Fhiz
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Abstract partial BaseCache. A simple base class for all kind of caches, provides only the update interval and the cache timer.
	/// </summary>
	/// <remarks>
	/// Interval is set to once-per-second as this seems a good number to work with.
	/// </remarks>
	public abstract partial class BaseCache
	{
		
		protected double cacheUpdateInterval = 1f;
		protected double _timerCache = 0;
		
		/// <summary>
		/// Constructor allows to set a custom interval for updating.
		/// </summary>
		public BaseCache(double _cacheUpdateInterval)
		{
			cacheUpdateInterval = _cacheUpdateInterval;
		}
		
	}

}