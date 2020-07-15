//by Fhiz
using UnityEngine;
using OpenMMO;

namespace OpenMMO
{

	/// <summary>
	/// A simple, partial interface shared by all ISyncableStructs. Those structs are data-holders and part of a sync-list that rests on a SyncableComponent.
	/// </summary>
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