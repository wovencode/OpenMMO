
using System;
using UnityEngine;
using OpenMMO;

namespace OpenMMO
{
	
	// ===================================================================================
	// GameRulesTemplate
	// ===================================================================================
	public partial class GameRulesTemplate
	{
	
		[Header("Player/User Settings")]
		[Tooltip("How many characters can one user create on one account?")]
		public int maxPlayersPerUser = 4;
		[Tooltip("How many accounts can be created on one device (helps fight twinks)?")]
		public int maxUsersPerDevice = 4;
		[Tooltip("How many accounts can be created on one email (helps fight twinks)?")]
		public int maxUsersPerEmail = 4;
		
	}

}

// =======================================================================================
