
using System;
using UnityEngine;
using OpenMMO;
using OpenMMO.Network;
using Mirror;

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// NetworkStartPosition
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class NetworkStartPosition : Mirror.NetworkStartPosition
	{
		[Tooltip("Add any number of archetypes")]
		public ArchetypeTemplate[] archeTypes;
	}
	
	// -----------------------------------------------------------------------------------
	
}