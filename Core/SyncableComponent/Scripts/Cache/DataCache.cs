
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// DataCache
	// ===================================================================================
	public partial class DataCache : BaseCache
	{
	
		protected Dictionary<int, DataCacheEntry> cacheEntries = new Dictionary<int, DataCacheEntry>();
		
		// -------------------------------------------------------------------------------
		// DataCache (Constructor) 
		// -------------------------------------------------------------------------------
		public DataCache(double _cacheUpdateInterval) : base(_cacheUpdateInterval)
		{
		
		}
	
	}

}

// =======================================================================================