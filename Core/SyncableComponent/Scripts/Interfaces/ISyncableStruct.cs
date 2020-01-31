// =======================================================================================
// ISyncableStruct
// by Weaver (Fhiz)
// MIT licensed
//
// A simple interface shared by all SyncStructs
//
// =======================================================================================

using UnityEngine;
using Wovencode;

namespace Wovencode
{
	// ===================================================================================
	// 
	// ===================================================================================
	public partial interface ISyncableStruct<T>
	{

		T template { get; }
		
		void Update(GameObject player);
		bool Valid { get; }
		
		bool CanRemove { get; }
		void Remove(long _amount=1);
		
		void Reset();
		
		int level { get; }
		
	}
		
}

// =======================================================================================