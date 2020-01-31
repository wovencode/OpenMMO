
using System;
using UnityEngine;
using Wovencode;
using Wovencode.Network;
using Mirror;

namespace Wovencode.Network
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