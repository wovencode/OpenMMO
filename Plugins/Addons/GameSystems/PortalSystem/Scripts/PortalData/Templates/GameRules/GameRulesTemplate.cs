//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;

namespace OpenMMO
{
	public partial class GameRulesTemplate
	{
		[Header("Teleportation Settings")]
		[Tooltip("Wait time between local scene teleportation (in seconds)")]
		public int localWarpDelay = 5;
		[Tooltip("Wait time between remote scene teleportation (in seconds)")]
		public int remoteWarpDelay = 10;
	}
}