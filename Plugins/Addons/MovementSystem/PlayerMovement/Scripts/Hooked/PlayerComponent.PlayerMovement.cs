//by Fhiz
using UnityEngine;
using Mirror;
using OpenMMO.Database;

namespace OpenMMO {
	
	/// <summary>
	/// This partial section of PlayerComponent adds a reference to PlayerMovement component to it.
	/// </summary>
	public partial class PlayerComponent
	{
	
		protected PlayerMovementComponent _playerMovementComponent;
		
		/// <summary>
		/// Returns the cached Player Movement Component or caches it first.
		/// </summary>
		public PlayerMovementComponent playerMovementComponent
		{
			get
			{
				if (!_playerMovementComponent)
					_playerMovementComponent = GetComponentInParent<PlayerMovementComponent>();
				return _playerMovementComponent;
			}
		}
		
	}

}