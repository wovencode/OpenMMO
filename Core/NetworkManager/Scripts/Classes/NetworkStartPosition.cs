
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
    /// <summary>
    /// Public partial class <c>NetworkStartPOsition</c> inherits from Mirror.NetworkStatPosition
    /// Is used to mark a player spawn point. 
    /// Contains an array of character archetypes. 
    /// </summary>
	[DisallowMultipleComponent]
	public partial class NetworkStartPosition : Mirror.NetworkStartPosition
	{
		[Tooltip("Add any number of archetypes")]
		public ArchetypeTemplate[] archeTypes;
	}
	
	// -----------------------------------------------------------------------------------
	
}