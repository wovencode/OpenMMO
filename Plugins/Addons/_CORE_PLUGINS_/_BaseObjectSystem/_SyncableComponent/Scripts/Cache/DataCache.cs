//by Fhiz
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	/// <summary>
	/// Partial DataCache is based on the BaseCache and features a Dictionary of DataCacheEntries.
	/// </summary>
	public partial class DataCache : BaseCache
	{
	
		protected Dictionary<int, DataCacheEntry> cacheEntries = new Dictionary<int, DataCacheEntry>();
		
		/// <summary>
		/// Constructor that also calls the base class Constructor
		/// </summary>
		public DataCache(double _cacheUpdateInterval) : base(_cacheUpdateInterval)
		{
		
		}
	
	}

}