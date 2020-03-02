//by Fhiz
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Abstract partial Base Cache Entry as the simplest form of a cache entry. Provides the timer and interval properties.
	/// </summary>
	public abstract partial class BaseCacheEntry
	{
		
		public 		double 	timer;
		protected 	double 	interval;
		
		/// <summary>
		/// Checks if enough time has passed to update the entry again (executing hooks and updating its properties).
		/// </summary>
		public bool CheckUpdateInterval(double _interval)
		{
			if (interval == 0)
				interval = _interval;
			return Time.time > timer;
		}
		
		/// <summary>
		/// Refreshes the update timer interval.
		/// </summary>
		public void RefreshUpdateInterval()
		{
			timer = Time.time + interval;
		}
		
	}

}