//by Fhiz
//MODIFIED BY DX4D
using UnityEngine;
using Mirror;
using OpenMMO.Database;

namespace OpenMMO {
	
	/// <summary>
	/// Partial class PlayerAccount, derived from MobileComponent. Base class for all Players.
	/// </summary>
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class PlayerAccount : MobileComponent
	{
        //ACCOUNT INFO
		public TablePlayer _tablePlayer = new TablePlayer();
        /// <summary>
		/// holds exact replica of table data as in database
		/// no need to sync, can be done individually if required
        /// </summary>
        public TablePlayer tablePlayer { get { return _tablePlayer; } }
        public TablePlayer accountInfo { get { return _tablePlayer; } }
        
        //ZONE INFO
		[SerializeField] TablePlayerZones _tablePlayerZones = new TablePlayerZones();
		public TablePlayerZones tablePlayerZones
        {
            get { return _tablePlayerZones; }
            set { _tablePlayerZones = value; }
        }
		public TablePlayerZones zoneInfo
        {
            get { return _tablePlayerZones; }
            set { _tablePlayerZones = value; }
        }

        /// <summary> Server side start </summary>
        //[ServerCallback] protected override void Start() { base.Start(); } // required

        /// <summary>
        /// Called when the local player enters the game.
        /// </summary>
        public override void OnStartLocalPlayer() { base.OnStartLocalPlayer(); } // required
		
		/// <summary>
		/// Called client and server-side, when the player object is destroyed
		/// </summary>
        //[Client][Server]
		void OnDestroy()
    	{
    	
        }

        // S E R V E R

        /// <summary> [Server] start method </summary>
        [Server] protected override void StartServer() { base.StartServer(); }
		/// <summary> [Server] throttled update </summary>
		[Server] protected override void UpdateServer()
		{
			base.UpdateServer();
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK //REMOVE - CALLED IN BASE - DX4D
		}
		

        // C L I E N T

        /// <summary> [Client] start method </summary>
        [Client] protected override void StartClient() { base.StartClient(); }
		/// <summary> [Client] Update </summary>
		[Client] protected override void UpdateClient()
		{
			base.UpdateClient();
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
		}
		
		/// <summary> Client-based late update </summary>
		[Client] protected override void LateUpdateClient()
		{
			this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient)); //HOOK
		}

        //// <summary> Client-based fixed update </summary>
        [Client] protected override void FixedUpdateClient() { }
		
	}

}