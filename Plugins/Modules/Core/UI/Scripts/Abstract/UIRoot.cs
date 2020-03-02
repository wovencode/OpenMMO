//by Fhiz
using OpenMMO;
using OpenMMO.UI;
using UnityEngine;

namespace OpenMMO.UI
{

	/// <summary>
    /// Based on Suriyun's UIBase class to provide a root class for all other UI elements.
	/// This class makes the Update function private and implements a ThrottledUpdate function 
	/// to be used instead. This way, all updates run at a throttled rate (that can be set
	/// via the inspector) instead of once per frame. The result is a major performance
	/// improvement, implemented right at the root of any UI element.
    /// </summary>
	public abstract partial class UIRoot : UIBase
	{
	
		[Header("UI Throttle")]
		public bool requiresActiveRoot = true;
		[Range(0.01f, 3f)]
		public float updateInterval = 0.25f;
		
		protected float fInterval = 0.01f;

		/// <summary>
   		/// Awake calls base awake.
    	/// </summary>
		protected override void Awake()
		{
			base.Awake();
		}
		
		/// <summary>
    	/// Update is called every frame. Throttled update is only called in certain intervals to lessen client load. Protected so that child class can still use it (it's sometimes required to gather user input).
   	 	/// </summary>
		protected virtual void Update()
		{
			if (updateInterval > 0 && Time.time > fInterval)
			{
				if (root.activeSelf || !requiresActiveRoot)
					ThrottledUpdate();
				fInterval = Time.time + updateInterval;
			}
		}
		
		/// <summary>
    	/// ThrottledUpdated is called once per 'updateInterval'. Protected to allow child classes to use it 
    	/// </summary>
		protected virtual void ThrottledUpdate() {}
        
	}

}