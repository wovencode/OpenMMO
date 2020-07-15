//by Fhiz
using UnityEngine;
using Mirror;
using OpenMMO.Database;

namespace OpenMMO
{
    // This partial section of PlayerAccount adds a reference to PlayerMovement component to it.
    public partial class PlayerAccount
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