// =======================================================================================
// UIRoot
// by Weaver (Fhiz)
// MIT licensed
//
// based of Suriyun's UIBase class to provide a root class for all other UI elements.
// This class makes the Update function private and implements a ThrottledUpdate function 
// to be used instead. This way, all updates run at a throttled rate (that can be set
// via the inspector) instead of once per frame. The result is a major performance
// improvement, implemented right at the root of any UI element.
//
// =======================================================================================

using OpenMMO;
using OpenMMO.UI;
using UnityEngine;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIRoot
	// ===================================================================================
	public abstract partial class UIRoot : UIBase
	{
	
		[Header("UI Throttle")]
		public bool requiresActiveRoot = true;
		[Range(0.01f, 3f)]
		public float updateInterval = 0.25f;
		
		protected float fInterval = 0.01f;

		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			base.Awake();
		}
		
		// -------------------------------------------------------------------------------
		// Update is called every frame
		// -------------------------------------------------------------------------------
		protected virtual void Update()
		{
			if (updateInterval > 0 && Time.time > fInterval)
			{
				if (root.activeSelf || !requiresActiveRoot)
					ThrottledUpdate();
				fInterval = Time.time + updateInterval;
			}
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdated is called once per updateInterval
		// Protected to allow child classes to use it 
		// -------------------------------------------------------------------------------
		protected virtual void ThrottledUpdate() {}
        
        // -------------------------------------------------------------------------------
        
	}

}

// =======================================================================================