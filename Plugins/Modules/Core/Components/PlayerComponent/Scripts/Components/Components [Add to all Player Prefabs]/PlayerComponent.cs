//by Fhiz
using UnityEngine;
using Mirror;
using OpenMMO.Database;

namespace OpenMMO {
	
	/// <summary>
	/// Partial class PlayerComponent, derived from EntityComponent. Base class for all Players.
	/// </summary>
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class PlayerComponent : EntityComponent
	{
	
		// holds exact replica of table data as in database
		// no need to sync, can be done individually if required
		public TablePlayer tablePlayer 				= new TablePlayer();
		public TablePlayerZones tablePlayerZones 	= new TablePlayerZones();
		
		/// <summary>
		/// Server side start
		/// </summary>
		[ServerCallback]
		protected override void Start()
    	{
        	base.Start(); // required
		}
		
		/// <summary>
		/// Called when the local player enters the game.
		/// </summary>
		public override void OnStartLocalPlayer()
    	{
    		base.OnStartLocalPlayer(); // required
		}
		
		/// <summary>
		/// Called client and server-side, when the player object is destroyed
		/// </summary>
		void OnDestroy()
    	{
    	
        }
		
		/// <summary>
		/// Server side, throttled update
		/// </summary>
		[Server]
		protected override void UpdateServer()
		{
			base.UpdateServer();
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK
		}
		
		/// <summary>
		/// 
		/// </summary>
		[Client]
		protected override void UpdateClient()
		{
			base.UpdateClient();
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
		}
		
		/// <summary>
		/// Client-based late update
		/// </summary>
		[Client]
		protected override void LateUpdateClient()
		{
			this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient)); //HOOK
		}
		
		//// <summary>
		/// Client-based fixed update
		/// </summary>
		[Client]
		protected override void FixedUpdateClient()
		{
			
		}
		
	}

}