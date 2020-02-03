
using System;
using System.Text;
using UnityEngine;
using Mirror;
using OpenMMO;

namespace OpenMMO {

	// ===================================================================================
	// SyncableComponent
	// ===================================================================================
	[System.Serializable]
	public abstract partial class SyncableComponent : NetworkBehaviour
	{
	
		[Header("Caching")]
		[Tooltip("How often the manager itself is updated (and all of its data, in seconds)")]
		[Range(0.01f, 99)]
		public double managerUpdateInterval = 1f;
		[Tooltip("How long cached data is kept (in seconds) before its re-calculated")]
		[Range(0.01f, 99)]
		public double cacheUpdateInterval = 1f;
		
		double _timerManager = 0;
		
		protected DataCache cacheData;
		protected string _name;
		
		public static GameObject localPlayer => ClientScene.localPlayer != null ? ClientScene.localPlayer.gameObject : null;
		
		// -------------------------------------------------------------------------------
		// Start
		// @Server
		// -------------------------------------------------------------------------------
		[ServerCallback]
		protected virtual void Start() {
			cacheData = new DataCache(cacheUpdateInterval);
		}
		
		// -------------------------------------------------------------------------------
		// name
		// -------------------------------------------------------------------------------
		public new string name
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_name))
					_name = base.name;
				return _name;
			}
			set
			{
				_name = base.name = value;
			}
		}
		
		// -------------------------------------------------------------------------------
		// CheckUpdateInterval
		// Used to throttle calls to "Update" (similar to how we do it in "Wovencore UI")
		// -------------------------------------------------------------------------------
		protected bool CheckUpdateInterval => Time.time > _timerManager;
		
		// -------------------------------------------------------------------------------
		// RefreshUpdateInterval
		// updates the cache timer interval
		// -------------------------------------------------------------------------------
		void RefreshUpdateInterval()
		{
			_timerManager = Time.time + managerUpdateInterval;
		}
		
		// -------------------------------------------------------------------------------
		// Update
		// updated every frame, private to enforce the use of UpdateServer/UpdateClient
		// -------------------------------------------------------------------------------
		void Update()
		{
			if (CheckUpdateInterval)
			{
				if (isClient)
					UpdateClient();
				if (isServer)
					UpdateServer();
				
				RefreshUpdateInterval();
			}
		}
		
		// -------------------------------------------------------------------------------
		// LateUpdate
		// updated every frame, private to enforce the use of LateUpdateClient
		// -------------------------------------------------------------------------------
		void LateUpdate()
		{
			if (isClient)
				LateUpdateClient();
		}
		
		// -------------------------------------------------------------------------------
		// UpdateServer
		// @Server
		// -------------------------------------------------------------------------------
		[Server]
		protected abstract void UpdateServer();
		
		// -------------------------------------------------------------------------------
		// UpdateClient
		// @Client
		// -------------------------------------------------------------------------------
		[Client]
		protected abstract void UpdateClient();
		
		// -------------------------------------------------------------------------------
		// LateUpdateClient
		// @Client
		// -------------------------------------------------------------------------------
		[Client]
		protected abstract void LateUpdateClient();
		
		// -------------------------------------------------------------------------------
			
	}

}

// =======================================================================================