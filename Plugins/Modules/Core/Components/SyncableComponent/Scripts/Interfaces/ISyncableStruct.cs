// =======================================================================================
// ISyncableStruct
// by Weaver (Fhiz)
// MIT licensed
//
// A simple interface shared by all SyncStructs
//
// =======================================================================================

using UnityEngine;
using OpenMMO;

namespace OpenMMO
{
	// ===================================================================================
	// 
	// ===================================================================================
	public partial interface ISyncableStruct<T>
	{

		T template { get; }
		
		void Update(GameObject player);
		bool Valid { get; }
		
		bool CanAdd(long amount=1);
		void Add(long amount=1);
		
		bool CanRemove(long amount=1);
		void Remove(long amount=1);
		
		int level { get; }
		
	}
		
}

// =======================================================================================